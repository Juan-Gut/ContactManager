using ContactManager.Logic;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.UI;

/// <summary>
/// Provides the main Contact Manager user interface.
/// </summary>
public partial class MainForm : Form
{
	/// <summary>Provides access to contact-management operations at runtime.</summary>
	private readonly PersonManager? personManager;

	/// <summary>Indicates whether the customer editor is currently in edit mode.</summary>
	private bool customerEditMode;

	/// <summary>Indicates whether a new customer is being created.</summary>
	private bool creatingCustomer;

	/// <summary>Indicates whether the employee editor is currently in edit mode.</summary>
	private bool employeeEditMode;

	/// <summary>Indicates whether a new employee is being created.</summary>
	private bool creatingEmployee;

	/// <summary>
	/// Initializes a new instance of the form for the visual designer.
	/// </summary>
	public MainForm()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Initializes a new instance of the form for runtime use.
	/// </summary>
	/// <param name="personManager">The manager used to read contact data.</param>
	public MainForm(PersonManager personManager)
		: this()
	{
		this.personManager = personManager ?? throw new ArgumentNullException(nameof(personManager));
		PopulateEnumComboBox<Title>(CustomerTitleInput, Title.Unknown);
		PopulateEnumComboBox<Gender>(CustomerGenderInput, Gender.Unknown);
		PopulateEnumComboBox<Title>(EmployeeTitleInput, Title.Unknown);
		PopulateEnumComboBox<Gender>(EmployeeGenderInput, Gender.Unknown);
		PopulateEnumComboBox<OfficeLocation>(EmployeeOfficeLocationInput, OfficeLocation.Unknown);
		PopulateEnumComboBox<ManagementLevel>(EmployeeManagementLevelInput);
		SetCustomerEditorMode(false, false);
		SetEmployeeEditorMode(false, false);
		Load += LoadInitialData;
		Shown += InitializeLayout;
		MainTabs.SelectedIndexChanged += ResetEditModesOnTabSwitch;
	}

	/// <summary>Represents the identifying customer data shown in the customer list.</summary>
	private sealed class CustomerListRow
	{
		/// <summary>Gets the stable customer identifier.</summary>
		public Guid Id { get; init; }

		/// <summary>Gets the creation date.</summary>
		public DateOnly CreatedAt { get; init; }

		/// <summary>Gets the customer's title.</summary>
		public Title Title { get; init; }

		/// <summary>Gets the customer's first name.</summary>
		public string FirstName { get; init; } = string.Empty;

		/// <summary>Gets the customer's last name.</summary>
		public string LastName { get; init; } = string.Empty;

		/// <summary>Gets the customer's date of birth.</summary>
		public DateOnly DateOfBirth { get; init; }

		/// <summary>Gets the customer's gender.</summary>
		public Gender Gender { get; init; }

		/// <summary>Gets the customer's job title.</summary>
		public string JobTitle { get; init; } = string.Empty;

		/// <summary>Gets the customer's business phone number.</summary>
		public string BusinessNumber { get; init; } = string.Empty;

		/// <summary>Gets the customer's mobile phone number.</summary>
		public string MobileNumber { get; init; } = string.Empty;

		/// <summary>Gets the customer's email address.</summary>
		public string EmailAddress { get; init; } = string.Empty;

		/// <summary>Gets the customer's display name.</summary>
		public string CustomerName { get; init; } = string.Empty;

		/// <summary>Gets the customer's company.</summary>
		public string Company { get; init; } = string.Empty;

		/// <summary>Gets the customer's email address.</summary>
		public string Email { get; init; } = string.Empty;

		/// <summary>Gets the customer's preferred displayed phone number.</summary>
		public string Phone { get; init; } = string.Empty;

		/// <summary>Gets the customer's active-state display text.</summary>
		public string Status { get; init; } = string.Empty;

		/// <summary>Gets the number of customer contact-history entries.</summary>
		public int ContactHistoryCount { get; init; }
	}

