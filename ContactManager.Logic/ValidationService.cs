using System.Net.Mail;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Validates contacts and customer-contact notes before they are persisted.
/// </summary>
public sealed class ValidationService
{
	/// <summary>
	/// Validates a person against the business rules used by the contact manager.
	/// </summary>
	/// <param name="person">The person to validate.</param>
	/// <returns>A read-only list containing all validation errors.</returns>
	public IReadOnlyList<string> Validate(Person person)
	{
		ArgumentNullException.ThrowIfNull(person);

		List<string> errors = [];

		if (person.Id == Guid.Empty)
		{
			errors.Add("A person must have an identifier.");
		}

		if (person.CreatedAt == default)
		{
			errors.Add("A person must have a creation timestamp.");
		}

		if (string.IsNullOrWhiteSpace(person.FirstName))
		{
			errors.Add("The first name is required.");
		}

		if (string.IsNullOrWhiteSpace(person.LastName))
		{
			errors.Add("The last name is required.");
		}

		if (person.DateOfBirth == default)
		{
			errors.Add("The date of birth is required.");
		}
		else if (person.DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
		{
			errors.Add("The date of birth cannot be in the future.");
		}

		if (!string.IsNullOrWhiteSpace(person.EmailAddress)
			&& !MailAddress.TryCreate(person.EmailAddress, out _))
		{
			errors.Add("The email address is invalid.");
		}

		if (person is Customer customer && string.IsNullOrWhiteSpace(customer.Company))
		{
			errors.Add("The company is required for a customer.");
		}

		if (person is Employee employee)
		{
			ValidateEmployee(employee, errors);
		}

		return errors.AsReadOnly();
	}

	/// <summary>
	/// Validates the text used for a customer-contact history entry.
	/// </summary>
	/// <param name="note">The contact note to validate.</param>
	/// <returns>A read-only list containing all validation errors.</returns>
	public IReadOnlyList<string> ValidateContactNote(string? note)
	{
		List<string> errors = [];

		if (string.IsNullOrWhiteSpace(note))
		{
			errors.Add("The contact note is required.");
		}

		return errors.AsReadOnly();
	}

	private static void ValidateEmployee(Employee employee, ICollection<string> errors)
	{
		// We allow 16-year-old employees due to the apprenticeship program
		DateOnly latestAllowedDateOfBirth = DateOnly.FromDateTime(DateTime.Today).AddYears(-16);

		if (employee.DateOfBirth > latestAllowedDateOfBirth)
		{
			errors.Add("An employee must be at least 16 years old.");
		}

		if (employee.EmploymentPercentage is < 5 or > 100)
		{
			errors.Add("The employee must be employed between 5% and 100%.");
		}

		if (employee.EmploymentStartDate == default)
		{
			errors.Add("The employment start date is required.");
		}

		if (employee.EmploymentEndDate < employee.EmploymentStartDate)
		{
			errors.Add("The employment end date cannot be before the start date.");
		}

		if (employee is not Apprentice apprentice)
		{
			return;
		}

		if (apprentice.ApprenticeshipDuration is <= 0 or > 4)
		{
			errors.Add("The apprenticeship duration must be between 1 and 4 years.");
		}

		if (apprentice.CurrentApprenticeshipYear <= 0 ||
			apprentice.CurrentApprenticeshipYear > apprentice.ApprenticeshipDuration)
		{
			errors.Add("The current apprenticeship year must be within the apprenticeship duration.");
		}
	}
}
