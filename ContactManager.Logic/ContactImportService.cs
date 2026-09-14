using System.Globalization;
using System.Text;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.Logic;

/// <summary>
/// Reads contacts from CSV and vCard files and validates them without persisting data.
/// </summary>
public sealed class ContactImportService
{
	private const long MaximumFileSize = 5 * 1024 * 1024;
	private static readonly string[] RequiredCsvHeaders = ["Type", "FirstName", "LastName", "DateOfBirth"];
	private static readonly HashSet<string> SupportedCsvHeaders = new(StringComparer.OrdinalIgnoreCase)
	{
		"Type", "Title", "FirstName", "LastName", "DateOfBirth", "Gender", "JobTitle",
		"BusinessNumber", "MobileNumber", "EmailAddress", "IsActive", "Company", "Department",
		"AhvNumber", "Nationality", "City", "Address", "Plz", "EmploymentStartDate",
		"EmploymentEndDate", "EmploymentPercentage", "OfficeLocation", "ManagementLevel",
		"ApprenticeshipDuration", "CurrentApprenticeshipYear"
	};

	private readonly ValidationService validationService;

	/// <summary>Initializes a contact import service using the application validation rules.</summary>
	public ContactImportService()
		: this(new ValidationService())
	{
	}

	/// <summary>Initializes a contact import service with an explicit validation service.</summary>
	/// <param name="validationService">The service used to validate parsed contacts.</param>
	public ContactImportService(ValidationService validationService)
	{
		this.validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
	}

	/// <summary>
	/// Reads and validates a supported contact import file.
	/// </summary>
	/// <param name="filePath">The CSV or vCard file to read.</param>
	/// <returns>A preview containing valid contacts and all detected issues.</returns>
	/// <exception cref="IOException">Thrown when the file cannot be read.</exception>
	/// <exception cref="UnauthorizedAccessException">Thrown when the file cannot be accessed.</exception>
	public ContactImportResult ReadFile(string filePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		FileInfo file = new(filePath);
		if (!file.Exists)
		{
			throw new FileNotFoundException("The selected import file no longer exists.", filePath);
		}

		if (file.Length > MaximumFileSize)
		{
			return Failure("File", "The selected file is larger than the supported limit of 5 MB.");
		}

		string content = File.ReadAllText(file.FullName, Encoding.UTF8);
		return file.Extension.ToLowerInvariant() switch
		{
			".csv" => ReadCsv(content),
			".vcf" or ".vcard" => ReadVCard(content),
			_ => Failure("File", "Only CSV (.csv) and vCard (.vcf, .vcard) files are supported.")
		};
	}