	/// <summary>Represents the identifying employee data shown in the employee list.</summary>
	private sealed class EmployeeListRow
	{
		/// <summary>Gets the stable employee identifier.</summary>
		public Guid Id { get; init; }

		/// <summary>Gets the creation date.</summary>
		public DateOnly CreatedAt { get; init; }

		/// <summary>Gets the employee's title.</summary>
		public Title Title { get; init; }

		/// <summary>Gets the employee's first name.</summary>
		public string FirstName { get; init; } = string.Empty;

		/// <summary>Gets the employee's last name.</summary>
		public string LastName { get; init; } = string.Empty;

		/// <summary>Gets the employee's date of birth.</summary>
		public DateOnly DateOfBirth { get; init; }

		/// <summary>Gets the employee's gender.</summary>
		public Gender Gender { get; init; }

		/// <summary>Gets the employee's job title.</summary>
		public string JobTitle { get; init; } = string.Empty;

		/// <summary>Gets the employee's business phone number.</summary>
		public string BusinessNumber { get; init; } = string.Empty;

		/// <summary>Gets the employee's mobile phone number.</summary>
		public string MobileNumber { get; init; } = string.Empty;

		/// <summary>Gets the employee's email address.</summary>
		public string EmailAddress { get; init; } = string.Empty;

		/// <summary>Gets the automatically assigned employee number.</summary>
		public int EmployeeNumber { get; init; }

		/// <summary>Gets the employee's display name.</summary>
		public string Name { get; init; } = string.Empty;

		/// <summary>Gets the employee's department.</summary>
		public string Department { get; init; } = string.Empty;

		/// <summary>Gets the employee's displayed employment end date.</summary>
		public string EmploymentEndDate { get; init; } = string.Empty;

		/// <summary>Gets the employee type display text.</summary>
		public string EmployeeType { get; init; } = string.Empty;

		/// <summary>Gets the employee's office location.</summary>
		public OfficeLocation OfficeLocation { get; init; }

		/// <summary>Gets the employee's management level.</summary>
		public ManagementLevel ManagementLevel { get; init; }

		/// <summary>Gets the employee's active-state display text.</summary>
		public string Status { get; init; } = string.Empty;

		/// <summary>Gets the apprenticeship duration in years.</summary>
		public ushort ApprenticeshipDuration { get; init; }

		/// <summary>Gets the apprentice's current apprenticeship year.</summary>
		public ushort CurrentApprenticeshipYear { get; init; }
	}

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

	/// <summary>
	/// Reloads the customer projection and optionally selects a customer by stable identifier.
	/// </summary>
	/// <param name="selectedCustomerId">The customer to select after reloading, if any.</param>
	private void ReloadCustomers(Guid? selectedCustomerId = null)
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

