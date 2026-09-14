using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Finds possible duplicate import candidates using stable contact attributes.
/// </summary>
public sealed class ContactDuplicateDetector
{
	/// <summary>
	/// Compares import candidates with stored contacts and earlier candidates in the same file.
	/// </summary>
	/// <param name="candidates">The valid contacts parsed from the import file.</param>
	/// <param name="existingContacts">The contacts already stored by the application.</param>
	/// <returns>Warnings for matching non-empty email addresses or employee AHV numbers.</returns>
	public IReadOnlyList<ContactImportIssue> FindPotentialDuplicates(
		IEnumerable<ContactImportCandidate> candidates,
		IEnumerable<Person> existingContacts)
	{
		ArgumentNullException.ThrowIfNull(candidates);
		ArgumentNullException.ThrowIfNull(existingContacts);
		IReadOnlyList<Person> existingContactSnapshot = existingContacts.ToList().AsReadOnly();

		Dictionary<string, Person> existingByEmail = CreateExistingIndex(
			existingContactSnapshot,
			contact => NormalizeEmail(contact.EmailAddress));
		Dictionary<string, Person> existingByAhv = CreateExistingIndex(
			existingContactSnapshot.OfType<Employee>(),
			contact => NormalizeAhv(contact.AhvNumber));
		Dictionary<string, ContactImportCandidate> candidatesByEmail = new(StringComparer.OrdinalIgnoreCase);
		Dictionary<string, ContactImportCandidate> candidatesByAhv = new(StringComparer.Ordinal);
		List<ContactImportIssue> warnings = [];

		foreach (ContactImportCandidate candidate in candidates)
		{
			string email = NormalizeEmail(candidate.Contact.EmailAddress);
			if (email.Length > 0)
			{
				if (existingByEmail.TryGetValue(email, out Person? existingContact))
				{
					warnings.Add(CreateWarning(
						candidate.Source,
						$"Email address '{candidate.Contact.EmailAddress}' is already used by {Describe(existingContact)}."));
				}
				else if (candidatesByEmail.TryGetValue(email, out ContactImportCandidate? earlierCandidate))
				{
					warnings.Add(CreateWarning(
						candidate.Source,
						$"Email address '{candidate.Contact.EmailAddress}' also occurs in {earlierCandidate.Source}."));
				}
				else
				{
					candidatesByEmail.Add(email, candidate);
				}
			}

			if (candidate.Contact is not Employee employee)
			{
				continue;
			}

			string ahvNumber = NormalizeAhv(employee.AhvNumber);
			if (ahvNumber.Length == 0)
			{
				continue;
			}

			if (existingByAhv.TryGetValue(ahvNumber, out Person? existingEmployee))
			{
				warnings.Add(CreateWarning(
					candidate.Source,
					$"AHV number is already used by {Describe(existingEmployee)}."));
			}
			else if (candidatesByAhv.TryGetValue(ahvNumber, out ContactImportCandidate? earlierCandidate))
			{
				warnings.Add(CreateWarning(
					candidate.Source,
					$"AHV number also occurs in {earlierCandidate.Source}."));
			}
			else
			{
				candidatesByAhv.Add(ahvNumber, candidate);
			}
		}

		return warnings.AsReadOnly();
	}

	private static Dictionary<string, Person> CreateExistingIndex<TPerson>(
		IEnumerable<TPerson> contacts,
		Func<TPerson, string> keySelector)
		where TPerson : Person
	{
		Dictionary<string, Person> index = new(StringComparer.OrdinalIgnoreCase);
		foreach (TPerson contact in contacts)
		{
			string key = keySelector(contact);
			if (key.Length > 0)
			{
				index.TryAdd(key, contact);
			}
		}
		return index;
	}

	private static ContactImportIssue CreateWarning(string source, string message)
	{
		return new ContactImportIssue(
			source,
			"Possible duplicate: " + message,
			ContactImportIssueSeverity.Warning,
			ContactImportIssueKind.PotentialDuplicate);
	}

	private static string NormalizeEmail(string? emailAddress)
	{
		return emailAddress?.Trim() ?? string.Empty;
	}

	private static string NormalizeAhv(string? ahvNumber)
	{
		return string.IsNullOrWhiteSpace(ahvNumber)
			? string.Empty
			: new string(ahvNumber.Where(char.IsDigit).ToArray());
	}

	private static string Describe(Person contact)
	{
		string type = contact switch
		{
			Apprentice => "apprentice",
			Employee => "employee",
			Customer => "customer",
			_ => "contact"
		};
		string name = $"{contact.FirstName} {contact.LastName}".Trim();
		return string.IsNullOrWhiteSpace(name) ? $"an existing {type}" : $"{type} '{name}'";
	}
}
