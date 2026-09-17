using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Creates, queries, and removes metadata-only entries in the contact mutation history.
/// </summary>
internal sealed class ContactMutationHistoryService
{
	/// <summary>Gets one contact's mutation history ordered from newest to oldest.</summary>
	/// <param name="data">The contact data containing the history.</param>
	/// <param name="contactId">The stable contact identifier.</param>
	/// <returns>A read-only snapshot of matching history entries.</returns>
	internal IReadOnlyList<MutationLogEntry> GetForContact(ContactData data, Guid contactId)
	{
		return data.MutationHistory
			.Where(entry => entry.ContactId == contactId)
			.OrderByDescending(entry => entry.ChangedAt)
			.ToList()
			.AsReadOnly();
	}

	/// <summary>Adds a metadata-only mutation entry.</summary>
	/// <param name="data">The contact data receiving the entry.</param>
	/// <param name="person">The contact affected by the mutation.</param>
	/// <param name="action">The completed action without field values.</param>
	/// <returns>The entry added to the mutation history.</returns>
	internal MutationLogEntry Add(ContactData data, Person person, string action)
	{
		MutationLogEntry mutation = new()
		{
			ContactId = person.Id,
			Action = action
		};

		data.MutationHistory.Add(mutation);
		return mutation;
	}

	/// <summary>Removes the newest mutation entry for one contact.</summary>
	/// <param name="data">The contact data containing the entry.</param>
	/// <param name="contactId">The contact whose newest entry should be removed.</param>
	internal void RemoveLatest(ContactData data, Guid contactId)
	{
		MutationLogEntry? mutation = data.MutationHistory
			.LastOrDefault(entry => entry.ContactId == contactId);
		if (mutation is not null)
		{
			data.MutationHistory.Remove(mutation);
		}
	}

	/// <summary>Removes every mutation entry associated with one contact.</summary>
	/// <param name="data">The contact data containing the entries.</param>
	/// <param name="contactId">The contact whose entries should be removed.</param>
	internal void RemoveAll(ContactData data, Guid contactId)
	{
		data.MutationHistory.RemoveAll(entry => entry.ContactId == contactId);
	}

	/// <summary>Describes an update by naming changed fields without recording their values.</summary>
	/// <param name="previousPerson">The stored contact before the update.</param>
	/// <param name="updatedPerson">The replacement contact after the update.</param>
	/// <returns>A mutation-history action description.</returns>
	internal string DescribeUpdate(Person previousPerson, Person updatedPerson)
	{
		List<string> changedFields = [];
		if (previousPerson.Title != updatedPerson.Title) { changedFields.Add("Title"); }
		if (previousPerson.FirstName != updatedPerson.FirstName) { changedFields.Add("First name"); }
		if (previousPerson.LastName != updatedPerson.LastName) { changedFields.Add("Last name"); }
		if (previousPerson.DateOfBirth != updatedPerson.DateOfBirth) { changedFields.Add("Date of birth"); }
		if (previousPerson.Gender != updatedPerson.Gender) { changedFields.Add("Gender"); }
		if (previousPerson.JobTitle != updatedPerson.JobTitle) { changedFields.Add("Job title"); }
		if (previousPerson.BusinessNumber != updatedPerson.BusinessNumber) { changedFields.Add("Business phone"); }
		if (previousPerson.MobileNumber != updatedPerson.MobileNumber) { changedFields.Add("Mobile phone"); }
		if (previousPerson.EmailAddress != updatedPerson.EmailAddress) { changedFields.Add("Email address"); }
		if (previousPerson.IsActive != updatedPerson.IsActive) { changedFields.Add("Active status"); }

		if (previousPerson is Customer previousCustomer && updatedPerson is Customer updatedCustomer
			&& previousCustomer.Company != updatedCustomer.Company)
		{
			changedFields.Add("Company");
		}

		if (previousPerson is Employee previousEmployee && updatedPerson is Employee updatedEmployee)
		{
			AddChangedEmployeeFields(previousEmployee, updatedEmployee, changedFields);
		}

		if (previousPerson is Apprentice previousApprentice && updatedPerson is Apprentice updatedApprentice)
		{
			AddChangedApprenticeFields(previousApprentice, updatedApprentice, changedFields);
		}

		return changedFields.Count == 0
			? "Updated contact information (no field values changed)"
			: $"Updated contact information ({string.Join(", ", changedFields)})";
	}

	private static void AddChangedEmployeeFields(
		Employee previousEmployee,
		Employee updatedEmployee,
		ICollection<string> changedFields)
	{
		if (previousEmployee.Department != updatedEmployee.Department) { changedFields.Add("Department"); }
		if (previousEmployee.AhvNumber != updatedEmployee.AhvNumber) { changedFields.Add("AHV number"); }
		if (previousEmployee.Nationality != updatedEmployee.Nationality) { changedFields.Add("Nationality"); }
		if (previousEmployee.City != updatedEmployee.City) { changedFields.Add("City"); }
		if (previousEmployee.Address != updatedEmployee.Address) { changedFields.Add("Address"); }
		if (previousEmployee.Plz != updatedEmployee.Plz) { changedFields.Add("PLZ"); }
		if (previousEmployee.EmploymentStartDate != updatedEmployee.EmploymentStartDate)
		{
			changedFields.Add("Employment start date");
		}
		if (previousEmployee.EmploymentEndDate != updatedEmployee.EmploymentEndDate)
		{
			changedFields.Add("Employment end date");
		}
		if (previousEmployee.EmploymentPercentage != updatedEmployee.EmploymentPercentage)
		{
			changedFields.Add("Employment percentage");
		}
		if (previousEmployee.OfficeLocation != updatedEmployee.OfficeLocation)
		{
			changedFields.Add("Office location");
		}
		if (previousEmployee.ManagementLevel != updatedEmployee.ManagementLevel)
		{
			changedFields.Add("Management level");
		}
	}

	private static void AddChangedApprenticeFields(
		Apprentice previousApprentice,
		Apprentice updatedApprentice,
		ICollection<string> changedFields)
	{
		if (previousApprentice.ApprenticeshipDuration != updatedApprentice.ApprenticeshipDuration)
		{
			changedFields.Add("Apprenticeship duration");
		}
		if (previousApprentice.CurrentApprenticeshipYear != updatedApprentice.CurrentApprenticeshipYear)
		{
			changedFields.Add("Current apprenticeship year");
		}
	}
}
