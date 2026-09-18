using System.Globalization;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.Logic;

/// <summary>
/// Maps canonical import fields to a contact model and applies the shared business validation rules.
/// </summary>
internal sealed class ContactImportCandidateFactory
{
	private readonly ValidationService _validationService;

	/// <summary>Initializes the factory with the validation service used by normal contact mutations.</summary>
	/// <param name="validationService">The service used to validate constructed contacts.</param>
	internal ContactImportCandidateFactory(ValidationService validationService)
	{
		_validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
	}

	/// <summary>Creates a validated candidate or records every issue that prevents its import.</summary>
	/// <param name="values">Canonical import field names and their source values.</param>
	/// <param name="source">The row or vCard identifier shown in diagnostics.</param>
	/// <param name="issues">The collection that receives conversion and validation issues.</param>
	/// <returns>A valid candidate, or <see langword="null"/> when the source entry is invalid.</returns>
	internal ContactImportCandidate? Create(
		IReadOnlyDictionary<string, string> values,
		string source,
		ICollection<ContactImportIssue> issues)
	{
		List<string> entryErrors = [];
		string typeValue = GetValue(values, "Type");
		Person? contact = typeValue.ToLowerInvariant() switch
		{
			"customer" or "kunde" => new Customer(),
			"employee" or "mitarbeiter" => new Employee(),
			"apprentice" or "lernender" or "lernende" => new Apprentice(),
			_ => null
		};

		if (contact is null)
		{
			issues.Add(new ContactImportIssue(
				source,
				$"Contact type '{typeValue}' is invalid. Use Customer, Employee or Apprentice."));
			return null;
		}

		MapSharedFields(contact, values, entryErrors);
		if (contact is Customer customer)
		{
			customer.Company = GetValue(values, "Company");
		}

		if (contact is Employee employee)
		{
			MapEmployeeFields(employee, values, entryErrors);
		}

		if (contact is Apprentice apprentice)
		{
			MapApprenticeFields(apprentice, values, entryErrors);
		}

		entryErrors.AddRange(_validationService.Validate(contact));
		foreach (string error in entryErrors.Distinct())
		{
			issues.Add(new ContactImportIssue(source, error));
		}

		return entryErrors.Count == 0
			? new ContactImportCandidate(source, contact)
			: null;
	}

	private static void MapSharedFields(
		Person contact,
		IReadOnlyDictionary<string, string> values,
		ICollection<string> errors)
	{
		contact.Title = ParseEnum(GetValue(values, "Title"), Title.Unknown, "Title", errors);
		contact.FirstName = GetValue(values, "FirstName");
		contact.LastName = GetValue(values, "LastName");
		contact.DateOfBirth = ParseRequiredDate(GetValue(values, "DateOfBirth"), "DateOfBirth", errors);
		contact.Gender = ParseEnum(GetValue(values, "Gender"), Gender.Unknown, "Gender", errors);
		contact.JobTitle = GetValue(values, "JobTitle");
		contact.BusinessNumber = GetValue(values, "BusinessNumber");
		contact.MobileNumber = GetValue(values, "MobileNumber");
		contact.EmailAddress = GetValue(values, "EmailAddress");
		contact.IsActive = ParseBoolean(GetValue(values, "IsActive"), true, "IsActive", errors);
	}

	private static void MapEmployeeFields(
		Employee employee,
		IReadOnlyDictionary<string, string> values,
		ICollection<string> errors)
	{
		employee.Department = GetValue(values, "Department");
		employee.AhvNumber = GetValue(values, "AhvNumber");
		employee.Nationality = GetValue(values, "Nationality");
		employee.City = GetValue(values, "City");
		employee.Address = GetValue(values, "Address");
		employee.Plz = GetValue(values, "Plz");
		employee.EmploymentStartDate = ParseRequiredDate(
			GetValue(values, "EmploymentStartDate"),
			"EmploymentStartDate",
			errors);
		employee.EmploymentEndDate = ParseOptionalDate(
			GetValue(values, "EmploymentEndDate"),
			DateOnly.MaxValue,
			"EmploymentEndDate",
			errors);
		employee.EmploymentPercentage = ParseUnsignedShort(
			GetValue(values, "EmploymentPercentage"),
			100,
			"EmploymentPercentage",
			errors);
		employee.OfficeLocation = ParseEnum(
			GetValue(values, "OfficeLocation"),
			OfficeLocation.Unknown,
			"OfficeLocation",
			errors);
		employee.ManagementLevel = ParseEnum(
			GetValue(values, "ManagementLevel"),
			ManagementLevel.None,
			"ManagementLevel",
			errors);
	}

