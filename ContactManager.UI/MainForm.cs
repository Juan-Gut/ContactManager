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
		PopulateEnumComboBox<OfficeLocation>(EmployeeOfficeLocationInput, OfficeLocation.Unknown);
		PopulateEnumComboBox<ManagementLevel>(EmployeeManagementLevelInput);
		SetCustomerEditorMode(false, false);
		SetEmployeeEditorMode(false, false);
		Load += LoadInitialData;
		Shown += InitializeLayout;
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
			Status = customer.IsActive ? "Active" : "Inactive"
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

	/// <summary>Centers split views and the login preview after the form has its final initial size.</summary>
	private void InitializeLayout(object? sender, EventArgs e)
	{
		CenterSplitView(CustomersSplitView);
		CenterSplitView(EmployeesSplitView);
		CenterLoginPreview(this, EventArgs.Empty);
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
		SetCustomerEditorMode(customerEditMode, CustomersGrid.CurrentRow is not null);
		int noteCount = (CustomersGrid.CurrentRow?.DataBoundItem as CustomerListRow)?.ContactHistoryCount ?? 0;
		ViewCustomerNotes.Text = $"Contact notes ({noteCount})";
	}

	/// <summary>Updates employee action availability when a row is selected.</summary>
	private void SelectEmployee(object? sender, EventArgs e)
	{
		SetEmployeeEditorMode(employeeEditMode, EmployeesGrid.CurrentRow is not null);
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
		SetCustomerEditorMode(true, true);
	}

	/// <summary>Explains that customer deletion is not connected in Part 1.</summary>
	private void DeleteSelectedCustomer(object? sender, EventArgs e)
	{
		ShowPreviewMessage("Customer deletion is not connected yet.");
	}

	/// <summary>Saves a newly created customer and returns to customer list mode.</summary>
	/// <param name="sender">The save-customer button.</param>
	/// <param name="e">The event data.</param>
	private void SaveCustomerDetails(object? sender, EventArgs e)
	{
		if (!creatingCustomer)
		{
			ShowPreviewMessage("Editing customers will be connected in a later implementation phase.");
			return;
		}

		try
		{
			Customer customer = new()
			{
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
				Company = CustomerCompanyInput.Text.Trim()
			};

			personManager!.Add(customer);
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
		SetCustomerEditorMode(false, CustomersGrid.CurrentRow is not null);
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
		SetEmployeeEditorMode(true, false);
	}

	/// <summary>Enters employee edit mode for the selected preview row.</summary>
	private void EditEmployeeDetails(object? sender, EventArgs e)
	{
		SetEmployeeEditorMode(true, true);
	}

	/// <summary>Explains that employee deletion is not connected in Part 1.</summary>
	private void DeleteSelectedEmployee(object? sender, EventArgs e)
	{
		ShowPreviewMessage("Employee deletion is not connected yet.");
	}

	/// <summary>Returns the employee detail pane to list mode after the preview save action.</summary>
	private void SaveEmployeeDetails(object? sender, EventArgs e)
	{
		SetEmployeeEditorMode(false, EmployeesGrid.CurrentRow is not null);
		ShowPreviewMessage("Employee persistence is not connected yet.");
	}

	/// <summary>Cancels employee creation or editing without changing data.</summary>
	private void CancelEmployeeEditMode(object? sender, EventArgs e)
	{
		SetEmployeeEditorMode(false, EmployeesGrid.CurrentRow is not null);
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
		EmployeeDetailsFields.GetControlFromPosition(0, 22)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(1, 22)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(0, 23)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(1, 23)!.Visible = visible;
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
			         CurrentApprenticeshipYearInput
		         })
		{
			input.Enabled = editable;
		}

		EditEmployee.Enabled = !editable && hasSelection;
		DeleteEmployee.Enabled = !editable && hasSelection;
		ViewEmployeeHistory.Enabled = !editable && hasSelection;
		CreateEmployee.Enabled = !editable;
		EmployeeNumberInput.ReadOnly = true;
		SaveEmployee.Visible = editable;
		CancelEmployeeEdit.Visible = editable;
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
