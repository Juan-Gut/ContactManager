using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Applies and persists individual contact mutations with in-memory rollback on save failure.
/// </summary>
internal sealed class ContactMutationService
{
	private readonly ContactData _data;
	private readonly EmployeeNrGenerator _employeeNumberGenerator;
	private readonly ContactMutationHistoryService _mutationHistoryService;
	private readonly IContactRepository _repository;
	private readonly ValidationService _validationService;

	/// <summary>Initializes the mutation service and its persistence collaborators.</summary>
	/// <param name="data">The in-memory contact data to mutate.</param>
	/// <param name="repository">The repository used to persist successful mutations.</param>
	/// <param name="validationService">The service used to validate contacts and notes.</param>
	/// <param name="employeeNumberGenerator">The generator used for new employee numbers.</param>
	/// <param name="mutationHistoryService">The service used to maintain mutation metadata.</param>
	internal ContactMutationService(
		ContactData data,
		IContactRepository repository,
		ValidationService validationService,
		EmployeeNrGenerator employeeNumberGenerator,
		ContactMutationHistoryService mutationHistoryService)
	{
		_data = data ?? throw new ArgumentNullException(nameof(data));
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		_validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
		_employeeNumberGenerator = employeeNumberGenerator
			?? throw new ArgumentNullException(nameof(employeeNumberGenerator));
		_mutationHistoryService = mutationHistoryService
			?? throw new ArgumentNullException(nameof(mutationHistoryService));
	}

	/// <summary>Adds and persists a new customer, employee, or apprentice.</summary>
	/// <param name="person">The contact to add.</param>
	internal void Add(Person person)
	{
		ArgumentNullException.ThrowIfNull(person);
		EnsureValid(person);

		if (ContactDataQuery.FindById(_data, person.Id) is not null)
		{
			throw new InvalidOperationException($"A person with identifier '{person.Id}' already exists.");
		}

		switch (person)
		{
			case Apprentice apprentice:
				AddEmployee(apprentice, _data.Apprentices);
				break;
			case Employee employee:
				AddEmployee(employee, _data.Employees);
				break;
			case Customer customer:
				AddAndSave(_data.Customers, customer);
				break;
			default:
				throw new ArgumentException(
					$"The person type '{person.GetType().Name}' is not supported.",
					nameof(person));
		}
	}

	/// <summary>Updates and persists an existing contact without allowing its type to change.</summary>
	/// <param name="person">The replacement contact values.</param>
	/// <returns><see langword="true"/> when the contact exists and was updated.</returns>
	internal bool Update(Person person)
	{
		ArgumentNullException.ThrowIfNull(person);

		Person? existingPerson = ContactDataQuery.FindById(_data, person.Id);
		if (existingPerson is null)
		{
			return false;
		}

		if (existingPerson.GetType() != person.GetType())
		{
			throw new ArgumentException("An existing person's contact type cannot be changed.", nameof(person));
		}

		if (person is Employee updatedEmployee && existingPerson is Employee existingEmployee)
		{
			updatedEmployee.EmployeeNumber = existingEmployee.EmployeeNumber;
		}

		EnsureValid(person);

		return person switch
		{
			Apprentice apprentice => ReplaceAndSave(_data.Apprentices, apprentice),
			Employee employee => ReplaceAndSave(_data.Employees, employee),
			Customer customer => ReplaceAndSave(_data.Customers, customer),
			_ => throw new ArgumentException(
				$"The person type '{person.GetType().Name}' is not supported.",
				nameof(person))
		};
	}

	/// <summary>Deletes and persists a contact and removes its mutation history.</summary>
	/// <param name="id">The identifier of the contact to delete.</param>
	/// <returns><see langword="true"/> when a contact was deleted.</returns>
	internal bool Delete(Guid id)
	{
		return RemoveAndSave(_data.Customers, id)
		       || RemoveAndSave(_data.Employees, id)
		       || RemoveAndSave(_data.Apprentices, id);
	}

