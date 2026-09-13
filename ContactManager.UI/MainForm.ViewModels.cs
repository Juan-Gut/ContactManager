using ContactManager.Logic;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.UI;

public partial class MainForm
{
	/// <summary>Represents the identifying customer data shown in the customer list.</summary>
	private sealed class CustomerListRow
	{
		/// <summary>Gets the stable customer identifier.</summary>
		public Guid Id { get; init; }

		/// <summary>Gets the creation date.</summary>
		public DateOnly CreatedAt { get; init; }

		/// <summary>Gets the customer's title.</summary>
		public Title Title { get; init; }

		/// <summary>Gets the customer's first name.</summary>
		public string FirstName { get; init; } = string.Empty;

		/// <summary>Gets the customer's last name.</summary>
		public string LastName { get; init; } = string.Empty;

		/// <summary>Gets the customer's date of birth.</summary>
		public DateOnly DateOfBirth { get; init; }

		/// <summary>Gets the customer's gender.</summary>
		public Gender Gender { get; init; }

		/// <summary>Gets the customer's job title.</summary>
		public string JobTitle { get; init; } = string.Empty;

		/// <summary>Gets the customer's business phone number.</summary>
		public string BusinessNumber { get; init; } = string.Empty;

		/// <summary>Gets the customer's mobile phone number.</summary>
		public string MobileNumber { get; init; } = string.Empty;

		/// <summary>Gets the customer's email address.</summary>
		public string EmailAddress { get; init; } = string.Empty;

		/// <summary>Gets the customer's display name.</summary>
		public string CustomerName { get; init; } = string.Empty;

		/// <summary>Gets the customer's company.</summary>
		public string Company { get; init; } = string.Empty;

		/// <summary>Gets the customer's email address.</summary>
		public string Email { get; init; } = string.Empty;

		/// <summary>Gets the customer's preferred displayed phone number.</summary>
		public string Phone { get; init; } = string.Empty;

		/// <summary>Gets the customer's active-state display text.</summary>
		public string Status { get; init; } = string.Empty;

		/// <summary>Gets the number of customer contact-history entries.</summary>
		public int ContactHistoryCount { get; init; }
	}

	/// <summary>Represents a customer contact note shown in the note-history list.</summary>
	private sealed class CustomerContactNoteRow
	{
		/// <summary>Gets the stable note identifier.</summary>
		public Guid Id { get; init; }

		/// <summary>Gets the note creation date and time without a timezone suffix.</summary>
		public string CreatedAt { get; init; } = string.Empty;

		/// <summary>Gets the shortened note text shown in the list.</summary>
		public string Preview { get; init; } = string.Empty;

		/// <summary>Gets the complete note text.</summary>
		public string Note { get; init; } = string.Empty;
	}

	/// <summary>Represents a metadata-only mutation shown in a contact's history view.</summary>
	private sealed class MutationHistoryRow
	{
		/// <summary>Gets the date and time at which the mutation completed.</summary>
		public string ChangedAt { get; init; } = string.Empty;

		/// <summary>Gets the action that was completed.</summary>
		public string Action { get; init; } = string.Empty;
	}

	/// <summary>Represents the identifying employee data shown in the employee list.</summary>
	private sealed class EmployeeListRow
	{
		/// <summary>Gets the stable employee identifier.</summary>
		public Guid Id { get; init; }

		/// <summary>Gets the creation date.</summary>
		public DateOnly CreatedAt { get; init; }

		/// <summary>Gets the employee's title.</summary>
		public Title Title { get; init; }

		/// <summary>Gets the employee's first name.</summary>
		public string FirstName { get; init; } = string.Empty;

		/// <summary>Gets the employee's last name.</summary>
		public string LastName { get; init; } = string.Empty;

		/// <summary>Gets the employee's date of birth.</summary>
		public DateOnly DateOfBirth { get; init; }

		/// <summary>Gets the employee's gender.</summary>
		public Gender Gender { get; init; }

		/// <summary>Gets the employee's job title.</summary>
		public string JobTitle { get; init; } = string.Empty;

		/// <summary>Gets the employee's business phone number.</summary>
		public string BusinessNumber { get; init; } = string.Empty;

		/// <summary>Gets the employee's mobile phone number.</summary>
		public string MobileNumber { get; init; } = string.Empty;

		/// <summary>Gets the employee's email address.</summary>
		public string EmailAddress { get; init; } = string.Empty;

		/// <summary>Gets the automatically assigned employee number.</summary>
		public int EmployeeNumber { get; init; }

		/// <summary>Gets the employee's display name.</summary>
		public string Name { get; init; } = string.Empty;

		/// <summary>Gets the employee's department.</summary>
		public string Department { get; init; } = string.Empty;

		/// <summary>Gets the employee's displayed employment end date.</summary>
		public string EmploymentEndDate { get; init; } = string.Empty;

		/// <summary>Gets the employee type display text.</summary>
		public string EmployeeType { get; init; } = string.Empty;

		/// <summary>Gets the employee's office location.</summary>
		public OfficeLocation OfficeLocation { get; init; }

		/// <summary>Gets the employee's management level.</summary>
		public ManagementLevel ManagementLevel { get; init; }

		/// <summary>Gets the employee's active-state display text.</summary>
		public string Status { get; init; } = string.Empty;

		/// <summary>Gets the apprenticeship duration in years.</summary>
		public ushort ApprenticeshipDuration { get; init; }

		/// <summary>Gets the apprentice's current apprenticeship year.</summary>
		public ushort CurrentApprenticeshipYear { get; init; }
	}

	/// <summary>Represents a person displayed in the upcoming-birthdays dashboard list.</summary>
	private sealed class UpcomingBirthdayRow
	{
		/// <summary>Gets the person's display name.</summary>
		public string PersonName { get; init; } = string.Empty;

		/// <summary>Gets the person's contact type.</summary>
		public string ContactType { get; init; } = string.Empty;

		/// <summary>Gets the person's date of birth.</summary>
		public DateOnly DateOfBirth { get; init; }

		/// <summary>Gets the next occurrence of the person's birthday.</summary>
		public DateOnly NextBirthday { get; init; }
	}

	/// <summary>Represents an employee displayed in the upcoming-departures dashboard list.</summary>
	private sealed class UpcomingDepartureRow
	{
		/// <summary>Gets the employee number.</summary>
		public int EmployeeNumber { get; init; }

		/// <summary>Gets the employee's display name.</summary>
		public string Name { get; init; } = string.Empty;

		/// <summary>Gets the employee's department.</summary>
		public string Department { get; init; } = string.Empty;

		/// <summary>Gets the employment end date.</summary>
		public DateOnly EndDate { get; init; }
	}
}