	/// <summary>Centers split views and the login preview after the form has its final initial size.</summary>
	private void InitializeLayout(object? sender, EventArgs e)
	{
		CenterSplitView(CustomersSplitView);
		CenterSplitView(EmployeesSplitView);
		CenterLoginPreview(this, EventArgs.Empty);
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

	/// <summary>Displays the in-place customer notes view.</summary>
	private void ShowCustomerNotesView(object? sender, EventArgs e)
	{
		ShowCustomerNotes(true);
	}

	/// <summary>Returns from customer notes to the detail view.</summary>
	private void HideCustomerNotesView(object? sender, EventArgs e)
	{
		ShowCustomerNotes(false);
	}

	/// <summary>Displays the selected customer's edit history view.</summary>
	private void ShowCustomerEditHistoryView(object? sender, EventArgs e)
	{
		ShowCustomerEditHistory(true);
	}

	/// <summary>Returns from customer edit history to the detail view.</summary>
	private void HideCustomerEditHistoryView(object? sender, EventArgs e)
	{
		ShowCustomerEditHistory(false);
	}

	/// <summary>Shows or hides the customer's per-person edit history.</summary>
	private void ShowCustomerEditHistory(bool visible)
	{
		CustomerDetailsScrollView.Visible = !visible;
		CustomerNotesView.Visible = false;
		CustomerEditHistoryView.Visible = visible;
	}

	/// <summary>Displays the selected employee's edit history view.</summary>
	private void ShowEmployeeEditHistoryView(object? sender, EventArgs e)
	{
		ShowEmployeeEditHistory(true);
	}

	/// <summary>Returns from employee edit history to the detail view.</summary>
	private void HideEmployeeEditHistoryView(object? sender, EventArgs e)
	{
		ShowEmployeeEditHistory(false);
	}

	/// <summary>Shows or hides the employee's per-person edit history.</summary>
	private void ShowEmployeeEditHistory(bool visible)
	{
		EmployeeDetailsScrollView.Visible = !visible;
		EmployeeEditHistoryView.Visible = visible;
	}

	/// <summary>Shows or hides customer notes without hiding the customer detail inputs.</summary>
	private void ShowCustomerNotes(bool visible)
	{
		CustomerDetailsScrollView.Visible = !visible;
		CustomerNotesView.Visible = visible;
		CustomerEditHistoryView.Visible = false;
	}

	/// <summary>Enters the UI-only new-note state.</summary>
	private void AddNewCustomerNote(object? sender, EventArgs e)
	{
		NewCustomerNoteArea.Visible = true;
		SaveCustomerNote.Visible = true;
		CancelCustomerNote.Visible = true;
		AddCustomerNote.Visible = false;
	}

	/// <summary>Returns from new-note state without persistence.</summary>
	private void CancelNewCustomerNote(object? sender, EventArgs e)
	{
		NewCustomerNoteArea.Visible = false;
		SaveCustomerNote.Visible = false;
		CancelCustomerNote.Visible = false;
		AddCustomerNote.Visible = true;
		NewCustomerNoteInput.Clear();
	}

	/// <summary>Shows that note persistence belongs to a later implementation phase.</summary>
	private void SaveNewCustomerNote(object? sender, EventArgs e)
	{
		CancelNewCustomerNote(sender, e);
		ShowPreviewMessage("Customer-note persistence is not connected yet.");
	}

	/// <summary>Shows the selected note placeholder without loading data.</summary>
	private void SelectCustomerNote(object? sender, EventArgs e)
	{
		CustomerNoteContent.Text = "Contact-note content will appear here when data is connected.";
	}

	/// <summary>Updates apprentice-only field visibility from the employee-type radio buttons.</summary>
	private void EmployeeTypeChanged(object? sender, EventArgs e)
	{
		SetApprenticeFieldsVisible(EmployeeTypeApprenticeOption.Checked);
	}

	/// <summary>Shows or hides apprentice inputs and their corresponding labels.</summary>
	private void SetApprenticeFieldsVisible(bool visible)
	{
		if (visible)
		{
			CurrentApprenticeshipYearInput.Maximum =
				Math.Max(ApprenticeshipDurationInput.Minimum, ApprenticeshipDurationInput.Value);
		}

		ApprenticeshipDurationInput.Visible = visible;
		CurrentApprenticeshipYearInput.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(0, 23)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(1, 23)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(0, 24)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(1, 24)!.Visible = visible;
	}

	/// <summary>Centers the login preview panel after its host is resized.</summary>
	private void CenterLoginPreview(object? sender, EventArgs e)
	{
		LoginPreview.Left = Math.Max(0, (LoginPreviewArea.ClientSize.Width - LoginPreview.Width) / 2);
		LoginPreview.Top = Math.Max(0, (LoginPreviewArea.ClientSize.Height - LoginPreview.Height) / 2);
	}

	/// <summary>Displays the non-authenticating login preview message.</summary>
	private void PreviewLoginMessage(object? sender, EventArgs e)
	{
		ShowPreviewMessage("Authentication will be connected later.");
	}

	/// <summary>Shows that CSV import will be connected in a later implementation phase.</summary>
	private void ImportFromCsv(object? sender, EventArgs e)
	{
		ShowPreviewMessage("CSV import is not connected yet.");
	}

	/// <summary>Shows that CSV export will be connected in a later implementation phase.</summary>
	private void ExportToCsv(object? sender, EventArgs e)
	{
		ShowPreviewMessage("CSV export is not connected yet.");
	}

	/// <summary>Applies read-only or editable state to customer inputs.</summary>
	private void SetCustomerEditorMode(bool editable, bool hasSelection)
	{
		customerEditMode = editable;
		foreach (TextBox input in new TextBox[]
		         {
			         CustomerFirstNameInput, CustomerLastNameInput, CustomerJobTitleInput, CustomerBusinessPhoneInput,
			         CustomerMobilePhoneInput, CustomerEmailInput, CustomerCompanyInput
		         })
		{
			input.ReadOnly = !editable;
		}

		foreach (Control input in new Control[]
			         { CustomerTitleInput, CustomerDateOfBirthInput, CustomerGenderInput, CustomerActiveInput })
		{
			input.Enabled = editable;
		}

		EditCustomer.Enabled = !editable && hasSelection;
		DeleteCustomer.Enabled = !editable && hasSelection;
		ViewCustomerNotes.Enabled = !editable && hasSelection;
		ViewCustomerHistory.Enabled = !editable && hasSelection;
		CreateCustomer.Enabled = !editable;
		CustomersGrid.Enabled = !editable;
		SaveCustomer.Visible = editable;
		CancelCustomerEdit.Visible = editable;
	}

	/// <summary>Applies read-only or editable state to employee inputs.</summary>
	private void SetEmployeeEditorMode(bool editable, bool hasSelection)
	{
		employeeEditMode = editable;
		foreach (TextBox input in new TextBox[]
		         {
			         EmployeeFirstNameInput, EmployeeLastNameInput, EmployeeDepartmentInput, EmployeeAhvNumberInput,
			         EmployeeNationalityInput, EmployeeCityInput, EmployeeAddressInput, EmployeePostalCodeInput,
			         EmployeeJobTitleInput, EmployeeBusinessPhoneInput, EmployeeMobilePhoneInput, EmployeeEmailInput
		         })
		{
			input.ReadOnly = !editable;
		}

		foreach (Control input in new Control[]
		         {
			         EmployeeTitleInput, EmployeeDateOfBirthInput, EmployeeGenderInput, EmployeeStartDateInput,
			         EmployeeEndDateInput, EmployeeEmploymentPercentageInput, EmployeeOfficeLocationInput,
			         EmployeeManagementLevelInput, EmployeeTypeSelection, ApprenticeshipDurationInput,
								 CurrentApprenticeshipYearInput, EmployeeActiveInput
		         })
		{
			input.Enabled = editable;
		}
		EmployeeTypeSelection.Enabled = editable && creatingEmployee;
		EmployeeIndefiniteInput.Enabled = editable;
		EmployeeEndDateInput.Enabled = editable && !EmployeeIndefiniteInput.Checked;

		EditEmployee.Enabled = !editable && hasSelection;
		DeleteEmployee.Enabled = !editable && hasSelection;
		ViewEmployeeHistory.Enabled = !editable && hasSelection;
		CreateEmployee.Enabled = !editable;
		EmployeesGrid.Enabled = !editable;
		EmployeeNumberInput.ReadOnly = true;
		SaveEmployee.Visible = editable;
		CancelEmployeeEdit.Visible = editable;
	}

	/// <summary>Enables or disables the employee end-date picker based on the indefinite option.</summary>
	/// <param name="sender">The indefinite checkbox.</param>
	/// <param name="e">The event data.</param>
	private void EmployeeIndefiniteChanged(object? sender, EventArgs e)
	{
		SetEmployeeEndDateDisplay(!EmployeeIndefiniteInput.Checked);
		EmployeeEndDateInput.Enabled = employeeEditMode && !EmployeeIndefiniteInput.Checked;
	}

	/// <summary>Displays a safe UI-only phase-one message without relying on a status header.</summary>
	private void ShowPreviewMessage(string message)
	{
		MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
	}

	/// <summary>Displays an error without allowing an expected operation failure to close the form.</summary>
	/// <param name="message">The user-friendly error message to display.</param>
	private void ShowErrorMessage(string message)
	{
		MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
	}
}
