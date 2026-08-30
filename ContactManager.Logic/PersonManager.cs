using System.Globalization;
using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Manages contacts and persists every successful mutation.
/// </summary>
public sealed class PersonManager
{
	/// <summary>Stores the loaded customers, employees, and apprentices.</summary>
	private readonly ContactData _data;
	/// <summary>Assigns employee numbers when employees are added.</summary>
	private readonly EmployeeNrGenerator _employeeNrGenerator;
	/// <summary>Persists contact data.</summary>
	private readonly IContactRepository _repository;
	/// <summary>Validates people and customer contact notes.</summary>
	private readonly ValidationService _validationService;

	/// <summary>
	/// Initializes a new instance of the <see cref="PersonManager"/> class and loads all contact data.
	/// </summary>
	/// <param name="repository">The repository used to load and save contact data.</param>
	public PersonManager(IContactRepository repository)
		: this(repository, new ValidationService(), new EmployeeNrGenerator())
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="PersonManager"/> class with its services.
	/// </summary>
	/// <param name="repository">The repository used to load and save contact data.</param>
	/// <param name="validationService">The service used to validate mutations.</param>
	/// <param name="employeeNrGenerator">The service used to assign employee numbers.</param>
	public PersonManager(
		IContactRepository repository,
		ValidationService validationService,
		EmployeeNrGenerator employeeNrGenerator)
	{
		_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		_validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
		_employeeNrGenerator = employeeNrGenerator ?? throw new ArgumentNullException(nameof(employeeNrGenerator));
		_data = _repository.Load()
		        ?? throw new InvalidOperationException("The contact repository returned no contact data.");

		_data.Customers ??= [];
		_data.Employees ??= [];
		_data.Apprentices ??= [];
	}

	/// <summary>
	/// Gets all customers, employees, and apprentices.
	/// </summary>
	/// <returns>A read-only snapshot of all people.</returns>
	public IReadOnlyList<Person> GetAll()
	{
		return _data.Customers
			.Cast<Person>()
			.Concat(_data.Employees)
			.Concat(_data.Apprentices)
			.ToList()
			.AsReadOnly();
	}

	/// <summary>
	/// Gets a person by their stable identifier.
	/// </summary>
	/// <param name="id">The identifier to find.</param>
	/// <returns>The matching person, or <see langword="null"/> when no person exists.</returns>
	public Person? GetById(Guid id)
	{
		return GetAll().FirstOrDefault(person => person.Id == id);
	}

	/// <summary>
	/// Gets the next employee number that will be assigned to a new employee.
	/// </summary>
	/// <returns>The next available employee number.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when no further employee number can be assigned.
	/// </exception>
	public int GetNextEmployeeNumber()
	{
		return _employeeNrGenerator.GetNextAvailable(_data);
	}

