using ContactManager.Logic;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.UI;

public partial class MainForm
{
	/// <summary>Clears the customer editor and enters customer creation mode.</summary>
	/// <param name="sender">The create-customer button.</param>
	/// <param name="e">The event data.</param>
	private void CreateNewCustomer(object? sender, EventArgs e)
	{
		CustomerTitleInput.SelectedIndex = 0;
		CustomerFirstNameInput.Clear();
		CustomerLastNameInput.Clear();
		CustomerDateOfBirthInput.Value = new DateTime(2000, 1, 1);
		CustomerGenderInput.SelectedIndex = 0;
		CustomerJobTitleInput.Clear();
		CustomerBusinessPhoneInput.Clear();
		CustomerMobilePhoneInput.Clear();
		CustomerEmailInput.Clear();
		CustomerActiveInput.Checked = true;
		CustomerCompanyInput.Clear();
		creatingCustomer = true;
		SetCustomerEditorMode(true, false);
	}

	/// <summary>Enters customer edit mode for the selected preview row.</summary>
	private void EditCustomerDetails(object? sender, EventArgs e)
	{
		if (GetSelectedCustomer() is null)
		{
			return;
		}

		SetCustomerEditorMode(true, true);
	}

	/// <summary>Confirms and deletes the selected customer.</summary>
	private void DeleteSelectedCustomer(object? sender, EventArgs e)
	{
		if (CustomersGrid.SelectedRows.Count != 1
			|| CustomersGrid.SelectedRows[0].DataBoundItem is not CustomerListRow selectedRow
			|| personManager?.GetById(selectedRow.Id) is not Customer customer)
		{
			return;
		}

		string customerName = $"{customer.FirstName} {customer.LastName}".Trim();
		DialogResult confirmation = MessageBox.Show(
			this,
			$"Are you sure you want to delete {customerName}?",
			"Delete customer",
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Warning,
			MessageBoxDefaultButton.Button2);
		if (confirmation != DialogResult.Yes)
		{
			return;
		}

		try
		{
			if (!personManager!.Delete(customer.Id))
			{
				ShowErrorMessage("The customer could not be found.");
				return;
			}

			ReloadCustomers();
			RefreshDashboard(personManager.GetAll());
		}
		catch (Exception exception)
		{
			ShowErrorMessage("The customer could not be deleted.\n\n" + exception.Message);
		}
	}

	/// <summary>Saves a new or edited customer and returns to customer list mode.</summary>
	/// <param name="sender">The save-customer button.</param>
	/// <param name="e">The event data.</param>
	private void SaveCustomerDetails(object? sender, EventArgs e)
	{
		try
		{
			Customer? existingCustomer = creatingCustomer ? null : GetSelectedCustomer();
			if (!creatingCustomer && existingCustomer is null)
			{
				return;
			}

			Customer customer = new()
			{
				Id = existingCustomer?.Id ?? Guid.NewGuid(),
				CreatedAt = existingCustomer?.CreatedAt ?? DateOnly.FromDateTime(DateTime.UtcNow),
				Title = CustomerTitleInput.SelectedItem is Title selectedTitle
					? selectedTitle
					: ParseEnum(CustomerTitleInput.Text, Title.Unknown),
				FirstName = CustomerFirstNameInput.Text.Trim(),
				LastName = CustomerLastNameInput.Text.Trim(),
				DateOfBirth = DateOnly.FromDateTime(CustomerDateOfBirthInput.Value),
				Gender = CustomerGenderInput.SelectedItem is Gender selectedGender
					? selectedGender
					: ParseEnum(CustomerGenderInput.Text, Gender.Unknown),
				JobTitle = CustomerJobTitleInput.Text.Trim(),
				BusinessNumber = CustomerBusinessPhoneInput.Text.Trim(),
				MobileNumber = CustomerMobilePhoneInput.Text.Trim(),
				EmailAddress = CustomerEmailInput.Text.Trim(),
				IsActive = CustomerActiveInput.Checked,
				Company = CustomerCompanyInput.Text.Trim(),
				ContactHistory = existingCustomer?.ContactHistory ?? []
			};

			if (creatingCustomer)
			{
				personManager!.Add(customer);
			}
			else if (!personManager!.Update(customer))
			{
				ShowErrorMessage("The customer could not be found.");
				return;
			}

			ReloadCustomers(customer.Id);
			RefreshDashboard(personManager.GetAll());
			creatingCustomer = false;
			SetCustomerEditorMode(false, true);
		}
		catch (ArgumentException exception)
		{
			ShowErrorMessage("The customer could not be saved. Please correct the following:\n\n" + exception.Message);
		}
		catch (Exception exception)
		{
			ShowErrorMessage("The customer could not be saved.\n\n" + exception.Message);
		}
	}

	/// <summary>Cancels customer creation or editing without changing data.</summary>
	private void CancelCustomerEditMode(object? sender, EventArgs e)
	{
		creatingCustomer = false;
		Customer? selectedCustomer = GetSelectedCustomer();
		if (selectedCustomer is null)
		{
			ClearCustomerDetails();
		}
		else
		{
			PopulateCustomerDetails(selectedCustomer);
		}

		SetCustomerEditorMode(false, selectedCustomer is not null);
	}

