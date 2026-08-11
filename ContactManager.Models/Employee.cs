namespace ContactManager.Models;

/// <summary>
/// Represents an employee of the company.
/// </summary>
public class Employee : Person
{
	/// <summary>
	/// Gets or sets the automatically assigned employee number.
	/// </summary>
	public int EmployeeNumber { get; set; }

	/// <summary>
	/// Gets or sets the employee's department.
	/// </summary>
	public string Department { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's Swiss social security number.
	/// </summary>
	public string SocialSecurityNumber { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's place of residence.
	/// </summary>
	public string PlaceOfResidence { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's nationality.
	/// </summary>
	public string Nationality { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's street address.
	/// </summary>
	public string Address { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the employee's postal code.
	/// </summary>
	public string PostalCode { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the date on which employment started.
	/// </summary>
	public DateOnly EmploymentStartDate { get; set; }

	/// <summary>
	/// Gets or sets the date on which employment ended, if applicable.
	/// </summary>
	public DateOnly? EmploymentEndDate { get; set; }

	/// <summary>
	/// Gets or sets the employment percentage from 0 to 100.
	/// </summary>
	public decimal EmploymentPercentage { get; set; }

	/// <summary>
	/// Gets or sets the management level from 0 to 5.
	/// </summary>
	public int ManagementLevel { get; set; }
}
