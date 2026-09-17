using System.Globalization;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Matches free-text queries against the user-visible fields of stored contacts.
/// </summary>
internal sealed class ContactSearchService
{
	/// <summary>Searches the supplied contact snapshot without changing its contents.</summary>
	/// <param name="contacts">The contacts to search.</param>
	/// <param name="searchText">The case-insensitive text to find.</param>
	/// <returns>A read-only snapshot of matching contacts.</returns>
	internal IReadOnlyList<Person> Search(IEnumerable<Person> contacts, string? searchText)
	{
		ArgumentNullException.ThrowIfNull(contacts);
		IReadOnlyList<Person> contactSnapshot = contacts.ToList().AsReadOnly();
		if (string.IsNullOrWhiteSpace(searchText))
		{
			return contactSnapshot;
		}

		string normalizedSearchText = searchText.Trim();
		return contactSnapshot
			.Where(person => GetSearchValues(person)
				.Any(value => Contains(value, normalizedSearchText)))
			.ToList()
			.AsReadOnly();
	}

	private static bool Contains(string? value, string searchText)
	{
		return value?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true;
	}

	private static IEnumerable<string?> GetSearchValues(Person person)
	{
		yield return person.LastName;
		yield return person.FirstName;
		yield return $"{person.FirstName} {person.LastName}";
		yield return $"{person.LastName} {person.FirstName}";
		yield return person.DateOfBirth.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
		yield return person.DateOfBirth.ToString("d.M.yyyy", CultureInfo.InvariantCulture);
		yield return person.JobTitle;
		yield return person.EmailAddress;
		yield return person.BusinessNumber;
		yield return person.MobileNumber;
		yield return person.IsActive ? "Active" : "Passive";

		if (person is Customer customer)
		{
			yield return customer.Company;
		}

		if (person is Employee employee)
		{
			yield return employee.EmployeeNumber.ToString(CultureInfo.InvariantCulture);
			yield return employee.Department;
			yield return employee.AhvNumber;
			yield return employee.Address;
			yield return employee.City;
			yield return employee.Plz;
		}
	}
}
