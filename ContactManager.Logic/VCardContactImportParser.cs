using System.Text;
using ContactManager.Models.Enums;

namespace ContactManager.Logic;

/// <summary>
/// Parses vCard 3.0 or 4.0 data into canonical import candidates and card-level issues.
/// </summary>
internal sealed class VCardContactImportParser
{
	private readonly ContactImportCandidateFactory _candidateFactory;

	/// <summary>Initializes the parser with the factory that maps canonical fields to contacts.</summary>
	/// <param name="candidateFactory">The factory used for model creation and validation.</param>
	internal VCardContactImportParser(ContactImportCandidateFactory candidateFactory)
	{
		_candidateFactory = candidateFactory ?? throw new ArgumentNullException(nameof(candidateFactory));
	}

	/// <summary>Parses complete vCard content.</summary>
	/// <param name="content">The complete vCard content.</param>
	/// <returns>Valid candidates and all parsing or validation issues.</returns>
	internal ContactImportResult Parse(string content)
	{
		if (string.IsNullOrWhiteSpace(content))
		{
			return ContactImportResultFactory.CreateFailure("File", "The vCard file is empty.");
		}

		List<ContactImportCandidate> candidates = [];
		List<ContactImportIssue> issues = [];
		List<List<string>> cards = SplitCards(content, issues);

		for (int index = 0; index < cards.Count; index++)
		{
			string source = $"vCard {index + 1}";
			int issueCountBeforeMapping = issues.Count;
			Dictionary<string, string> values = MapCardFields(cards[index], source, issues);
			if (issues.Skip(issueCountBeforeMapping)
				.Any(issue => issue.Severity == ContactImportIssueSeverity.Error))
			{
				continue;
			}

			ContactImportCandidate? candidate = _candidateFactory.Create(values, source, issues);
			if (candidate is not null)
			{
				candidates.Add(candidate);
			}
		}

		if (cards.Count == 0 && issues.Count == 0)
		{
			issues.Add(new ContactImportIssue("File", "No vCard entries were found."));
		}

		return new ContactImportResult(candidates.AsReadOnly(), issues.AsReadOnly());
	}

	private static Dictionary<string, string> MapCardFields(
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
			string value = UnescapeValue(rawValue).Trim();

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
					List<string> nameParts = SplitValue(rawValue, ';');
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
				case "ORG": values["Company"] = SplitValue(rawValue, ';').FirstOrDefault() ?? string.Empty; break;
				case "TEL":
					MapTelephone(descriptor, value, values);
					break;
				case "ADR":
					List<string> addressParts = SplitValue(rawValue, ';');
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

		ApplyFormattedNameFallback(values, formattedName);
		return values;
	}

	private static void MapTelephone(
		string descriptor,
		string value,
		IDictionary<string, string> values)
	{
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
	}

	private static void ApplyFormattedNameFallback(
		IDictionary<string, string> values,
		string? formattedName)
	{
		if ((!values.TryGetValue("FirstName", out string? firstName) || string.IsNullOrWhiteSpace(firstName))
			&& (!values.TryGetValue("LastName", out string? lastName) || string.IsNullOrWhiteSpace(lastName))
			&& !string.IsNullOrWhiteSpace(formattedName))
		{
			string[] parts = formattedName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
			values["FirstName"] = parts[0];
			values["LastName"] = parts.Length > 1 ? parts[1] : string.Empty;
		}
	}

	private static List<List<string>> SplitCards(
		string content,
		ICollection<ContactImportIssue> issues)
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

	private static List<string> SplitValue(string value, char separator)
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

	private static string UnescapeValue(string value)
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
}
