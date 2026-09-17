using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Provides read-only queries over the customer, employee, and apprentice collections.
/// </summary>
internal static class ContactDataQuery
{
	/// <summary>Creates a read-only snapshot containing every stored contact type.</summary>
	/// <param name="data">The contact data to query.</param>
	/// <returns>A read-only snapshot of all stored contacts.</returns>
	internal static IReadOnlyList<Person> CreatePeopleSnapshot(ContactData data)
	{
		ArgumentNullException.ThrowIfNull(data);

		return data.Customers
			.Cast<Person>()
			.Concat(data.Employees)
			.Concat(data.Apprentices)
			.ToList()
			.AsReadOnly();
	}

	/// <summary>Finds a contact by its stable identifier.</summary>
	/// <param name="data">The contact data to query.</param>
	/// <param name="id">The identifier to find.</param>
	/// <returns>The matching contact, or <see langword="null"/> when it does not exist.</returns>
	internal static Person? FindById(ContactData data, Guid id)
	{
		return CreatePeopleSnapshot(data).FirstOrDefault(person => person.Id == id);
	}
}