	/// <summary>Changes and persists one contact's active state.</summary>
	/// <param name="id">The stable contact identifier.</param>
	/// <param name="isActive">The new active state.</param>
	/// <returns><see langword="true"/> when the contact exists.</returns>
	internal bool SetActive(Guid id, bool isActive)
	{
		Person? person = ContactDataQuery.FindById(_data, id);
		if (person is null)
		{
			return false;
		}

		if (person.IsActive == isActive)
		{
			return true;
		}

		bool previousValue = person.IsActive;
		person.IsActive = isActive;
		MutationLogEntry mutation = _mutationHistoryService.Add(
			_data,
			person,
			isActive ? "Activated" : "Deactivated");

		try
		{
			_repository.Save(_data);
			return true;
		}
		catch
		{
			person.IsActive = previousValue;
			_data.MutationHistory.Remove(mutation);
			throw;
		}
	}

	/// <summary>Adds and persists a timestamped note for a customer.</summary>
	/// <param name="customerId">The stable customer identifier.</param>
	/// <param name="note">The contact note to append.</param>
	/// <returns><see langword="true"/> when the customer exists.</returns>
	internal bool AddCustomerContact(Guid customerId, string note)
	{
		IReadOnlyList<string> errors = _validationService.ValidateContactNote(note);
		if (errors.Count > 0)
		{
			throw new ArgumentException(string.Join(Environment.NewLine, errors), nameof(note));
		}

		Customer? customer = _data.Customers.FirstOrDefault(storedCustomer => storedCustomer.Id == customerId);
		if (customer is null)
		{
			return false;
		}

		customer.ContactHistory ??= [];
		CustomerContactEntry entry = new()
		{
			Note = note.Trim()
		};
		customer.ContactHistory.Add(entry);
		MutationLogEntry mutation = _mutationHistoryService.Add(_data, customer, "Customer contact note added");

		try
		{
			_repository.Save(_data);
			return true;
		}
		catch
		{
			customer.ContactHistory.Remove(entry);
			_data.MutationHistory.Remove(mutation);
			throw;
		}
	}

	private void AddEmployee<TEmployee>(TEmployee employee, List<TEmployee> collection)
		where TEmployee : Employee
	{
		int previousEmployeeNumber = employee.EmployeeNumber;
		int previousNextEmployeeNumber = _data.NextEmployeeNumber;
		_employeeNumberGenerator.AssignNext(employee, _data);

		try
		{
			collection.Add(employee);
			_mutationHistoryService.Add(_data, employee, "Created");
			_repository.Save(_data);
		}
		catch
		{
			_mutationHistoryService.RemoveLatest(_data, employee.Id);
			collection.Remove(employee);
			employee.EmployeeNumber = previousEmployeeNumber;
			_data.NextEmployeeNumber = previousNextEmployeeNumber;
			throw;
		}
	}

	private void AddAndSave<TPerson>(List<TPerson> collection, TPerson person)
		where TPerson : Person
	{
		collection.Add(person);
		MutationLogEntry mutation = _mutationHistoryService.Add(_data, person, "Created");

		try
		{
			_repository.Save(_data);
		}
		catch
		{
			_data.MutationHistory.Remove(mutation);
			collection.Remove(person);
			throw;
		}
	}

	private void EnsureValid(Person person)
	{
		IReadOnlyList<string> errors = _validationService.Validate(person);
		if (errors.Count > 0)
		{
			throw new ArgumentException(string.Join(Environment.NewLine, errors));
		}
	}

	private bool RemoveAndSave<TPerson>(List<TPerson> collection, Guid id)
		where TPerson : Person
	{
		int index = collection.FindIndex(person => person.Id == id);
		if (index < 0)
		{
			return false;
		}

		TPerson person = collection[index];
		List<MutationLogEntry> previousMutationHistory = [.. _data.MutationHistory];
		collection.RemoveAt(index);
		_mutationHistoryService.RemoveAll(_data, person.Id);

		try
		{
			_repository.Save(_data);
			return true;
		}
		catch
		{
			collection.Insert(index, person);
			_data.MutationHistory = previousMutationHistory;
			throw;
		}
	}

	private bool ReplaceAndSave<TPerson>(List<TPerson> collection, TPerson person)
		where TPerson : Person
	{
		int index = collection.FindIndex(storedPerson => storedPerson.Id == person.Id);
		if (index < 0)
		{
			return false;
		}

		TPerson previousPerson = collection[index];
		collection[index] = person;
		MutationLogEntry mutation = _mutationHistoryService.Add(
			_data,
			person,
			_mutationHistoryService.DescribeUpdate(previousPerson, person));

		try
		{
			_repository.Save(_data);
			return true;
		}
		catch
		{
			_data.MutationHistory.Remove(mutation);
			collection[index] = previousPerson;
			throw;
		}
	}
}