	private static void MapApprenticeFields(
		Apprentice apprentice,
		IReadOnlyDictionary<string, string> values,
		ICollection<string> errors)
	{
		apprentice.ApprenticeshipDuration = ParseUnsignedShort(
			GetValue(values, "ApprenticeshipDuration"),
			0,
			"ApprenticeshipDuration",
			errors);
		apprentice.CurrentApprenticeshipYear = ParseUnsignedShort(
			GetValue(values, "CurrentApprenticeshipYear"),
			0,
			"CurrentApprenticeshipYear",
			errors);
	}

	private static string GetValue(IReadOnlyDictionary<string, string> values, string name)
	{
		return values.TryGetValue(name, out string? value) ? value.Trim() : string.Empty;
	}

	private static DateOnly ParseRequiredDate(string value, string fieldName, ICollection<string> errors)
	{
		return string.IsNullOrWhiteSpace(value)
			? default
			: ParseOptionalDate(value, default, fieldName, errors);
	}

	private static DateOnly ParseOptionalDate(
		string value,
		DateOnly defaultValue,
		string fieldName,
		ICollection<string> errors)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return defaultValue;
		}

		string normalized = value.Length == 8 && value.All(char.IsDigit)
			? $"{value[..4]}-{value.Substring(4, 2)}-{value[6..]}"
			: value;
		string[] formats = ["yyyy-MM-dd", "dd.MM.yyyy"];
		if (DateOnly.TryParseExact(
			normalized,
			formats,
			CultureInfo.InvariantCulture,
			DateTimeStyles.None,
			out DateOnly date))
		{
			return date;
		}

		errors.Add($"{fieldName} '{value}' is invalid. Use yyyy-MM-dd or dd.MM.yyyy.");
		return defaultValue;
	}

	private static bool ParseBoolean(
		string value,
		bool defaultValue,
		string fieldName,
		ICollection<string> errors)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return defaultValue;
		}

		if (value.Equals("true", StringComparison.OrdinalIgnoreCase)
			|| value.Equals("yes", StringComparison.OrdinalIgnoreCase)
			|| value.Equals("active", StringComparison.OrdinalIgnoreCase)
			|| value == "1")
		{
			return true;
		}

		if (value.Equals("false", StringComparison.OrdinalIgnoreCase)
			|| value.Equals("no", StringComparison.OrdinalIgnoreCase)
			|| value.Equals("inactive", StringComparison.OrdinalIgnoreCase)
			|| value.Equals("passive", StringComparison.OrdinalIgnoreCase)
			|| value == "0")
		{
			return false;
		}

		errors.Add($"{fieldName} '{value}' is invalid. Use true or false.");
		return defaultValue;
	}

	private static ushort ParseUnsignedShort(
		string value,
		ushort defaultValue,
		string fieldName,
		ICollection<string> errors)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return defaultValue;
		}

		if (ushort.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out ushort parsedValue))
		{
			return parsedValue;
		}

		errors.Add($"{fieldName} '{value}' must be a whole number between 0 and 65535.");
		return defaultValue;
	}

	private static TEnum ParseEnum<TEnum>(
		string value,
		TEnum defaultValue,
		string fieldName,
		ICollection<string> errors)
		where TEnum : struct, Enum
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return defaultValue;
		}

		if (Enum.TryParse(value, true, out TEnum parsedValue) && Enum.IsDefined(parsedValue))
		{
			return parsedValue;
		}

		string allowedValues = string.Join(", ", Enum.GetNames<TEnum>());
		errors.Add($"{fieldName} '{value}' is invalid. Allowed values: {allowedValues}.");
		return defaultValue;
	}
}
