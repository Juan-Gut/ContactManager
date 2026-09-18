using System.Text;

namespace ContactManager.Logic;

/// <summary>
/// Parses CSV contact data into canonical import candidates and row-level issues.
/// </summary>
internal sealed class CsvContactImportParser
{
	private static readonly string[] _requiredHeaders = ["Type", "FirstName", "LastName", "DateOfBirth"];
	private static readonly HashSet<string> _supportedHeaders = new(StringComparer.OrdinalIgnoreCase)
	{
		"Type", "Title", "FirstName", "LastName", "DateOfBirth", "Gender", "JobTitle",
		"BusinessNumber", "MobileNumber", "EmailAddress", "IsActive", "Company", "Department",
		"AhvNumber", "Nationality", "City", "Address", "Plz", "EmploymentStartDate",
		"EmploymentEndDate", "EmploymentPercentage", "OfficeLocation", "ManagementLevel",
		"ApprenticeshipDuration", "CurrentApprenticeshipYear"
	};

	private readonly ContactImportCandidateFactory _candidateFactory;

	/// <summary>Initializes the parser with the factory that maps canonical fields to contacts.</summary>
	/// <param name="candidateFactory">The factory used for model creation and validation.</param>
	internal CsvContactImportParser(ContactImportCandidateFactory candidateFactory)
	{
		_candidateFactory = candidateFactory ?? throw new ArgumentNullException(nameof(candidateFactory));
	}

	/// <summary>Parses comma- or semicolon-delimited contact data.</summary>
	/// <param name="content">The complete CSV content.</param>
	/// <returns>Valid candidates and all parsing or validation issues.</returns>
	internal ContactImportResult Parse(string content)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return ContactImportResultFactory.CreateFailure("File", "The CSV file is empty.");
		}

		List<ContactImportCandidate> candidates = [];
		List<ContactImportIssue> issues = [];
		char delimiter = DetectDelimiter(content);
		List<CsvRecord> records;

		try
		{
			records = ParseRecords(content, delimiter);
		}
		catch (FormatException exception)
		{
			return ContactImportResultFactory.CreateFailure("File", exception.Message);
		}

		if (records.Count == 0)
		{
			return ContactImportResultFactory.CreateFailure("File", "The CSV file does not contain a header row.");
		}

		string[] headers = records[0].Values.Select(value => value.Trim().TrimStart('\uFEFF')).ToArray();
		Dictionary<string, int> columns = BuildColumnIndex(headers, issues);
		AddMissingRequiredHeaderIssues(columns, issues);

		if (issues.Any(issue => issue.Severity == ContactImportIssueSeverity.Error))
		{
			return new ContactImportResult(candidates.AsReadOnly(), issues.AsReadOnly());
		}

		foreach (CsvRecord record in records.Skip(1)
			.Where(record => record.Values.Any(value => !string.IsNullOrWhiteSpace(value))))
		{
			string source = $"CSV row {record.LineNumber}";
			if (record.Values.Count > headers.Length)
			{
				issues.Add(new ContactImportIssue(source, "The row contains more values than the header."));
				continue;
			}

			Dictionary<string, string> values = columns
				.Where(column => _supportedHeaders.Contains(column.Key))
				.ToDictionary(
					column => column.Key,
					column => column.Value < record.Values.Count ? record.Values[column.Value].Trim() : string.Empty,
					StringComparer.OrdinalIgnoreCase);

			ContactImportCandidate? candidate = _candidateFactory.Create(values, source, issues);
			if (candidate is not null)
			{
				candidates.Add(candidate);
			}
		}

		return new ContactImportResult(candidates.AsReadOnly(), issues.AsReadOnly());
	}

	private static Dictionary<string, int> BuildColumnIndex(
		IReadOnlyList<string> headers,
		ICollection<ContactImportIssue> issues)
	{
		Dictionary<string, int> columns = new(StringComparer.OrdinalIgnoreCase);
		for (int index = 0; index < headers.Count; index++)
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
			else if (!_supportedHeaders.Contains(header))
			{
				issues.Add(new ContactImportIssue(
					"Header",
					$"Column '{header}' is not supported and will be ignored.",
					ContactImportIssueSeverity.Warning));
			}
		}

		return columns;
	}

	private static void AddMissingRequiredHeaderIssues(
		IReadOnlyDictionary<string, int> columns,
		ICollection<ContactImportIssue> issues)
	{
		foreach (string requiredHeader in _requiredHeaders)
		{
			if (!columns.ContainsKey(requiredHeader))
			{
				issues.Add(new ContactImportIssue("Header", $"Required column '{requiredHeader}' is missing."));
			}
		}
	}

	private static List<CsvRecord> ParseRecords(string content, char delimiter)
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

	private static char DetectDelimiter(string content)
	{
		string firstLine = content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()
			?? string.Empty;
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

	private sealed record CsvRecord(int LineNumber, IReadOnlyList<string> Values);
}
