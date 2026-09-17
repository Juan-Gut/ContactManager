using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Validates, numbers, and persists a complete batch of imported contacts atomically.
/// </summary>
internal sealed class ContactBatchImportService
{
	private readonly ContactData _data;
	private readonly EmployeeNrGenerator _employeeNumberGenerator;
	private readonly ContactMutationHistoryService _mutationHistoryService;
	private readonly IContactRepository _repository;
	private readonly ValidationService _validationService;

	/// <summary>Initializes the batch importer and its persistence collaborators.</summary>
	/// <param name="data">The in-memory contact data receiving imported contacts.</param>
	/// <param name="repository">The repository used to persist the completed batch.</param>
	/// <param name="validationService">The service used to validate each candidate.</param>
	/// <param name="employeeNumberGenerator">The generator used for imported employees.</param>
	/// <param name="mutationHistoryService">The service used to record import metadata.</param>
	internal ContactBatchImportService(
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

	/// <summary>Imports a validated batch and persists it once.</summary>
	/// <param name="people">The contacts to import.</param>
	/// <returns>The number of imported contacts.</returns>
	internal int Import(IEnumerable<Person> people)
	{
		ArgumentNullException.ThrowIfNull(people);
		List<Person> contacts = people.ToList();
		if (contacts.Count == 0)
		{
			return 0;
		}

		ValidateBatch(contacts);

		int previousCustomerCount = _data.Customers.Count;
		int previousEmployeeCount = _data.Employees.Count;
		int previousApprenticeCount = _data.Apprentices.Count;
		int previousMutationCount = _data.MutationHistory.Count;
		int previousNextEmployeeNumber = _data.NextEmployeeNumber;
		Dictionary<Employee, int> previousEmployeeNumbers = contacts
			.OfType<Employee>()
			.ToDictionary(employee => employee, employee => employee.EmployeeNumber);

		try
		{
			foreach (Person contact in contacts)
			{
				AddImportedContact(contact);
				_mutationHistoryService.Add(_data, contact, "Imported from contact file");
			}

			_repository.Save(_data);
			return contacts.Count;
		}
		catch
		{
			RollbackBatch(
				previousCustomerCount,
				previousEmployeeCount,
				previousApprenticeCount,
				previousMutationCount,
				previousNextEmployeeNumber,
				previousEmployeeNumbers);
			throw;
		}
	}

	private void ValidateBatch(IEnumerable<Person> people)
	{
		HashSet<Guid> identifiers = ContactDataQuery.CreatePeopleSnapshot(_data)
			.Select(person => person.Id)
			.ToHashSet();

		foreach (Person contact in people)
		{
			IReadOnlyList<string> errors = _validationService.Validate(contact);
			if (errors.Count > 0)
			{
				throw new ArgumentException(string.Join(Environment.NewLine, errors));
			}

			if (!identifiers.Add(contact.Id))
			{
				throw new InvalidOperationException(
					$"A person with identifier '{contact.Id}' occurs more than once or already exists.");
			}

			if (contact is not Customer and not Employee)
			{
				throw new ArgumentException(
					$"The person type '{contact.GetType().Name}' is not supported.",
					nameof(people));
			}
		}
	}

	private void AddImportedContact(Person contact)
	{
		switch (contact)
		{
			case Apprentice apprentice:
				_employeeNumberGenerator.AssignNext(apprentice, _data);
				_data.Apprentices.Add(apprentice);
				break;
			case Employee employee:
				_employeeNumberGenerator.AssignNext(employee, _data);
				_data.Employees.Add(employee);
				break;
			case Customer customer:
				_data.Customers.Add(customer);
				break;
		}
	}

	private void RollbackBatch(
		int previousCustomerCount,
		int previousEmployeeCount,
		int previousApprenticeCount,
		int previousMutationCount,
		int previousNextEmployeeNumber,
		IReadOnlyDictionary<Employee, int> previousEmployeeNumbers)
	{
		_data.Customers.RemoveRange(previousCustomerCount, _data.Customers.Count - previousCustomerCount);
		_data.Employees.RemoveRange(previousEmployeeCount, _data.Employees.Count - previousEmployeeCount);
		_data.Apprentices.RemoveRange(previousApprenticeCount, _data.Apprentices.Count - previousApprenticeCount);
		_data.MutationHistory.RemoveRange(previousMutationCount, _data.MutationHistory.Count - previousMutationCount);
		_data.NextEmployeeNumber = previousNextEmployeeNumber;
		foreach ((Employee employee, int employeeNumber) in previousEmployeeNumbers)
		{
			employee.EmployeeNumber = employeeNumber;
		}
	}
}
