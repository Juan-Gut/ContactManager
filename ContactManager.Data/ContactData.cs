using ContactManager.Models;

namespace ContactManager.Data;

/// <summary>
/// Contains all data persisted by the Contact Manager.
/// </summary>
public sealed class ContactData
{
	/// <summary>
	/// Gets or sets the version of the persisted data structure.
	/// </summary>
	/// <remarks>
	/// Having a schema version allows for future changes to the data structure while maintaining backwards compatibility.
	/// </remarks>
	public int SchemaVersion { get; init; } = 1;

	/// <summary>
	/// Gets or sets the employee number assigned to the next new employee.
	/// </summary>
	/// <remarks>
	/// This should only be incremented from the EmployeeNrGenerator on employee creation.
	/// </remarks>
	public int NextEmployeeNumber { get; set; } = 1000;

	/// <summary>
	/// Gets or sets the stored customers.
	/// </summary>
	public List<Customer> Customers { get; set; } = [];

	/// <summary>
	/// Gets or sets the stored employees who are not apprentices.
	/// </summary>
	public List<Employee> Employees { get; set; } = [];

	/// <summary>
	/// Gets or sets the stored apprentices.
	/// </summary>
	public List<Apprentice> Apprentices { get; set; } = [];
}
