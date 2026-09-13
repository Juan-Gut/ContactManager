using ContactManager.Logic;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.UI;

public partial class MainForm
{
	/// <summary>
	/// Reloads the customer projection and optionally selects a customer by stable identifier.
	/// </summary>
	/// <param name="selectedCustomerId">The customer to select after reloading, if any.</param>
	/// <param name="preserveCurrentView">Whether selection events during rebinding should preserve the current view.</param>
	private void ReloadCustomers(Guid? selectedCustomerId = null, bool preserveCurrentView = false)
	{
		bool previousSuppression = suppressCustomerSelectionReset;
		suppressCustomerSelectionReset |= preserveCurrentView;
		try
		{
			CustomersGrid.DataSource = personManager!.GetAll()
				.OfType<Customer>()
				.Select(CreateCustomerListRow)
				.ToList();

			if (selectedCustomerId is not Guid id)
			{
				return;
			}

			for (int rowIndex = 0; rowIndex < CustomersGrid.Rows.Count; rowIndex++)
			{
				if (CustomersGrid.Rows[rowIndex].DataBoundItem is CustomerListRow { Id: var rowId } && rowId == id)
				{
					CustomersGrid.ClearSelection();
					CustomersGrid.Rows[rowIndex].Selected = true;
					CustomersGrid.CurrentCell = CustomersGrid.Rows[rowIndex].Cells[0];
					break;
				}
			}
		}
		finally
		{
			suppressCustomerSelectionReset = previousSuppression;
		}
	}

	/// <summary>
	/// Reloads the employee projection and optionally selects an employee by stable identifier.
	/// </summary>
	/// <param name="selectedEmployeeId">The employee to select after reloading, if any.</param>
	private void ReloadEmployees(Guid? selectedEmployeeId = null)
	{
		EmployeesGrid.DataSource = personManager!.GetAll()
			.OfType<Employee>()
			.OrderBy(employee => employee.EmployeeNumber)
			.Select(CreateEmployeeListRow)
			.ToList();

		if (selectedEmployeeId is not Guid id)
		{
			return;
		}

		for (int rowIndex = 0; rowIndex < EmployeesGrid.Rows.Count; rowIndex++)
		{
			if (EmployeesGrid.Rows[rowIndex].DataBoundItem is EmployeeListRow { Id: var rowId } && rowId == id)
			{
				EmployeesGrid.ClearSelection();
				EmployeesGrid.Rows[rowIndex].Selected = true;
				EmployeesGrid.CurrentCell = EmployeesGrid.Rows[rowIndex].Cells[0];
				break;
			}
		}
	}

	/// <summary>Centers split views after the form has its final initial size.</summary>
	private void InitializeLayout(object? sender, EventArgs e)
	{
		CenterSplitView(CustomersSplitView);
		CenterSplitView(EmployeesSplitView);
		CenterCustomerNotesSplitView();
	}

	/// <summary>Returns both contact editors to view mode when the active tab changes.</summary>
	private void ResetEditModesOnTabSwitch(object? sender, EventArgs e)
	{
		if (customerEditMode)
		{
			CancelCustomerEditMode(sender, e);
		}

		if (employeeEditMode)
		{
			CancelEmployeeEditMode(sender, e);
		}
	}

	/// <summary>Centers a vertical split handle after the split view has its final runtime size.</summary>
	private static void CenterSplitView(SplitContainer splitView)
	{
		splitView.SplitterDistance = (splitView.ClientSize.Width - splitView.SplitterWidth) * 2 / 3;
	}

	/// <summary>Centers the customer note list and note viewer at an even width.</summary>
	private void CenterCustomerNotesSplitView()
	{
		// The notes view can be hidden while the form is initializing, so use its final client height when available.
		if (CustomerNotesSplitView.ClientSize.Height > CustomerNotesSplitView.SplitterWidth)
		{
			CustomerNotesSplitView.SplitterDistance =
				(CustomerNotesSplitView.ClientSize.Height - CustomerNotesSplitView.SplitterWidth) / 2;
		}
	}

