using ContactManager.Logic;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.UI;

public partial class MainForm
{
	/// <summary>
	/// Loads the initial customer and employee list projections after the form has been created.
	/// </summary>
	/// <param name="sender">The form raising the load event.</param>
	/// <param name="e">The event data.</param>
	private void LoadInitialData(object? sender, EventArgs e)
	{
		try
		{
			IReadOnlyList<Person> people = personManager!.GetAll();
			RefreshDashboard(people);
			CustomersGrid.DataSource = people
				.OfType<Customer>()
				.Select(CreateCustomerListRow)
				.ToList();
			EmployeesGrid.DataSource = people
				.OfType<Employee>()
				.OrderBy(employee => employee.EmployeeNumber)
				.Select(CreateEmployeeListRow)
				.ToList();

			SetCustomerEditorMode(false, CustomersGrid.CurrentRow is not null);
			SetEmployeeEditorMode(false, EmployeesGrid.CurrentRow is not null);
			ShowCustomerNotes(false);
			ShowCustomerEditHistory(false);
			ShowEmployeeEditHistory(false);
		}
		catch (Exception exception)
		{
			CustomersGrid.DataSource = null;
			EmployeesGrid.DataSource = null;
			SetCustomerEditorMode(false, false);
			SetEmployeeEditorMode(false, false);
			ShowCustomerNotes(false);
			ShowCustomerEditHistory(false);
			ShowEmployeeEditHistory(false);
			MessageBox.Show(
				this,
				$"The contact lists could not be loaded.\n\n{exception.Message}",
				Text,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}
	}

	/// <summary>Refreshes dashboard metrics and date-based lists from the current contact snapshot.</summary>
	/// <param name="people">The contact snapshot used to calculate the dashboard.</param>
	private void RefreshDashboard(IReadOnlyList<Person> people)
	{
		DateOnly today = DateOnly.FromDateTime(DateTime.Today);
		DateOnly birthdayWindowEnd = today.AddDays(30);
		DateOnly departureWindowEnd = today.AddMonths(6);

		CustomerCount.Text = people.OfType<Customer>().Count().ToString();
		EmployeeCount.Text = people.OfType<Employee>().Count().ToString();
		ActiveContactCount.Text = people.Count(person => person.IsActive).ToString();

		UpcomingBirthdaysGrid.DataSource = people
			.Where(person => person.DateOfBirth != default)
			.Select(person => CreateUpcomingBirthdayRow(person, today))
			.Where(row => row.NextBirthday >= today && row.NextBirthday <= birthdayWindowEnd)
			.OrderBy(row => row.NextBirthday)
			.ThenBy(row => row.PersonName)
			.ToList();

		UpcomingDeparturesGrid.DataSource = people
			.OfType<Employee>()
			.Where(employee => employee.EmploymentEndDate != DateOnly.MaxValue
				&& employee.EmploymentEndDate >= today
				&& employee.EmploymentEndDate <= departureWindowEnd)
			.OrderBy(employee => employee.EmploymentEndDate)
			.ThenBy(employee => employee.EmployeeNumber)
			.Select(employee => new UpcomingDepartureRow
			{
				EmployeeNumber = employee.EmployeeNumber,
				Name = $"{employee.FirstName} {employee.LastName}".Trim(),
				Department = employee.Department,
				EndDate = employee.EmploymentEndDate
			})
			.ToList();

		UpcomingBirthdays.Text = UpcomingBirthdaysGrid.Rows.Count == 0
			? "Upcoming birthdays (none in the next 30 days)"
			: "Upcoming birthdays";
		UpcomingDepartures.Text = UpcomingDeparturesGrid.Rows.Count == 0
			? "Contracts ending within six months (none)"
			: "Contracts ending within six months";
	}

	/// <summary>Creates a birthday projection using the next occurrence on or after today.</summary>
	/// <param name="person">The person whose birthday should be projected.</param>
	/// <param name="today">The date used as the beginning of the dashboard window.</param>
	/// <returns>The birthday dashboard row.</returns>
	private static UpcomingBirthdayRow CreateUpcomingBirthdayRow(Person person, DateOnly today)
	{
		DateOnly nextBirthday = CreateBirthdayDate(person.DateOfBirth, today.Year);
		if (nextBirthday < today)
		{
			nextBirthday = CreateBirthdayDate(person.DateOfBirth, today.Year + 1);
		}

		return new UpcomingBirthdayRow
		{
			PersonName = $"{person.FirstName} {person.LastName}".Trim(),
			ContactType = person switch
			{
				Customer => "Customer",
				Apprentice => "Apprentice",
				Employee => "Employee",
				_ => "Person"
			},
			DateOfBirth = person.DateOfBirth,
			NextBirthday = nextBirthday
		};
	}

	/// <summary>Creates a birthday date for a year, handling February 29 in non-leap years.</summary>
	/// <param name="dateOfBirth">The original date of birth.</param>
	/// <param name="year">The year of the next birthday.</param>
	/// <returns>The birthday date in the requested year.</returns>
	private static DateOnly CreateBirthdayDate(DateOnly dateOfBirth, int year)
	{
		if (dateOfBirth.Month == 2 && dateOfBirth.Day == 29 && !DateTime.IsLeapYear(year))
		{
			return new DateOnly(year, 2, 28);
		}

		return new DateOnly(year, dateOfBirth.Month, dateOfBirth.Day);
	}

	/// <summary>Refreshes the dashboard when the user enters its tab.</summary>
	/// <param name="sender">The tab control raising the event.</param>
	/// <param name="e">The event data.</param>
	private void RefreshDashboardOnTabSelection(object? sender, EventArgs e)
	{
		if (MainTabs.SelectedTab != DashboardTab || personManager is null)
		{
			return;
		}

		try
		{
			RefreshDashboard(personManager.GetAll());
		}
		catch (Exception exception)
		{
			ShowErrorMessage("The dashboard could not be refreshed.\n\n" + exception.Message);
		}
	}

	/// <summary>Creates the customer presentation model used by the customer grid.</summary>
	/// <param name="customer">The customer to project.</param>
	/// <returns>A customer list row containing only display data.</returns>
	private static CustomerListRow CreateCustomerListRow(Customer customer)
	{
		return new CustomerListRow
		{
			Id = customer.Id,
			CreatedAt = customer.CreatedAt,
			Title = customer.Title,
			FirstName = customer.FirstName,
			LastName = customer.LastName,
			DateOfBirth = customer.DateOfBirth,
			Gender = customer.Gender,
			JobTitle = customer.JobTitle,
			BusinessNumber = customer.BusinessNumber,
			MobileNumber = customer.MobileNumber,
			EmailAddress = customer.EmailAddress,
			CustomerName = $"{customer.FirstName} {customer.LastName}".Trim(),
			Company = customer.Company,
			ContactHistoryCount = customer.ContactHistory?.Count ?? 0,
			Email = customer.EmailAddress,
			Phone =
				string.IsNullOrWhiteSpace(customer.BusinessNumber) ? customer.MobileNumber : customer.BusinessNumber,
			Status = customer.IsActive ? "Active" : "Passive"
		};
	}

	/// <summary>Creates the employee presentation model used by the employee grid.</summary>
	/// <param name="employee">The employee to project.</param>
	/// <returns>An employee list row containing only display data.</returns>
	private static EmployeeListRow CreateEmployeeListRow(Employee employee)
	{
		return new EmployeeListRow
		{
			Id = employee.Id,
			CreatedAt = employee.CreatedAt,
			Title = employee.Title,
			FirstName = employee.FirstName,
			LastName = employee.LastName,
			DateOfBirth = employee.DateOfBirth,
			Gender = employee.Gender,
			JobTitle = employee.JobTitle,
			BusinessNumber = employee.BusinessNumber,
			MobileNumber = employee.MobileNumber,
			EmailAddress = employee.EmailAddress,
			EmployeeNumber = employee.EmployeeNumber,
			Name = $"{employee.FirstName} {employee.LastName}".Trim(),
			Department = employee.Department,
			EmploymentEndDate = employee.EmploymentEndDate == DateOnly.MaxValue
				? string.Empty
				: employee.EmploymentEndDate.ToString("d"),
			EmployeeType = employee is Apprentice ? "Apprentice" : "Employee",
			OfficeLocation = employee.OfficeLocation,
			ManagementLevel = employee.ManagementLevel,
			Status = employee.IsActive ? "Active" : "Passive",
			ApprenticeshipDuration = employee is Apprentice apprentice ? apprentice.ApprenticeshipDuration : (ushort)0,
			CurrentApprenticeshipYear = employee is Apprentice apprenticeData
				? apprenticeData.CurrentApprenticeshipYear
				: (ushort)0
		};
	}
}
