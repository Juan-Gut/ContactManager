using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Coordinates contact queries and mutations while preserving the application's public use-case API.
/// </summary>
public sealed class PersonManager
{
	private readonly ContactBatchImportService _batchImportService;
	private readonly ContactData _data;
	private readonly EmployeeNrGenerator _employeeNumberGenerator;
	private readonly ContactMutationHistoryService _mutationHistoryService;
	private readonly ContactMutationService _mutationService;
	private readonly ContactSearchService _searchService;

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
		ArgumentNullException.ThrowIfNull(repository);
		ArgumentNullException.ThrowIfNull(validationService);
		ArgumentNullException.ThrowIfNull(employeeNrGenerator);

		_data = repository.Load()
		       ?? throw new InvalidOperationException("The contact repository returned no contact data.");
		_data.Customers ??= [];
		_data.Employees ??= [];
		_data.Apprentices ??= [];
		_data.MutationHistory ??= [];

		_employeeNumberGenerator = employeeNrGenerator;
		_mutationHistoryService = new ContactMutationHistoryService();
		_searchService = new ContactSearchService();
		_mutationService = new ContactMutationService(
			_data,
			repository,
			validationService,
			employeeNrGenerator,
			_mutationHistoryService);
		_batchImportService = new ContactBatchImportService(
			_data,
			repository,
			validationService,
			employeeNrGenerator,
			_mutationHistoryService);
	}

	/// <summary>
	/// Gets all customers, employees, and apprentices.
	/// </summary>
	/// <returns>A read-only snapshot of all people.</returns>
	public IReadOnlyList<Person> GetAll()
	{
		return ContactDataQuery.CreatePeopleSnapshot(_data);
	}

	/// <summary>
	/// Gets a person by their stable identifier.
	/// </summary>
	/// <param name="id">The identifier to find.</param>
	/// <returns>The matching person, or <see langword="null"/> when no person exists.</returns>
	public Person? GetById(Guid id)
	{
		return ContactDataQuery.FindById(_data, id);
	}

	/// <summary>
	/// Gets the next employee number that will be assigned to a new employee.
	/// </summary>
	/// <returns>The next available employee number.</returns>
	/// <exception cref="InvalidOperationException">Thrown when no further employee number can be assigned.</exception>
	public int GetNextEmployeeNumber()
	{
		return _employeeNumberGenerator.GetNextAvailable(_data);
	}

	/// <summary>
	/// Gets the metadata-only mutation history for a contact.
	/// </summary>
	/// <param name="contactId">The stable identifier of the contact.</param>
	/// <returns>A read-only snapshot ordered from newest to oldest.</returns>
	public IReadOnlyList<MutationLogEntry> GetMutationHistory(Guid contactId)
	{
		return _mutationHistoryService.GetForContact(_data, contactId);
	}

	/// <summary>
	/// Adds and persists a new customer, employee, or apprentice.
	/// </summary>
	/// <param name="person">The person to add.</param>
	/// <exception cref="ArgumentException">Thrown when the person is invalid.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the identifier is already in use.</exception>
	public void Add(Person person)
	{
		_mutationService.Add(person);
	}

	/// <summary>
	/// Adds a validated collection of imported contacts and persists the complete batch once.
	/// </summary>
	/// <param name="people">The customers, employees, and apprentices to import.</param>
	/// <returns>The number of imported contacts.</returns>
	/// <remarks>
	/// The operation is atomic from the application's perspective: if validation, employee-number
	/// assignment, or persistence fails, no contact from the batch remains in memory.
	/// </remarks>
	/// <exception cref="ArgumentException">Thrown when a contact is invalid or has an unsupported type.</exception>
	/// <exception cref="InvalidOperationException">Thrown when an identifier is duplicated.</exception>
	public int Import(IEnumerable<Person> people)
	{
		return _batchImportService.Import(people);
	}

	/// <summary>
	/// Updates and persists an existing person.
	/// </summary>
	/// <param name="person">The updated person.</param>
	/// <returns><see langword="true"/> when the person was updated; otherwise, <see langword="false"/>.</returns>
	/// <exception cref="ArgumentException">Thrown when the person is invalid or its type changed.</exception>
	public bool Update(Person person)
	{
		return _mutationService.Update(person);
	}

	/// <summary>
	/// Deletes and persists a person.
	/// </summary>
	/// <param name="id">The identifier of the person to delete.</param>
	/// <returns><see langword="true"/> when the person was deleted; otherwise, <see langword="false"/>.</returns>
	public bool Delete(Guid id)
	{
		return _mutationService.Delete(id);
	}

	/// <summary>
	/// Activates or deactivates a person and persists the change.
	/// </summary>
	/// <param name="id">The identifier of the person to change.</param>
	/// <param name="isActive">The new active state.</param>
	/// <returns><see langword="true"/> when the person exists; otherwise, <see langword="false"/>.</returns>
	public bool SetActive(Guid id, bool isActive)
	{
		return _mutationService.SetActive(id, isActive);
	}

	/// <summary>
	/// Searches all user-visible contact fields of all contact types.
	/// Shared fields are searched for every person, while customer- and employee-specific
	/// fields are searched only on the applicable contact type.
	/// </summary>
	/// <param name="searchText">The case-insensitive text to find.</param>
	/// <returns>A read-only snapshot of matching people.</returns>
	public IReadOnlyList<Person> Search(string? searchText)
	{
		return _searchService.Search(GetAll(), searchText);
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
		return _mutationService.AddCustomerContact(customerId, note);
	}
}