	/// <summary>Draws a tab label centered horizontally and vertically.</summary>
	private void DrawMainTab(object? sender, DrawItemEventArgs e)
	{
		if (e.Index < 0 || e.Index >= MainTabs.TabPages.Count)
		{
			return;
		}

		TabPage tabPage = MainTabs.TabPages[e.Index];
		bool isSelected = e.Index == MainTabs.SelectedIndex;
		if (isSelected)
		{
			using SolidBrush selectedBackground = new(Color.FromArgb(250, 250, 250));
			e.Graphics.FillRectangle(selectedBackground, e.Bounds);
		}
		else
		{
			using SolidBrush unselectedBackground = new(Color.FromArgb(225, 225, 225));
			e.Graphics.FillRectangle(unselectedBackground, e.Bounds);
		}

		Color textColor = isSelected ? SystemColors.ControlText : SystemColors.GrayText;
		TextRenderer.DrawText(e.Graphics, tabPage.Text, MainTabs.Font, e.Bounds, textColor,
			TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
	}

	/// <summary>Retains the search input as a UI-only preview until customer data is connected.</summary>
	private void SearchCustomers(object? sender, EventArgs e)
	{
	}

	/// <summary>Retains the search input as a UI-only preview until employee data is connected.</summary>
	private void SearchEmployees(object? sender, EventArgs e)
	{
	}

	/// <summary>Updates customer action availability when a row is selected.</summary>
	private void SelectCustomer(object? sender, EventArgs e)
	{
		if (!suppressCustomerSelectionReset)
		{
			ResetViewsAfterContactSelection();
		}
		CustomerListRow? selectedRow = CustomersGrid.SelectedRows.Count == 1
			? CustomersGrid.SelectedRows[0].DataBoundItem as CustomerListRow
			: null;

		Customer? selectedCustomer = selectedRow is null
			? null
			: personManager?.GetById(selectedRow.Id) as Customer;

		if (selectedCustomer is null)
		{
			ClearCustomerDetails();
		}
		else
		{
			PopulateCustomerDetails(selectedCustomer);
		}

		SetCustomerEditorMode(customerEditMode, selectedCustomer is not null);
		int noteCount = selectedRow?.ContactHistoryCount ?? 0;
		ViewCustomerNotes.Text = $"Contact notes ({noteCount})";
	}

	/// <summary>
	/// Copies a customer's values into the customer detail controls.
	/// </summary>
	/// <param name="customer">The customer whose values should be displayed.</param>
	private void PopulateCustomerDetails(Customer customer)
	{
		CustomerTitleInput.SelectedItem = customer.Title;
		CustomerFirstNameInput.Text = customer.FirstName;
		CustomerLastNameInput.Text = customer.LastName;
		CustomerDateOfBirthInput.Value = customer.DateOfBirth.ToDateTime(TimeOnly.MinValue);
		CustomerGenderInput.SelectedItem = customer.Gender;
		CustomerJobTitleInput.Text = customer.JobTitle;
		CustomerBusinessPhoneInput.Text = customer.BusinessNumber;
		CustomerMobilePhoneInput.Text = customer.MobileNumber;
		CustomerEmailInput.Text = customer.EmailAddress;
		CustomerActiveInput.Checked = customer.IsActive;
		CustomerCompanyInput.Text = customer.Company;
	}

	/// <summary>Clears the customer detail controls when no customer is selected.</summary>
	private void ClearCustomerDetails()
	{
		CustomerTitleInput.SelectedIndex = -1;
		CustomerFirstNameInput.Clear();
		CustomerLastNameInput.Clear();
		CustomerDateOfBirthInput.Value = CustomerDateOfBirthInput.MinDate;
		CustomerGenderInput.SelectedIndex = -1;
		CustomerJobTitleInput.Clear();
		CustomerBusinessPhoneInput.Clear();
		CustomerMobilePhoneInput.Clear();
		CustomerEmailInput.Clear();
		CustomerActiveInput.Checked = false;
		CustomerCompanyInput.Clear();
	}

	/// <summary>Updates employee action availability when a row is selected.</summary>
	private void SelectEmployee(object? sender, EventArgs e)
	{
		ResetViewsAfterContactSelection();
		EmployeeListRow? selectedRow = EmployeesGrid.SelectedRows.Count == 1
			? EmployeesGrid.SelectedRows[0].DataBoundItem as EmployeeListRow
			: null;
		Employee? selectedEmployee = selectedRow is null
			? null
			: personManager?.GetById(selectedRow.Id) as Employee;

		if (selectedEmployee is null)
		{
			ClearEmployeeDetails();
		}
		else
		{
			PopulateEmployeeDetails(selectedEmployee);
		}

		SetEmployeeEditorMode(employeeEditMode, selectedEmployee is not null);
	}

	/// <summary>
	/// Returns all contact detail areas to their normal list/detail state after selection changes.
	/// </summary>
	private void ResetViewsAfterContactSelection()
	{
		customerEditMode = false;
		creatingCustomer = false;
		employeeEditMode = false;
		creatingEmployee = false;
		CancelNewCustomerNote(this, EventArgs.Empty);
		ShowCustomerNotes(false);
		ShowCustomerEditHistory(false);
		ShowEmployeeEditHistory(false);
	}

	/// <summary>Gets the full customer represented by the selected customer-grid row.</summary>
	/// <returns>The selected customer, or <see langword="null"/> when no valid row is selected.</returns>
	private Customer? GetSelectedCustomer()
	{
		return CustomersGrid.SelectedRows.Count == 1
			&& CustomersGrid.SelectedRows[0].DataBoundItem is CustomerListRow selectedRow
			? personManager?.GetById(selectedRow.Id) as Customer
			: null;
	}

	/// <summary>Gets the full employee represented by the selected employee-grid row.</summary>
	/// <returns>The selected employee, or <see langword="null"/> when no valid row is selected.</returns>
	private Employee? GetSelectedEmployee()
	{
		return EmployeesGrid.SelectedRows.Count == 1
			&& EmployeesGrid.SelectedRows[0].DataBoundItem is EmployeeListRow selectedRow
			? personManager?.GetById(selectedRow.Id) as Employee
			: null;
	}

	/// <summary>
	/// Copies an employee's values into the employee detail controls.
	/// </summary>
	/// <param name="employee">The employee whose values should be displayed.</param>
	private void PopulateEmployeeDetails(Employee employee)
	{
		EmployeeNumberInput.Text = employee.EmployeeNumber.ToString();
		EmployeeTitleInput.SelectedItem = employee.Title;
		EmployeeFirstNameInput.Text = employee.FirstName;
		EmployeeLastNameInput.Text = employee.LastName;
		SetDatePickerValue(EmployeeDateOfBirthInput, employee.DateOfBirth);
		EmployeeGenderInput.SelectedItem = employee.Gender;
		EmployeeJobTitleInput.Text = employee.JobTitle;
		EmployeeBusinessPhoneInput.Text = employee.BusinessNumber;
		EmployeeMobilePhoneInput.Text = employee.MobileNumber;
		EmployeeEmailInput.Text = employee.EmailAddress;
		EmployeeActiveInput.Checked = employee.IsActive;
		EmployeeDepartmentInput.Text = employee.Department;
		EmployeeAhvNumberInput.Text = employee.AhvNumber;
		EmployeeNationalityInput.Text = employee.Nationality;
		EmployeeCityInput.Text = employee.City;
		EmployeeAddressInput.Text = employee.Address;
		EmployeePostalCodeInput.Text = employee.Plz;
		SetDatePickerValue(EmployeeStartDateInput, employee.EmploymentStartDate);
		EmployeeIndefiniteInput.Checked = employee.EmploymentEndDate == DateOnly.MaxValue;
		if (!EmployeeIndefiniteInput.Checked)
		{
			SetDatePickerValue(EmployeeEndDateInput, employee.EmploymentEndDate);
		}
		SetNumericValue(EmployeeEmploymentPercentageInput, employee.EmploymentPercentage);
		EmployeeOfficeLocationInput.SelectedItem = employee.OfficeLocation;
		EmployeeManagementLevelInput.SelectedItem = employee.ManagementLevel;
		EmployeeTypeApprenticeOption.Checked = employee is Apprentice;

		if (employee is Apprentice apprentice)
		{
			SetNumericValue(ApprenticeshipDurationInput, apprentice.ApprenticeshipDuration);
			SetNumericValue(CurrentApprenticeshipYearInput, apprentice.CurrentApprenticeshipYear);
		}

		SetApprenticeFieldsVisible(employee is Apprentice);
	}

	/// <summary>Clears the employee detail controls when no employee is selected.</summary>
	private void ClearEmployeeDetails()
	{
		EmployeeNumberInput.Clear();
		EmployeeTitleInput.SelectedIndex = -1;
		EmployeeFirstNameInput.Clear();
		EmployeeLastNameInput.Clear();
		EmployeeDateOfBirthInput.Value = EmployeeDateOfBirthInput.MinDate;
		EmployeeGenderInput.SelectedIndex = -1;
		EmployeeJobTitleInput.Clear();
		EmployeeBusinessPhoneInput.Clear();
		EmployeeMobilePhoneInput.Clear();
		EmployeeEmailInput.Clear();
		EmployeeActiveInput.Checked = false;
		EmployeeDepartmentInput.Clear();
		EmployeeAhvNumberInput.Clear();
		EmployeeNationalityInput.Clear();
		EmployeeCityInput.Clear();
		EmployeeAddressInput.Clear();
		EmployeePostalCodeInput.Clear();
		EmployeeStartDateInput.Value = EmployeeStartDateInput.MinDate;
		EmployeeIndefiniteInput.Checked = true;
		EmployeeEmploymentPercentageInput.Value = EmployeeEmploymentPercentageInput.Minimum;
		EmployeeOfficeLocationInput.SelectedIndex = -1;
		EmployeeManagementLevelInput.SelectedIndex = -1;
		EmployeeTypeEmployeeOption.Checked = true;
		ApprenticeshipDurationInput.Value = ApprenticeshipDurationInput.Minimum;
		CurrentApprenticeshipYearInput.Value = CurrentApprenticeshipYearInput.Minimum;
		SetApprenticeFieldsVisible(false);
	}

	/// <summary>Sets a date picker to a model date while respecting its supported range.</summary>
	/// <param name="datePicker">The date picker to update.</param>
	/// <param name="date">The model date to display.</param>
	private static void SetDatePickerValue(DateTimePicker datePicker, DateOnly date)
	{
		DateTime value = date.ToDateTime(TimeOnly.MinValue);
		datePicker.Value = value < datePicker.MinDate
			? datePicker.MinDate
			: value > datePicker.MaxDate
				? datePicker.MaxDate
				: value;
	}

	/// <summary>Displays or clears the employee end-date picker while retaining a valid internal date value.</summary>
	/// <param name="showDate">Whether the picker should display its selected date.</param>
	private void SetEmployeeEndDateDisplay(bool showDate)
	{
		if (showDate)
		{
			EmployeeEndDateInput.CustomFormat = "dd.MM.yyyy";
			if (EmployeeEndDateInput.Value == EmployeeEndDateInput.MinDate)
			{
				EmployeeEndDateInput.Value = DateTime.Today;
			}

			return;
		}

		EmployeeEndDateInput.CustomFormat = " ";
		EmployeeEndDateInput.Value = EmployeeEndDateInput.MinDate;
	}

	/// <summary>Sets a numeric control to a value constrained to its configured range.</summary>
	/// <param name="numericInput">The numeric control to update.</param>
	/// <param name="value">The value to display.</param>
	private static void SetNumericValue(NumericUpDown numericInput, decimal value)
	{
		numericInput.Value = Math.Clamp(value, numericInput.Minimum, numericInput.Maximum);
	}
}
