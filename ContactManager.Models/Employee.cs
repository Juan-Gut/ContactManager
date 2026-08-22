using ContactManager.Models.Enums;

namespace ContactManager.Models;

/// <summary>
/// Represents an employee of the company.
/// </summary>
public class Employee : Person
{
	/// <summary>
	/// Gets or sets the automatically assigned employee number.
	/// </summary>
	/// <remarks>
	///     A unique number assigned to each employee upon creation and should not change
	/// </remarks>
	public int EmployeeNumber { get; set; }

	/// <summary>
	/// Gets or sets the employee's department.
	/// </summary>
	public string Department { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's Swiss social security number.
	/// </summary>
	public string AhvNumber { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's place of residence.
	/// </summary>
	public string Nationality { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's nationality.
	/// </summary>
	public string City { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's street address.
	/// </summary>
	public string Address { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's postal code.
	/// </summary>
	public string Plz { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the date on which employment started.
	/// </summary>
	public DateOnly EmploymentStartDate { get; set; }

	/// <summary>
	/// Gets or sets the date on which employment ended, if applicable.
	/// </summary>
	public DateOnly EmploymentEndDate { get; set; } = DateOnly.MaxValue;

	/// <summary>
	/// Gets or sets the employment percentage from 0 to 100.
	/// </summary>
	public ushort EmploymentPercentage { get; set; } = 100;

	/// <summary>
	/// Gets or sets the management level from 0 to 5.
	/// </summary>
	public OfficeLocation OfficeLocation { get; set; }

	/// <summary>
	///     Gets or sets the management level (0 to 5).
	/// </summary>
	public ManagementLevel ManagementLevel { get; set; }
}