	/// <summary>Parses CSV text using comma or semicolon delimiters.</summary>
	/// <param name="content">The complete CSV content.</param>
	/// <returns>The import preview.</returns>
	public ContactImportResult ReadCsv(string content)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return Failure("File", "The CSV file is empty.");
		}

		List<ContactImportCandidate> candidates = [];
		List<ContactImportIssue> issues = [];
		char delimiter = DetectCsvDelimiter(content);
		List<CsvRecord> records;

		try
		{
			records = ParseCsvRecords(content, delimiter);
		}
		catch (FormatException exception)
		{
			return Failure("File", exception.Message);
		}

		if (records.Count == 0)
		{
			return Failure("File", "The CSV file does not contain a header row.");
		}

		string[] headers = records[0].Values.Select(value => value.Trim().TrimStart('\uFEFF')).ToArray();
		Dictionary<string, int> columns = new(StringComparer.OrdinalIgnoreCase);
		for (int index = 0; index < headers.Length; index++)
		{
			string header = headers[index];
			if (string.IsNullOrWhiteSpace(header))
			{
				issues.Add(new ContactImportIssue("Header", $"Column {index + 1} has no name."));
				continue;
			}

			if (!columns.TryAdd(header, index))
			{
				issues.Add(new ContactImportIssue("Header", $"Column '{header}' occurs more than once."));
			}
			else if (!SupportedCsvHeaders.Contains(header))
			{
				issues.Add(new ContactImportIssue(
					"Header",
					$"Column '{header}' is not supported and will be ignored.",
					ContactImportIssueSeverity.Warning));
			}
		}

		foreach (string requiredHeader in RequiredCsvHeaders)
		{
			if (!columns.ContainsKey(requiredHeader))
			{
				issues.Add(new ContactImportIssue("Header", $"Required column '{requiredHeader}' is missing."));
			}
		}

		if (issues.Any(issue => issue.Severity == ContactImportIssueSeverity.Error))
		{
			return new ContactImportResult(candidates.AsReadOnly(), issues.AsReadOnly());
		}

		foreach (CsvRecord record in records.Skip(1).Where(record => record.Values.Any(value => !string.IsNullOrWhiteSpace(value))))
		{
			string source = $"CSV row {record.LineNumber}";
			if (record.Values.Count > headers.Length)
			{
				issues.Add(new ContactImportIssue(source, "The row contains more values than the header."));
				continue;
			}

			Dictionary<string, string> values = columns
				.Where(column => SupportedCsvHeaders.Contains(column.Key))
				.ToDictionary(
					column => column.Key,
					column => column.Value < record.Values.Count ? record.Values[column.Value].Trim() : string.Empty,
					StringComparer.OrdinalIgnoreCase);

			TryCreateCandidate(values, source, candidates, issues);
		}

		return new ContactImportResult(candidates.AsReadOnly(), issues.AsReadOnly());
	}

	/// <summary>Parses vCard 3.0 or 4.0 text.</summary>
	/// <param name="content">The complete vCard content.</param>
	/// <returns>The import preview.</returns>
	public ContactImportResult ReadVCard(string content)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return Failure("File", "The vCard file is empty.");
		}

		List<ContactImportCandidate> candidates = [];
		List<ContactImportIssue> issues = [];
		List<List<string>> cards = SplitVCards(content, issues);

		for (int index = 0; index < cards.Count; index++)
		{
			string source = $"vCard {index + 1}";
			int issueCountBeforeMapping = issues.Count;
			Dictionary<string, string> values = MapVCard(cards[index], source, issues);
			if (issues.Skip(issueCountBeforeMapping)
				.Any(issue => issue.Severity == ContactImportIssueSeverity.Error))
			{
				continue;
			}
			TryCreateCandidate(values, source, candidates, issues);
		}

		if (cards.Count == 0 && issues.Count == 0)
		{
			issues.Add(new ContactImportIssue("File", "No vCard entries were found."));
		}

		return new ContactImportResult(candidates.AsReadOnly(), issues.AsReadOnly());
	}

	private void TryCreateCandidate(
		IReadOnlyDictionary<string, string> values,
		string source,
		ICollection<ContactImportCandidate> candidates,
		ICollection<ContactImportIssue> issues)
	{
		List<string> rowErrors = [];
		string typeValue = Get(values, "Type");
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
			return;
		}

		contact.Title = ParseEnum(Get(values, "Title"), Title.Unknown, "Title", rowErrors);
		contact.FirstName = Get(values, "FirstName");
		contact.LastName = Get(values, "LastName");
		contact.DateOfBirth = ParseRequiredDate(Get(values, "DateOfBirth"), "DateOfBirth", rowErrors);
		contact.Gender = ParseEnum(Get(values, "Gender"), Gender.Unknown, "Gender", rowErrors);
		contact.JobTitle = Get(values, "JobTitle");
		contact.BusinessNumber = Get(values, "BusinessNumber");
		contact.MobileNumber = Get(values, "MobileNumber");
		contact.EmailAddress = Get(values, "EmailAddress");
		contact.IsActive = ParseBoolean(Get(values, "IsActive"), true, "IsActive", rowErrors);

		if (contact is Customer customer)
		{
			customer.Company = Get(values, "Company");
		}

		if (contact is Employee employee)
		{
			employee.Department = Get(values, "Department");
			employee.AhvNumber = Get(values, "AhvNumber");
			employee.Nationality = Get(values, "Nationality");
			employee.City = Get(values, "City");
			employee.Address = Get(values, "Address");
			employee.Plz = Get(values, "Plz");
			employee.EmploymentStartDate = ParseRequiredDate(
				Get(values, "EmploymentStartDate"),
				"EmploymentStartDate",
				rowErrors);
			employee.EmploymentEndDate = ParseOptionalDate(
				Get(values, "EmploymentEndDate"),
				DateOnly.MaxValue,
				"EmploymentEndDate",
				rowErrors);
			employee.EmploymentPercentage = ParseUnsignedShort(
				Get(values, "EmploymentPercentage"),
				100,
				"EmploymentPercentage",
				rowErrors);
			employee.OfficeLocation = ParseEnum(
				Get(values, "OfficeLocation"),
				OfficeLocation.Unknown,
				"OfficeLocation",
				rowErrors);
			employee.ManagementLevel = ParseEnum(
				Get(values, "ManagementLevel"),
				ManagementLevel.None,
				"ManagementLevel",
				rowErrors);
		}

		if (contact is Apprentice apprentice)
		{
			apprentice.ApprenticeshipDuration = ParseUnsignedShort(
				Get(values, "ApprenticeshipDuration"),
				0,
				"ApprenticeshipDuration",
				rowErrors);
			apprentice.CurrentApprenticeshipYear = ParseUnsignedShort(
				Get(values, "CurrentApprenticeshipYear"),
				0,
				"CurrentApprenticeshipYear",
				rowErrors);
		}

		rowErrors.AddRange(validationService.Validate(contact));
		foreach (string error in rowErrors.Distinct())
		{
			issues.Add(new ContactImportIssue(source, error));
		}

		if (rowErrors.Count == 0)
		{
			candidates.Add(new ContactImportCandidate(source, contact));
		}
	}

	private static Dictionary<string, string> MapVCard(
		IEnumerable<string> lines,
		string source,
		ICollection<ContactImportIssue> issues)
	{
		Dictionary<string, string> values = new(StringComparer.OrdinalIgnoreCase)
		{
			["Type"] = "Customer"
		};
		string? formattedName = null;

		foreach (string line in lines)
		{
			int separatorIndex = FindUnescaped(line, ':');
			if (separatorIndex <= 0)
			{
				issues.Add(new ContactImportIssue(source, $"Invalid vCard line '{line}'."));
				continue;
			}

			string descriptor = line[..separatorIndex];
			string rawValue = line[(separatorIndex + 1)..];
			string rawName = descriptor.Split(';')[0];
			string name = rawName.Contains('.') ? rawName[(rawName.LastIndexOf('.') + 1)..] : rawName;
			string value = UnescapeVCard(rawValue).Trim();

			switch (name.ToUpperInvariant())
			{
				case "VERSION":
					if (value is not ("3.0" or "4.0"))
					{
						issues.Add(new ContactImportIssue(
							source,
							$"vCard version '{value}' is not officially supported; import will be attempted.",
							ContactImportIssueSeverity.Warning));
					}
					break;
				case "FN":
					formattedName = value;
					break;
				case "N":
					List<string> nameParts = SplitVCardValue(rawValue, ';');
					values["LastName"] = ElementAtOrEmpty(nameParts, 0);
					values["FirstName"] = ElementAtOrEmpty(nameParts, 1);
					if (nameParts.Count > 3)
					{
						values["Title"] = NormalizeTitle(ElementAtOrEmpty(nameParts, 3));
					}
					break;
				case "BDAY": values["DateOfBirth"] = value; break;
				case "TITLE": values["JobTitle"] = value; break;
				case "EMAIL": values.TryAdd("EmailAddress", value); break;
				case "ORG": values["Company"] = SplitVCardValue(rawValue, ';').FirstOrDefault() ?? string.Empty; break;
				case "TEL":
					if (descriptor.Contains("CELL", StringComparison.OrdinalIgnoreCase))
					{
						values.TryAdd("MobileNumber", value);
					}
					else if (descriptor.Contains("WORK", StringComparison.OrdinalIgnoreCase))
					{
						values.TryAdd("BusinessNumber", value);
					}
					else if (!values.TryAdd("MobileNumber", value))
					{
						values.TryAdd("BusinessNumber", value);
					}
					break;
				case "ADR":
					List<string> addressParts = SplitVCardValue(rawValue, ';');
					values.TryAdd("Address", ElementAtOrEmpty(addressParts, 2));
					values.TryAdd("City", ElementAtOrEmpty(addressParts, 3));
					values.TryAdd("Plz", ElementAtOrEmpty(addressParts, 5));
					break;
				case "X-CONTACTMANAGER-TYPE": values["Type"] = value; break;
				case "X-CONTACTMANAGER-TITLE": values["Title"] = value; break;
				case "X-CONTACTMANAGER-GENDER": values["Gender"] = value; break;
				case "X-CONTACTMANAGER-IS-ACTIVE": values["IsActive"] = value; break;
				case "X-CONTACTMANAGER-DEPARTMENT": values["Department"] = value; break;
				case "X-CONTACTMANAGER-AHV-NUMBER": values["AhvNumber"] = value; break;
				case "X-CONTACTMANAGER-NATIONALITY": values["Nationality"] = value; break;
				case "X-CONTACTMANAGER-EMPLOYMENT-START-DATE": values["EmploymentStartDate"] = value; break;
				case "X-CONTACTMANAGER-EMPLOYMENT-END-DATE": values["EmploymentEndDate"] = value; break;
				case "X-CONTACTMANAGER-EMPLOYMENT-PERCENTAGE": values["EmploymentPercentage"] = value; break;
				case "X-CONTACTMANAGER-OFFICE-LOCATION": values["OfficeLocation"] = value; break;
				case "X-CONTACTMANAGER-MANAGEMENT-LEVEL": values["ManagementLevel"] = value; break;
				case "X-CONTACTMANAGER-APPRENTICESHIP-DURATION": values["ApprenticeshipDuration"] = value; break;
				case "X-CONTACTMANAGER-CURRENT-APPRENTICESHIP-YEAR": values["CurrentApprenticeshipYear"] = value; break;
			}
		}

		if ((!values.TryGetValue("FirstName", out string? firstName) || string.IsNullOrWhiteSpace(firstName))
			&& (!values.TryGetValue("LastName", out string? lastName) || string.IsNullOrWhiteSpace(lastName))
			&& !string.IsNullOrWhiteSpace(formattedName))
		{
			string[] parts = formattedName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
			values["FirstName"] = parts[0];
			values["LastName"] = parts.Length > 1 ? parts[1] : string.Empty;
		}

		return values;
	}

	private static List<List<string>> SplitVCards(string content, ICollection<ContactImportIssue> issues)
	{
		string normalized = content.Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n');
		List<string> unfoldedLines = [];
		foreach (string line in normalized.Split('\n'))
		{
			if ((line.StartsWith(' ') || line.StartsWith('\t')) && unfoldedLines.Count > 0)
			{
				unfoldedLines[^1] += line[1..];
			}
			else
			{
				unfoldedLines.Add(line.TrimEnd());
			}
		}

		List<List<string>> cards = [];
		List<string>? currentCard = null;
		foreach (string line in unfoldedLines)
		{
			if (line.Equals("BEGIN:VCARD", StringComparison.OrdinalIgnoreCase))
			{
				if (currentCard is not null)
				{
					issues.Add(new ContactImportIssue("File", "A vCard begins before the previous vCard ends."));
				}
				currentCard = [];
			}
			else if (line.Equals("END:VCARD", StringComparison.OrdinalIgnoreCase))
			{
				if (currentCard is null)
				{
					issues.Add(new ContactImportIssue("File", "A vCard end marker has no matching begin marker."));
				}
				else
				{
					cards.Add(currentCard);
					currentCard = null;
				}
			}
			else if (currentCard is not null && !string.IsNullOrWhiteSpace(line))
			{
				currentCard.Add(line);
			}
		}

		if (currentCard is not null)
		{
			issues.Add(new ContactImportIssue("File", "The final vCard has no END:VCARD marker."));
		}

		return cards;
	}

	private static List<CsvRecord> ParseCsvRecords(string content, char delimiter)
	{
		List<CsvRecord> records = [];
		List<string> fields = [];
		StringBuilder field = new();
		bool inQuotes = false;
		int lineNumber = 1;
		int recordLineNumber = 1;

		for (int index = 0; index < content.Length; index++)
		{
			char character = content[index];
			if (inQuotes)
			{
				if (character == '"' && index + 1 < content.Length && content[index + 1] == '"')
				{
					field.Append('"');
					index++;
				}
				else if (character == '"')
				{
					inQuotes = false;
				}
				else
				{
					field.Append(character);
					if (character == '\n')
					{
						lineNumber++;
					}
				}
			}
			else if (character == '"' && field.Length == 0)
			{
				inQuotes = true;
			}
			else if (character == delimiter)
			{
				fields.Add(field.ToString());
				field.Clear();
			}
			else if (character is '\r' or '\n')
			{
				if (character == '\r' && index + 1 < content.Length && content[index + 1] == '\n')
				{
					index++;
				}
				fields.Add(field.ToString());
				field.Clear();
				records.Add(new CsvRecord(recordLineNumber, [.. fields]));
				fields.Clear();
				lineNumber++;
				recordLineNumber = lineNumber;
			}
			else
			{
				field.Append(character);
			}
		}

		if (inQuotes)
		{
			throw new FormatException($"A quoted CSV value beginning near line {recordLineNumber} is not closed.");
		}

		if (field.Length > 0 || fields.Count > 0)
		{
			fields.Add(field.ToString());
			records.Add(new CsvRecord(recordLineNumber, [.. fields]));
		}

		return records;
	}

	private static char DetectCsvDelimiter(string content)
	{
		string firstLine = content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
		int commas = CountUnquoted(firstLine, ',');
		int semicolons = CountUnquoted(firstLine, ';');
		return semicolons > commas ? ';' : ',';
	}

	private static int CountUnquoted(string value, char character)
	{
		bool inQuotes = false;
		int count = 0;
		foreach (char current in value)
		{
			if (current == '"')
			{
				inQuotes = !inQuotes;
			}
			else if (current == character && !inQuotes)
			{
				count++;
			}
		}
		return count;
	}

	private static ContactImportResult Failure(string source, string message)
	{
		return new ContactImportResult(
			Array.Empty<ContactImportCandidate>(),
			new[] { new ContactImportIssue(source, message) });
	}

	private static string Get(IReadOnlyDictionary<string, string> values, string name)
	{
		return values.TryGetValue(name, out string? value) ? value.Trim() : string.Empty;
	}

	private static DateOnly ParseRequiredDate(string value, string fieldName, ICollection<string> errors)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return default;
		}

		return ParseOptionalDate(value, default, fieldName, errors);
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
		if (DateOnly.TryParseExact(normalized, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
		{
			return date;
		}

		errors.Add($"{fieldName} '{value}' is invalid. Use yyyy-MM-dd or dd.MM.yyyy.");
		return defaultValue;
	}

	private static bool ParseBoolean(string value, bool defaultValue, string fieldName, ICollection<string> errors)
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

	private static List<string> SplitVCardValue(string value, char separator)
	{
		List<string> parts = [];
		StringBuilder part = new();
		bool escaped = false;
		foreach (char character in value)
		{
			if (escaped)
			{
				part.Append(character is 'n' or 'N' ? '\n' : character);
				escaped = false;
			}
			else if (character == '\\')
			{
				escaped = true;
			}
			else if (character == separator)
			{
				parts.Add(part.ToString().Trim());
				part.Clear();
			}
			else
			{
				part.Append(character);
			}
		}
		parts.Add(part.ToString().Trim());
		return parts;
	}

	private static int FindUnescaped(string value, char character)
	{
		bool escaped = false;
		for (int index = 0; index < value.Length; index++)
		{
			if (!escaped && value[index] == character)
			{
				return index;
			}
			escaped = !escaped && value[index] == '\\';
		}
		return -1;
	}

	private static string UnescapeVCard(string value)
	{
		return value
			.Replace("\\n", "\n", StringComparison.OrdinalIgnoreCase)
			.Replace("\\,", ",", StringComparison.Ordinal)
			.Replace("\\;", ";", StringComparison.Ordinal)
			.Replace("\\\\", "\\", StringComparison.Ordinal);
	}

	private static string ElementAtOrEmpty(IReadOnlyList<string> values, int index)
	{
		return index < values.Count ? values[index] : string.Empty;
	}

	private static string NormalizeTitle(string value)
	{
		return value.Trim().TrimEnd('.').ToLowerInvariant() switch
		{
			"mr" or "herr" => nameof(Title.Mr),
			"mrs" or "frau" => nameof(Title.Mrs),
			"ms" => nameof(Title.Ms),
			_ => value
		};
	}

	private sealed record CsvRecord(int LineNumber, IReadOnlyList<string> Values);
}
