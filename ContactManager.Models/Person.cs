using ContactManager.Models.Enums;

namespace ContactManager.Models;

/// <summary>
/// Defines the information shared by every contact.
/// </summary>
public abstract class Person
{
	/// <summary>
	/// Gets or sets the stable identifier of the person.
	/// </summary>
	/// <remarks>
	///     Set on creation and should not change after that, thus init only.
	/// </remarks>
	public Guid Id { get; init; } = Guid.NewGuid();

	/// <summary>
	/// Gets or sets the person's salutation.
	/// </summary>
	public Title Title { get; set; } = Title.Unknown;

	/// <summary>
	/// Gets or sets the person's first name.
	/// </summary>
	public string FirstName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the person's last name.
	/// </summary>
	public string LastName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the person's date of birth.
	/// </summary>
	public DateOnly DateOfBirth { get; set; }

	/// <summary>
	/// Gets or sets the person's gender.
	/// </summary>
	public Gender Gender { get; set; }

	/// <summary>
	/// Gets or sets the person's job title.
	/// </summary>
	public string JobTitle { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the person's business phone number.
	/// </summary>
	public string BusinessNumber { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the person's mobile phone number.
	/// </summary>
	public string MobileNumber { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the person's email address.
	/// </summary>
	public string EmailAddress { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets whether the person is active.
	/// </summary>
	public bool IsActive { get; set; } = true;
}