	/// <summary>
	/// Adds and persists a new customer, employee, or apprentice.
	/// </summary>
	/// <param name="person">The person to add.</param>
	/// <exception cref="ArgumentException">Thrown when the person is invalid.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the identifier is already in use.</exception>
	public void Add(Person person)
	{
		ArgumentNullException.ThrowIfNull(person);
		EnsureValid(person);

		if (GetById(person.Id) is not null)
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

	/// <summary>
	/// Updates and persists an existing person.
	/// </summary>
	/// <param name="person">The updated person.</param>
	/// <returns><see langword="true"/> when the person was updated; otherwise, <see langword="false"/>.</returns>
	/// <exception cref="ArgumentException">Thrown when the person is invalid or its type changed.</exception>
	public bool Update(Person person)
	{
		ArgumentNullException.ThrowIfNull(person);

		Person? existingPerson = GetById(person.Id);
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

	/// <summary>
	/// Deletes and persists a person.
	/// </summary>
	/// <param name="id">The identifier of the person to delete.</param>
	/// <returns><see langword="true"/> when the person was deleted; otherwise, <see langword="false"/>.</returns>
	public bool Delete(Guid id)
	{
		return RemoveAndSave(_data.Customers, id)
		       || RemoveAndSave(_data.Employees, id)
		       || RemoveAndSave(_data.Apprentices, id);
	}

	/// <summary>
	/// Activates or deactivates a customer and persists the change.
	/// </summary>
	/// <param name="id">The identifier of the customer to change.</param>
	/// <param name="isActive">The new active state.</param>
	/// <returns><see langword="true"/> when the customer exists; otherwise, <see langword="false"/>.</returns>
	public bool SetActive(Guid id, bool isActive)
	{
		Customer? customer = _data.Customers.FirstOrDefault(storedCustomer => storedCustomer.Id == id);
		if (customer is null)
		{
			return false;
		}

		if (customer.IsActive == isActive)
		{
			return true;
		}

		bool previousValue = customer.IsActive;
		customer.IsActive = isActive;

		try
		{
			_repository.Save(_data);
			return true;
		}
		catch
		{
			customer.IsActive = previousValue;
			throw;
		}
	}

	/// <summary>
	/// Searches useful fields across all contact types.
	/// </summary>
	/// <param name="searchText">The case-insensitive text to find.</param>
	/// <returns>A read-only snapshot of matching people.</returns>
	public IReadOnlyList<Person> Search(string? searchText)
	{
		if (string.IsNullOrWhiteSpace(searchText))
		{
			return GetAll();
		}

		string normalizedSearchText = searchText.Trim();
		return GetAll()
			.Where(person => GetSearchValues(person).Any(value => Contains(value, normalizedSearchText)))
			.ToList()
			.AsReadOnly();
	}

	/// <summary>
	/// Appends and persists a timestamped note in a customer's contact history.
	/// </summary>
	/// <param name="customerId">The identifier of the customer.</param>
	/// <param name="note">The contact note.</param>
	/// <returns><see langword="true"/> when the customer exists; otherwise, <see langword="false"/>.</returns>
	/// <exception cref="ArgumentException">Thrown when the note is invalid.</exception>
	public bool AddCustomerContact(Guid customerId, string note)
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

		try
		{
			_repository.Save(_data);
			return true;
		}
		catch
		{
			customer.ContactHistory.Remove(entry);
			throw;
		}
	}

	/// <summary>
	/// Determines whether a value contains search text without regard to case.
	/// </summary>
	/// <param name="value">The value to search.</param>
	/// <param name="searchText">The text to find.</param>
	/// <returns><see langword="true"/> when the value contains the search text; otherwise, <see langword="false"/>.</returns>
	private static bool Contains(string? value, string searchText)
	{
		return value?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
	}

	/// <summary>
	/// Enumerates searchable values for a person.
	/// </summary>
	/// <param name="person">The person whose values should be searched.</param>
	/// <returns>The searchable values associated with the person.</returns>
	private static IEnumerable<string?> GetSearchValues(Person person)
	{
		yield return person.FirstName;
		yield return person.LastName;
		yield return $"{person.FirstName} {person.LastName}";
		yield return person.JobTitle;
		yield return person.BusinessNumber;
		yield return person.MobileNumber;
		yield return person.EmailAddress;
		yield return person.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		yield return person.CreatedAt.ToString("O", CultureInfo.InvariantCulture);
		yield return person.Title.ToString();
		yield return person.Gender.ToString();
		if (person is Customer customer)
		{
			yield return customer.IsActive ? "active" : "inactive";
		}

		if (person is Customer customerWithHistory)
		{
			yield return customerWithHistory.Company;
			foreach (CustomerContactEntry entry in customerWithHistory.ContactHistory ?? [])
			{
				yield return entry.Note;
			}
		}

		if (person is Employee employee)
		{
			yield return employee.EmployeeNumber.ToString(CultureInfo.InvariantCulture);
			yield return employee.Department;
			yield return employee.AhvNumber;
			yield return employee.Nationality;
			yield return employee.City;
			yield return employee.Address;
			yield return employee.Plz;
			yield return employee.EmploymentStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			yield return employee.EmploymentEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
			yield return employee.EmploymentPercentage.ToString(CultureInfo.InvariantCulture);
			yield return employee.OfficeLocation.ToString();
			yield return employee.ManagementLevel.ToString();
		}

		if (person is Apprentice apprentice)
		{
			yield return apprentice.ApprenticeshipDuration.ToString(CultureInfo.InvariantCulture);
			yield return apprentice.CurrentApprenticeshipYear.ToString(CultureInfo.InvariantCulture);
		}
	}

	/// <summary>
	/// Assigns an employee number, adds an employee, and persists the change.
	/// </summary>
	/// <typeparam name="TEmployee">The employee type being added.</typeparam>
	/// <param name="employee">The employee to add.</param>
	/// <param name="collection">The collection receiving the employee.</param>
	private void AddEmployee<TEmployee>(TEmployee employee, List<TEmployee> collection)
		where TEmployee : Employee
	{
		int previousEmployeeNumber = employee.EmployeeNumber;
		int previousNextEmployeeNumber = _data.NextEmployeeNumber;
		_employeeNrGenerator.AssignNext(employee, _data);

		try
		{
			collection.Add(employee);
			_repository.Save(_data);
		}
		catch
		{
			collection.Remove(employee);
			employee.EmployeeNumber = previousEmployeeNumber;
			_data.NextEmployeeNumber = previousNextEmployeeNumber;
			throw;
		}
	}

	/// <summary>
	/// Adds a person to a collection and persists the change.
	/// </summary>
	/// <typeparam name="TPerson">The person type being added.</typeparam>
	/// <param name="collection">The collection receiving the person.</param>
	/// <param name="person">The person to add.</param>
	private void AddAndSave<TPerson>(List<TPerson> collection, TPerson person)
		where TPerson : Person
	{
		collection.Add(person);

		try
		{
			_repository.Save(_data);
		}
		catch
		{
			collection.Remove(person);
			throw;
		}
	}

	/// <summary>
	/// Validates a person and throws when validation errors exist.
	/// </summary>
	/// <param name="person">The person to validate.</param>
	private void EnsureValid(Person person)
	{
		IReadOnlyList<string> errors = _validationService.Validate(person);
		if (errors.Count > 0)
		{
			throw new ArgumentException(string.Join(Environment.NewLine, errors), nameof(person));
		}
	}

	/// <summary>
	/// Removes a person from a collection and persists the change.
	/// </summary>
	/// <typeparam name="TPerson">The person type stored in the collection.</typeparam>
	/// <param name="collection">The collection from which to remove the person.</param>
	/// <param name="id">The identifier of the person to remove.</param>
	/// <returns><see langword="true"/> when a person was removed; otherwise, <see langword="false"/>.</returns>
	private bool RemoveAndSave<TPerson>(List<TPerson> collection, Guid id)
		where TPerson : Person
	{
		int index = collection.FindIndex(person => person.Id == id);
		if (index < 0)
		{
			return false;
		}

		TPerson person = collection[index];
		collection.RemoveAt(index);

		try
		{
			_repository.Save(_data);
			return true;
		}
		catch
		{
			collection.Insert(index, person);
			throw;
		}
	}

	/// <summary>
	/// Replaces a person in a collection and persists the change.
	/// </summary>
	/// <typeparam name="TPerson">The person type stored in the collection.</typeparam>
	/// <param name="collection">The collection containing the person.</param>
	/// <param name="person">The replacement person.</param>
	/// <returns><see langword="true"/> when a person was replaced; otherwise, <see langword="false"/>.</returns>
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

		try
		{
			_repository.Save(_data);
			return true;
		}
		catch
		{
			collection[index] = previousPerson;
			throw;
		}
	}
}
