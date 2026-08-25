namespace ContactManager.UI;

public partial class MainForm : Form
{
	private bool customerEditMode;
	private bool employeeEditMode;

	public MainForm()
	{
		InitializeComponent();
		SetCustomerEditorMode(false, false);
		SetEmployeeEditorMode(false, false);
		Shown += InitializeLayout;
	}

	/// <summary>Centers split views after the form has its final initial size.</summary>
	private void InitializeLayout(object? sender, EventArgs e)
	{
		CenterSplitView(CustomersSplitView);
		CenterSplitView(EmployeesSplitView);
	}

	/// <summary>Centers a vertical split handle after the split view has its final runtime size.</summary>
	private static void CenterSplitView(SplitContainer splitView)
	{
		splitView.SplitterDistance = (splitView.ClientSize.Width - splitView.SplitterWidth) / 2;
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
		TextRenderer.DrawText(e.Graphics, tabPage.Text, MainTabs.Font, e.Bounds, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
	}

	/// <summary>Retains the search input as a UI-only preview until customer data is connected.</summary>
	private void SearchCustomers(object? sender, EventArgs e) { }

	/// <summary>Retains the search input as a UI-only preview until employee data is connected.</summary>
	private void SearchEmployees(object? sender, EventArgs e) { }

	/// <summary>Updates customer action availability when a row is selected.</summary>
	private void SelectCustomer(object? sender, EventArgs e) => SetCustomerEditorMode(customerEditMode, CustomersGrid.CurrentRow is not null);

	/// <summary>Updates employee action availability when a row is selected.</summary>
	private void SelectEmployee(object? sender, EventArgs e) => SetEmployeeEditorMode(employeeEditMode, EmployeesGrid.CurrentRow is not null);

	/// <summary>Clears the customer detail preview because no real data is connected.</summary>
	private void CreateNewCustomer(object? sender, EventArgs e) => SetCustomerEditorMode(true, false);

	/// <summary>Enters customer edit mode for the selected preview row.</summary>
	private void EditCustomerDetails(object? sender, EventArgs e) => SetCustomerEditorMode(true, true);

	/// <summary>Explains that customer deletion is not connected in Part 1.</summary>
	private void DeleteSelectedCustomer(object? sender, EventArgs e) => ShowPreviewMessage("Customer deletion is not connected yet.");

	/// <summary>Returns the customer detail pane to list mode after the preview save action.</summary>
	private void SaveCustomerDetails(object? sender, EventArgs e) { SetCustomerEditorMode(false, CustomersGrid.CurrentRow is not null); ShowPreviewMessage("Customer persistence is not connected yet."); }

	/// <summary>Cancels customer creation or editing without changing data.</summary>
	private void CancelCustomerEditMode(object? sender, EventArgs e) => SetCustomerEditorMode(false, CustomersGrid.CurrentRow is not null);

	/// <summary>Enters employee creation mode without constructing a model.</summary>
	private void CreateNewEmployee(object? sender, EventArgs e) => SetEmployeeEditorMode(true, false);

	/// <summary>Enters employee edit mode for the selected preview row.</summary>
	private void EditEmployeeDetails(object? sender, EventArgs e) => SetEmployeeEditorMode(true, true);

	/// <summary>Explains that employee deletion is not connected in Part 1.</summary>
	private void DeleteSelectedEmployee(object? sender, EventArgs e) => ShowPreviewMessage("Employee deletion is not connected yet.");

	/// <summary>Returns the employee detail pane to list mode after the preview save action.</summary>
	private void SaveEmployeeDetails(object? sender, EventArgs e) { SetEmployeeEditorMode(false, EmployeesGrid.CurrentRow is not null); ShowPreviewMessage("Employee persistence is not connected yet."); }

	/// <summary>Cancels employee creation or editing without changing data.</summary>
	private void CancelEmployeeEditMode(object? sender, EventArgs e) => SetEmployeeEditorMode(false, EmployeesGrid.CurrentRow is not null);

	/// <summary>Displays the in-place customer notes view.</summary>
	private void ShowCustomerNotesView(object? sender, EventArgs e) => ShowCustomerNotes(true);

	/// <summary>Returns from customer notes to the detail view.</summary>
	private void HideCustomerNotesView(object? sender, EventArgs e) => ShowCustomerNotes(false);

	/// <summary>Displays the selected customer's edit history view.</summary>
	private void ShowCustomerEditHistoryView(object? sender, EventArgs e) => ShowCustomerEditHistory(true);

	/// <summary>Returns from customer edit history to the detail view.</summary>
	private void HideCustomerEditHistoryView(object? sender, EventArgs e) => ShowCustomerEditHistory(false);

	/// <summary>Shows or hides the customer's per-person edit history.</summary>
	private void ShowCustomerEditHistory(bool visible)
	{
		CustomerDetailsScrollView.Visible = !visible;
		CustomerNotesView.Visible = false;
		CustomerEditHistoryView.Visible = visible;
	}

	/// <summary>Displays the selected employee's edit history view.</summary>
	private void ShowEmployeeEditHistoryView(object? sender, EventArgs e) => ShowEmployeeEditHistory(true);

	/// <summary>Returns from employee edit history to the detail view.</summary>
	private void HideEmployeeEditHistoryView(object? sender, EventArgs e) => ShowEmployeeEditHistory(false);

	/// <summary>Shows or hides the employee's per-person edit history.</summary>
	private void ShowEmployeeEditHistory(bool visible)
	{
		EmployeeDetailsScrollView.Visible = !visible;
		EmployeeEditHistoryView.Visible = visible;
	}

	/// <summary>Shows or hides customer notes without hiding the customer detail inputs.</summary>
	private void ShowCustomerNotes(bool visible) { CustomerDetailsScrollView.Visible = !visible; CustomerNotesView.Visible = visible; CustomerEditHistoryView.Visible = false; }

	/// <summary>Enters the UI-only new-note state.</summary>
	private void AddNewCustomerNote(object? sender, EventArgs e) { NewCustomerNoteArea.Visible = true; SaveCustomerNote.Visible = true; CancelCustomerNote.Visible = true; AddCustomerNote.Visible = false; }

	/// <summary>Returns from new-note state without persistence.</summary>
	private void CancelNewCustomerNote(object? sender, EventArgs e) { NewCustomerNoteArea.Visible = false; SaveCustomerNote.Visible = false; CancelCustomerNote.Visible = false; AddCustomerNote.Visible = true; NewCustomerNoteInput.Clear(); }

	/// <summary>Shows that note persistence belongs to a later implementation phase.</summary>
	private void SaveNewCustomerNote(object? sender, EventArgs e) { CancelNewCustomerNote(sender, e); ShowPreviewMessage("Customer-note persistence is not connected yet."); }

	/// <summary>Shows the selected note placeholder without loading data.</summary>
	private void SelectCustomerNote(object? sender, EventArgs e) => CustomerNoteContent.Text = "Contact-note content will appear here when data is connected.";

	/// <summary>Updates apprentice-only field visibility from the employee-type radio buttons.</summary>
	private void EmployeeTypeChanged(object? sender, EventArgs e) => SetApprenticeFieldsVisible(EmployeeTypeApprenticeOption.Checked);

	/// <summary>Shows or hides apprentice inputs and their corresponding labels.</summary>
	private void SetApprenticeFieldsVisible(bool visible)
	{
		ApprenticeshipDurationInput.Visible = visible;
		CurrentApprenticeshipYearInput.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(0, 15)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(1, 15)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(0, 16)!.Visible = visible;
		EmployeeDetailsFields.GetControlFromPosition(1, 16)!.Visible = visible;
	}

	/// <summary>Shows that CSV import will be connected in a later implementation phase.</summary>
	private void ImportFromCsv(object? sender, EventArgs e) => ShowPreviewMessage("CSV import is not connected yet.");

	/// <summary>Shows that CSV export will be connected in a later implementation phase.</summary>
	private void ExportToCsv(object? sender, EventArgs e) => ShowPreviewMessage("CSV export is not connected yet.");

	/// <summary>Applies read-only or editable state to customer inputs.</summary>
	private void SetCustomerEditorMode(bool editable, bool hasSelection) { customerEditMode = editable; foreach (var input in new TextBox[] { CustomerFirstNameInput, CustomerLastNameInput, CustomerJobTitleInput, CustomerBusinessPhoneInput, CustomerMobilePhoneInput, CustomerEmailInput, CustomerCompanyInput }) input.ReadOnly = !editable; foreach (var input in new Control[] { CustomerTitleInput, CustomerDateOfBirthInput, CustomerGenderInput, CustomerActiveInput }) input.Enabled = editable; EditCustomer.Enabled = !editable && hasSelection; DeleteCustomer.Enabled = !editable && hasSelection; ViewCustomerNotes.Enabled = !editable && hasSelection; ViewCustomerHistory.Enabled = !editable && hasSelection; CreateCustomer.Enabled = !editable; SaveCustomer.Visible = editable; CancelCustomerEdit.Visible = editable; }

	/// <summary>Applies read-only or editable state to employee inputs.</summary>
	private void SetEmployeeEditorMode(bool editable, bool hasSelection) { employeeEditMode = editable; foreach (var input in new TextBox[] { EmployeeFirstNameInput, EmployeeLastNameInput, EmployeeDepartmentInput, EmployeeAhvNumberInput, EmployeeNationalityInput, EmployeeCityInput, EmployeeAddressInput, EmployeePostalCodeInput }) input.ReadOnly = !editable; foreach (var input in new Control[] { EmployeeStartDateInput, EmployeeEndDateInput, EmployeeEmploymentPercentageInput, EmployeeOfficeLocationInput, EmployeeManagementLevelInput, EmployeeTypeSelection, ApprenticeshipDurationInput, CurrentApprenticeshipYearInput }) input.Enabled = editable; EditEmployee.Enabled = !editable && hasSelection; DeleteEmployee.Enabled = !editable && hasSelection; ViewEmployeeHistory.Enabled = !editable && hasSelection; CreateEmployee.Enabled = !editable; EmployeeNumberInput.ReadOnly = true; SaveEmployee.Visible = editable; CancelEmployeeEdit.Visible = editable; }

	/// <summary>Displays a safe UI-only phase-one message without relying on a status header.</summary>
	private void ShowPreviewMessage(string message) => MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
}
