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

		if (string.IsNullOrWhiteSpace(person.EmailAddress))
		{
			errors.Add("The email address is required.");
		}
		else if (!string.IsNullOrWhiteSpace(person.EmailAddress)
		         && !MailAddress.TryCreate(person.EmailAddress, out _))
		{
			errors.Add("The email address is invalid.");
		}

		if (!IsValidPhoneNumber(person.BusinessNumber))
		{
			errors.Add(
				"The business phone number may contain only numbers and an optional leading '+' for country codes.");
		}

		if (!IsValidPhoneNumber(person.MobileNumber))
		{
			errors.Add(
				"The mobile phone number may contain only numbers and an optional leading '+' for country codes.");
		}

		if (person is Customer customer)
		{
			if (string.IsNullOrWhiteSpace(customer.JobTitle))
			{
				errors.Add("The job title is required.");
			}

			if (string.IsNullOrWhiteSpace(customer.Company))
			{
				errors.Add("The company is required.");
			}
		}

		if (person is Employee employee)
		{
			ValidateEmployee(employee, errors);
		}

		return errors.AsReadOnly();
	}

	/// <summary>
	/// Determines whether a phone number is empty or contains an optional leading plus sign followed by digits.
	/// Whitespace is ignored so formatted numbers can include spaces.
	/// </summary>
	/// <param name="phoneNumber">The phone number to validate.</param>
	/// <returns><see langword="true"/> when the phone number uses the supported format; otherwise, <see langword="false"/>.</returns>
	private static bool IsValidPhoneNumber(string? phoneNumber)
	{
		if (string.IsNullOrWhiteSpace(phoneNumber))
		{
			return true;
		}

		string normalizedPhoneNumber = string.Concat(
			phoneNumber.Where(character => !char.IsWhiteSpace(character)));
		int firstDigitIndex = normalizedPhoneNumber[0] == '+' ? 1 : 0;
		if (firstDigitIndex == normalizedPhoneNumber.Length)
		{
			// if the first and only digit is a + sign
			return false;
		}

		return normalizedPhoneNumber[firstDigitIndex..]
			.All(character => character is >= '0' and <= '9');
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

	/// <summary>
	/// Adds validation errors for employee-specific rules.
	/// </summary>
	/// <param name="employee">The employee to validate.</param>
	/// <param name="errors">The collection receiving validation errors.</param>
	private static void ValidateEmployee(Employee employee, ICollection<string> errors)
	{
		if (string.IsNullOrWhiteSpace(employee.JobTitle))
		{
			errors.Add("The job title is required.");
		}

		if (string.IsNullOrWhiteSpace(employee.Department))
		{
			errors.Add("The department is required.");
		}

		if (string.IsNullOrWhiteSpace(employee.AhvNumber))
		{
			errors.Add("The AHV number is required.");
		}
		else if (!IsValidAhvNumber(employee.AhvNumber))
		{
			errors.Add("The AHV number must follow the Swiss format (Start with 756 and contain exactly 13 digits).");
		}

		if (string.IsNullOrWhiteSpace(employee.Nationality))
		{
			errors.Add("The nationality is required.");
		}

		if (string.IsNullOrWhiteSpace(employee.City))
		{
			errors.Add("The city is required.");
		}

		if (string.IsNullOrWhiteSpace(employee.Address))
		{
			errors.Add("The address is required.");
		}

		if (string.IsNullOrWhiteSpace(employee.Plz))
		{
							errors.Add("The PLZ is required.");
		}
		else if (!employee.Plz.All(character => character is >= '0' and <= '9'))
		{
			errors.Add("The PLZ may contain only digits.");
		}

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

	/// <summary>
	/// Determines whether an AHV number has the expected 13-digit Swiss format.
	/// </summary>
	/// <param name="ahvNumber">The AHV number to validate.</param>
	/// <returns><see langword="true"/> when the normalized number starts with 756 and contains 13 digits; otherwise, <see langword="false"/>.</returns>
	private static bool IsValidAhvNumber(string ahvNumber)
	{
		string normalizedAhvNumber = string.Concat(
			ahvNumber.Where(character => !char.IsWhiteSpace(character) && character != '.'));

		return normalizedAhvNumber.Length == 13
			&& normalizedAhvNumber.StartsWith("756", StringComparison.Ordinal)
			&& normalizedAhvNumber.All(character => character is >= '0' and <= '9');
	}
}