	/// <summary>Converts a displayed enum value to an enum member with a safe fallback.</summary>
	/// <typeparam name="TEnum">The enum type to parse.</typeparam>
	/// <param name="value">The displayed value to parse.</param>
	/// <param name="fallback">The value to use when parsing fails.</param>
	/// <returns>The parsed enum value or the fallback.</returns>
	private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback)
		where TEnum : struct, Enum
	{
		return Enum.TryParse(value, true, out TEnum parsedValue) ? parsedValue : fallback;
	}

	/// <summary>Populates a combo box with enum values while omitting an Unknown member.</summary>
	/// <typeparam name="TEnum">The enum type displayed by the combo box.</typeparam>
	/// <param name="comboBox">The combo box to populate.</param>
	/// <param name="unknownValue">The optional enum member to omit.</param>
	private static void PopulateEnumComboBox<TEnum>(ComboBox comboBox, TEnum? unknownValue = null)
		where TEnum : struct, Enum
	{
		comboBox.Items.AddRange(Enum.GetValues<TEnum>()
			.Where(value =>
				!unknownValue.HasValue || !EqualityComparer<TEnum>.Default.Equals(value, unknownValue.Value))
			.Cast<object>()
			.ToArray());
		comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
		if (comboBox.Items.Count > 0)
		{
			comboBox.SelectedIndex = 0;
		}
	}

	/// <summary>Enters employee creation mode without constructing a model.</summary>
	private void CreateNewEmployee(object? sender, EventArgs e)
	{
		try
		{
			EmployeeNumberInput.Text = personManager?.GetNextEmployeeNumber().ToString() ?? string.Empty;
		}
		catch (InvalidOperationException exception)
		{
			ShowErrorMessage("A new employee cannot be created.\n\n" + exception.Message);
			return;
		}

		EmployeeTitleInput.SelectedIndex = 0;
		EmployeeFirstNameInput.Clear();
		EmployeeLastNameInput.Clear();
		EmployeeDateOfBirthInput.Value = new DateTime(2000, 1, 1);
		EmployeeGenderInput.SelectedIndex = 0;
		EmployeeJobTitleInput.Clear();
		EmployeeBusinessPhoneInput.Clear();
		EmployeeMobilePhoneInput.Clear();
		EmployeeEmailInput.Clear();
		EmployeeActiveInput.Checked = true;
		EmployeeDepartmentInput.Clear();
		EmployeeAhvNumberInput.Clear();
		EmployeeNationalityInput.Clear();
		EmployeeCityInput.Clear();
		EmployeeAddressInput.Clear();
		EmployeePostalCodeInput.Clear();
		EmployeeStartDateInput.Value = DateTime.Today;
		EmployeeIndefiniteInput.Checked = true;
		EmployeeEmploymentPercentageInput.Value = 100;
		EmployeeOfficeLocationInput.SelectedIndex = 0;
		EmployeeManagementLevelInput.SelectedIndex = 0;
		EmployeeTypeEmployeeOption.Checked = true;
		ApprenticeshipDurationInput.Value = ApprenticeshipDurationInput.Minimum;
		CurrentApprenticeshipYearInput.Value = CurrentApprenticeshipYearInput.Minimum;
		creatingEmployee = true;
		SetEmployeeEditorMode(true, false);
	}

	/// <summary>Enters employee edit mode for the selected preview row.</summary>
	private void EditEmployeeDetails(object? sender, EventArgs e)
	{
		if (GetSelectedEmployee() is null)
		{
			return;
		}

		SetEmployeeEditorMode(true, true);
	}

	/// <summary>Confirms and deletes the selected employee.</summary>
	private void DeleteSelectedEmployee(object? sender, EventArgs e)
	{
		if (EmployeesGrid.SelectedRows.Count != 1
			|| EmployeesGrid.SelectedRows[0].DataBoundItem is not EmployeeListRow selectedRow
			|| personManager?.GetById(selectedRow.Id) is not Employee employee)
		{
			return;
		}

		string employeeName = $"{employee.FirstName} {employee.LastName}".Trim();
		DialogResult confirmation = MessageBox.Show(
			this,
			$"Are you sure you want to delete {employeeName}?",
			"Delete employee",
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Warning,
			MessageBoxDefaultButton.Button2);
		if (confirmation != DialogResult.Yes)
		{
			return;
		}

		try
		{
			if (!personManager!.Delete(employee.Id))
			{
				ShowErrorMessage("The employee could not be found.");
				return;
			}

			ReloadEmployees();
			RefreshDashboard(personManager.GetAll());
		}
		catch (Exception exception)
		{
			ShowErrorMessage("The employee could not be deleted.\n\n" + exception.Message);
		}
	}

	/// <summary>Saves a newly created employee or apprentice and returns to list mode.</summary>
	private void SaveEmployeeDetails(object? sender, EventArgs e)
	{
		try
		{
			Employee? existingEmployee = creatingEmployee ? null : GetSelectedEmployee();
			if (!creatingEmployee && existingEmployee is null)
			{
				return;
			}

			Employee employee = CreateEmployeeFromInputs(existingEmployee);
			if (creatingEmployee)
			{
				personManager!.Add(employee);
			}
			else if (!personManager!.Update(employee))
			{
				ShowErrorMessage("The employee could not be found.");
				return;
			}

			ReloadEmployees(employee.Id);
			RefreshDashboard(personManager.GetAll());
			EmployeeNumberInput.Text = employee.EmployeeNumber.ToString();
			creatingEmployee = false;
			SetEmployeeEditorMode(false, true);
		}
		catch (ArgumentException exception)
		{
			ShowErrorMessage("The employee could not be saved. Please correct the following:\n\n" + exception.Message);
		}
		catch (Exception exception)
		{
			ShowErrorMessage("The employee could not be saved.\n\n" + exception.Message);
		}
	}

	/// <summary>Cancels employee creation or editing without changing data.</summary>
	private void CancelEmployeeEditMode(object? sender, EventArgs e)
	{
		creatingEmployee = false;
		Employee? selectedEmployee = GetSelectedEmployee();
		if (selectedEmployee is null)
		{
			ClearEmployeeDetails();
		}
		else
		{
			PopulateEmployeeDetails(selectedEmployee);
		}

		SetEmployeeEditorMode(false, selectedEmployee is not null);
	}

	/// <summary>Creates an employee or apprentice from the current employee form values.</summary>
	/// <param name="existingEmployee">The existing employee being edited, if applicable.</param>
	/// <returns>The employee model represented by the form.</returns>
	private Employee CreateEmployeeFromInputs(Employee? existingEmployee = null)
	{
		Guid id = existingEmployee?.Id ?? Guid.NewGuid();
		DateOnly createdAt = existingEmployee?.CreatedAt ?? DateOnly.FromDateTime(DateTime.UtcNow);
		int employeeNumber = existingEmployee?.EmployeeNumber ?? 0;
		Employee employee = EmployeeTypeApprenticeOption.Checked
			? new Apprentice { Id = id, CreatedAt = createdAt, EmployeeNumber = employeeNumber }
			: new Employee { Id = id, CreatedAt = createdAt, EmployeeNumber = employeeNumber };

		employee.Title = EmployeeTitleInput.SelectedItem is Title selectedTitle
			? selectedTitle
			: ParseEnum(EmployeeTitleInput.Text, Title.Unknown);
		employee.FirstName = EmployeeFirstNameInput.Text.Trim();
		employee.LastName = EmployeeLastNameInput.Text.Trim();
		employee.DateOfBirth = DateOnly.FromDateTime(EmployeeDateOfBirthInput.Value);
		employee.Gender = EmployeeGenderInput.SelectedItem is Gender selectedGender
			? selectedGender
			: ParseEnum(EmployeeGenderInput.Text, Gender.Unknown);
		employee.JobTitle = EmployeeJobTitleInput.Text.Trim();
		employee.BusinessNumber = EmployeeBusinessPhoneInput.Text.Trim();
		employee.MobileNumber = EmployeeMobilePhoneInput.Text.Trim();
		employee.EmailAddress = EmployeeEmailInput.Text.Trim();
		employee.IsActive = EmployeeActiveInput.Checked;
		employee.Department = EmployeeDepartmentInput.Text.Trim();
		employee.AhvNumber = EmployeeAhvNumberInput.Text.Trim();
		employee.Nationality = EmployeeNationalityInput.Text.Trim();
		employee.City = EmployeeCityInput.Text.Trim();
		employee.Address = EmployeeAddressInput.Text.Trim();
		employee.Plz = EmployeePostalCodeInput.Text.Trim();
		employee.EmploymentStartDate = DateOnly.FromDateTime(EmployeeStartDateInput.Value);
		employee.EmploymentEndDate = EmployeeIndefiniteInput.Checked
			? DateOnly.MaxValue
			: DateOnly.FromDateTime(EmployeeEndDateInput.Value);
		employee.EmploymentPercentage = (ushort)EmployeeEmploymentPercentageInput.Value;
		employee.OfficeLocation = EmployeeOfficeLocationInput.SelectedItem is OfficeLocation selectedOfficeLocation
			? selectedOfficeLocation
			: ParseEnum(EmployeeOfficeLocationInput.Text, OfficeLocation.Unknown);
		employee.ManagementLevel = EmployeeManagementLevelInput.SelectedItem is ManagementLevel selectedManagementLevel
			? selectedManagementLevel
			: ParseEnum(EmployeeManagementLevelInput.Text, ManagementLevel.None);

		if (employee is Apprentice apprentice)
		{
			apprentice.ApprenticeshipDuration = (ushort)ApprenticeshipDurationInput.Value;
			apprentice.CurrentApprenticeshipYear = (ushort)CurrentApprenticeshipYearInput.Value;
		}

		return employee;
	}
}
