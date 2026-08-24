namespace ContactManager.UI;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private TableLayoutPanel MainLayout = null!;
    private TabControl MainTabs = null!;
    private TabPage DashboardTab = null!;
    private TabPage CustomersTab = null!;
    private TabPage EmployeesTab = null!;
    private TabPage LogsTab = null!;
    private TabPage LoginPreviewTab = null!;
    private TableLayoutPanel DashboardLayout = null!;
    private Label DashboardHeading = null!;
    private TableLayoutPanel MetricsLayout = null!;
    private GroupBox CustomerMetrics = null!;
    private GroupBox EmployeeMetrics = null!;
    private GroupBox ActiveContactMetrics = null!;
    private Label CustomerCount = null!;
    private Label EmployeeCount = null!;
    private Label ActiveContactCount = null!;
    private TableLayoutPanel DashboardListsLayout = null!;
    private GroupBox UpcomingBirthdays = null!;
    private GroupBox UpcomingDepartures = null!;
    private DataGridView UpcomingBirthdaysGrid = null!;
    private DataGridView UpcomingDeparturesGrid = null!;
    private SplitContainer CustomersSplitView = null!;
    private TableLayoutPanel CustomerListLayout = null!;
    private Panel CustomerSearchArea = null!;
    private TextBox CustomerSearchInput = null!;
    private DataGridView CustomersGrid = null!;
    private TableLayoutPanel CustomerDetailsLayout = null!;
    private FlowLayoutPanel CustomerActions = null!;
    private Button CreateCustomer = null!;
    private Button EditCustomer = null!;
    private Button DeleteCustomer = null!;
    private Button ViewCustomerNotes = null!;
    private Button SaveCustomer = null!;
    private Button CancelCustomerEdit = null!;
    private Panel CustomerDetailsHost = null!;
    private Panel CustomerDetailsScrollView = null!;
    private GroupBox CustomerDetails = null!;
    private TableLayoutPanel CustomerDetailsFields = null!;
    private TextBox CustomerTitleInput = null!;
    private TextBox CustomerFirstNameInput = null!;
    private TextBox CustomerLastNameInput = null!;
    private DateTimePicker CustomerDateOfBirthInput = null!;
    private ComboBox CustomerGenderInput = null!;
    private TextBox CustomerJobTitleInput = null!;
    private TextBox CustomerBusinessPhoneInput = null!;
    private TextBox CustomerMobilePhoneInput = null!;
    private TextBox CustomerEmailInput = null!;
    private CheckBox CustomerActiveInput = null!;
    private TextBox CustomerCompanyInput = null!;
    private Panel CustomerNotesView = null!;
    private TableLayoutPanel CustomerNotesLayout = null!;
    private FlowLayoutPanel CustomerNotesActions = null!;
    private Button BackToCustomerDetails = null!;
    private Button AddCustomerNote = null!;
    private Button SaveCustomerNote = null!;
    private Button CancelCustomerNote = null!;
    private SplitContainer CustomerNotesSplitView = null!;
    private DataGridView CustomerContactEntriesGrid = null!;
    private TextBox CustomerNoteContent = null!;
    private Panel NewCustomerNoteArea = null!;
    private TextBox NewCustomerNoteInput = null!;
    private SplitContainer EmployeesSplitView = null!;
    private TableLayoutPanel EmployeeListLayout = null!;
    private Panel EmployeeSearchArea = null!;
    private TextBox EmployeeSearchInput = null!;
    private DataGridView EmployeesGrid = null!;
    private TableLayoutPanel EmployeeDetailsLayout = null!;
    private FlowLayoutPanel EmployeeActions = null!;
    private Button CreateEmployee = null!;
    private Button EditEmployee = null!;
    private Button DeleteEmployee = null!;
    private Button SaveEmployee = null!;
    private Button CancelEmployeeEdit = null!;
    private Panel EmployeeDetailsScrollView = null!;
    private GroupBox EmployeeDetails = null!;
    private TableLayoutPanel EmployeeDetailsFields = null!;
    private TextBox EmployeeNumberInput = null!;
    private TextBox EmployeeFirstNameInput = null!;
    private TextBox EmployeeLastNameInput = null!;
    private TextBox EmployeeDepartmentInput = null!;
    private TextBox EmployeeAhvNumberInput = null!;
    private TextBox EmployeeNationalityInput = null!;
    private TextBox EmployeeCityInput = null!;
    private TextBox EmployeeAddressInput = null!;
    private TextBox EmployeePostalCodeInput = null!;
    private DateTimePicker EmployeeStartDateInput = null!;
    private DateTimePicker EmployeeEndDateInput = null!;
    private NumericUpDown EmployeeEmploymentPercentageInput = null!;
    private ComboBox EmployeeOfficeLocationInput = null!;
    private ComboBox EmployeeManagementLevelInput = null!;
    private FlowLayoutPanel EmployeeTypeSelection = null!;
    private RadioButton EmployeeTypeEmployeeOption = null!;
    private RadioButton EmployeeTypeApprenticeOption = null!;
    private NumericUpDown ApprenticeshipDurationInput = null!;
    private NumericUpDown CurrentApprenticeshipYearInput = null!;
    private Label ApprenticeshipDurationLabel = null!;
    private Label CurrentApprenticeshipYearLabel = null!;
    private TableLayoutPanel LogsLayout = null!;
    private FlowLayoutPanel LogsActions = null!;
    private Button RefreshLogs = null!;
    private TextBox LogsContent = null!;
    private Panel LoginPreviewArea = null!;
    private TableLayoutPanel LoginPreview = null!;
    private Label LoginTitle = null!;
    private TextBox LoginUserNameInput = null!;
    private TextBox LoginPasswordInput = null!;
    private Button PreviewLogin = null!;
    private Label LoginPreviewInfo = null!;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        MainLayout = new TableLayoutPanel();
        MainTabs = new TabControl();
        DashboardTab = new TabPage("Dashboard"); CustomersTab = new TabPage("Customers"); EmployeesTab = new TabPage("Employees"); LogsTab = new TabPage("Logs"); LoginPreviewTab = new TabPage("Login preview");
        DashboardLayout = new TableLayoutPanel(); DashboardHeading = new Label(); MetricsLayout = new TableLayoutPanel(); CustomerMetrics = new GroupBox(); EmployeeMetrics = new GroupBox(); ActiveContactMetrics = new GroupBox(); CustomerCount = new Label(); EmployeeCount = new Label(); ActiveContactCount = new Label(); DashboardListsLayout = new TableLayoutPanel(); UpcomingBirthdays = new GroupBox(); UpcomingDepartures = new GroupBox(); UpcomingBirthdaysGrid = new DataGridView(); UpcomingDeparturesGrid = new DataGridView();
        CustomersSplitView = new SplitContainer(); CustomerListLayout = new TableLayoutPanel(); CustomerSearchArea = new Panel(); CustomerSearchInput = new TextBox(); CustomersGrid = new DataGridView(); CustomerDetailsLayout = new TableLayoutPanel(); CustomerActions = new FlowLayoutPanel(); CreateCustomer = new Button(); EditCustomer = new Button(); DeleteCustomer = new Button(); ViewCustomerNotes = new Button(); SaveCustomer = new Button(); CancelCustomerEdit = new Button(); CustomerDetailsHost = new Panel(); CustomerDetailsScrollView = new Panel(); CustomerDetails = new GroupBox(); CustomerDetailsLayout = new TableLayoutPanel();
        CustomerTitleInput = new TextBox(); CustomerFirstNameInput = new TextBox(); CustomerLastNameInput = new TextBox(); CustomerDateOfBirthInput = new DateTimePicker(); CustomerGenderInput = new ComboBox(); CustomerJobTitleInput = new TextBox(); CustomerBusinessPhoneInput = new TextBox(); CustomerMobilePhoneInput = new TextBox(); CustomerEmailInput = new TextBox(); CustomerActiveInput = new CheckBox(); CustomerCompanyInput = new TextBox(); CustomerDetailsFields = new TableLayoutPanel(); CustomerNotesView = new Panel(); CustomerNotesLayout = new TableLayoutPanel(); CustomerNotesActions = new FlowLayoutPanel(); BackToCustomerDetails = new Button(); AddCustomerNote = new Button(); SaveCustomerNote = new Button(); CancelCustomerNote = new Button(); CustomerNotesSplitView = new SplitContainer(); CustomerContactEntriesGrid = new DataGridView(); CustomerNoteContent = new TextBox(); NewCustomerNoteArea = new Panel(); NewCustomerNoteInput = new TextBox();
        EmployeesSplitView = new SplitContainer(); EmployeeListLayout = new TableLayoutPanel(); EmployeeSearchArea = new Panel(); EmployeeSearchInput = new TextBox(); EmployeesGrid = new DataGridView(); EmployeeDetailsLayout = new TableLayoutPanel(); EmployeeActions = new FlowLayoutPanel(); CreateEmployee = new Button(); EditEmployee = new Button(); DeleteEmployee = new Button(); SaveEmployee = new Button(); CancelEmployeeEdit = new Button(); EmployeeDetailsScrollView = new Panel(); EmployeeDetails = new GroupBox(); EmployeeDetailsFields = new TableLayoutPanel(); EmployeeNumberInput = new TextBox(); EmployeeDepartmentInput = new TextBox(); EmployeeFirstNameInput = new TextBox(); EmployeeLastNameInput = new TextBox();
        EmployeeAhvNumberInput = new TextBox(); EmployeeNationalityInput = new TextBox(); EmployeeCityInput = new TextBox(); EmployeeAddressInput = new TextBox(); EmployeePostalCodeInput = new TextBox(); EmployeeStartDateInput = new DateTimePicker(); EmployeeEndDateInput = new DateTimePicker(); EmployeeEmploymentPercentageInput = new NumericUpDown(); EmployeeOfficeLocationInput = new ComboBox(); EmployeeManagementLevelInput = new ComboBox(); EmployeeTypeSelection = new FlowLayoutPanel(); EmployeeTypeEmployeeOption = new RadioButton(); EmployeeTypeApprenticeOption = new RadioButton(); ApprenticeshipDurationInput = new NumericUpDown(); CurrentApprenticeshipYearInput = new NumericUpDown(); ApprenticeshipDurationLabel = new Label(); CurrentApprenticeshipYearLabel = new Label();
        LogsLayout = new TableLayoutPanel(); LogsActions = new FlowLayoutPanel(); RefreshLogs = new Button(); LogsContent = new TextBox(); LoginPreviewArea = new Panel(); LoginPreview = new TableLayoutPanel(); LoginTitle = new Label(); LoginUserNameInput = new TextBox(); LoginPasswordInput = new TextBox(); PreviewLogin = new Button(); LoginPreviewInfo = new Label();

        SuspendLayout();
        Text = "Contact Manager"; StartPosition = FormStartPosition.CenterScreen; MinimumSize = new Size(1100, 700); ClientSize = new Size(1400, 900); AutoScaleMode = AutoScaleMode.Font;
        MainLayout.Dock = DockStyle.Fill; MainLayout.ColumnCount = 1; MainLayout.RowCount = 1; MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        MainTabs.Dock = DockStyle.Fill; MainTabs.TabPages.AddRange(new[] { DashboardTab, CustomersTab, EmployeesTab, LogsTab, LoginPreviewTab }); MainLayout.Controls.Add(MainTabs, 0, 0); Controls.Add(MainLayout);

        DashboardLayout.Dock = DockStyle.Fill; DashboardLayout.Padding = new Padding(14); DashboardLayout.ColumnCount = 1; DashboardLayout.RowCount = 3; DashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); DashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 125)); DashboardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); DashboardHeading.Text = "Dashboard"; DashboardHeading.Dock = DockStyle.Fill; DashboardHeading.Font = new Font(Font, FontStyle.Bold); DashboardHeading.Font = new Font(DashboardHeading.Font.FontFamily, 16, FontStyle.Bold); DashboardHeading.TextAlign = ContentAlignment.MiddleLeft; DashboardLayout.Controls.Add(DashboardHeading, 0, 0);
        MetricsLayout.Dock = DockStyle.Fill; MetricsLayout.ColumnCount = 3; MetricsLayout.RowCount = 1; for (var i = 0; i < 3; i++) MetricsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333f));
        ConfigureMetric(CustomerMetrics, "Customers", CustomerCount); ConfigureMetric(EmployeeMetrics, "Employees and apprentices", EmployeeCount); ConfigureMetric(ActiveContactMetrics, "Active contacts", ActiveContactCount); MetricsLayout.Controls.Add(CustomerMetrics, 0, 0); MetricsLayout.Controls.Add(EmployeeMetrics, 1, 0); MetricsLayout.Controls.Add(ActiveContactMetrics, 2, 0); DashboardLayout.Controls.Add(MetricsLayout, 0, 1);
        DashboardListsLayout.Dock = DockStyle.Fill; DashboardListsLayout.ColumnCount = 2; DashboardListsLayout.RowCount = 1; DashboardListsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); DashboardListsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); ConfigureGrid(UpcomingBirthdaysGrid); ConfigureGrid(UpcomingDeparturesGrid); UpcomingBirthdays.Text = "Upcoming birthdays"; UpcomingDepartures.Text = "Contracts ending within six months"; UpcomingBirthdays.Dock = DockStyle.Fill; UpcomingDepartures.Dock = DockStyle.Fill; UpcomingBirthdaysGrid.Dock = DockStyle.Fill; UpcomingDeparturesGrid.Dock = DockStyle.Fill; UpcomingBirthdays.Controls.Add(UpcomingBirthdaysGrid); UpcomingDepartures.Controls.Add(UpcomingDeparturesGrid); DashboardListsLayout.Controls.Add(UpcomingBirthdays, 0, 0); DashboardListsLayout.Controls.Add(UpcomingDepartures, 1, 0); DashboardLayout.Controls.Add(DashboardListsLayout, 0, 2); DashboardTab.Controls.Add(DashboardLayout);
        AddTextColumn(UpcomingBirthdaysGrid, "Person name", "PersonName"); AddTextColumn(UpcomingBirthdaysGrid, "Contact type", "ContactType"); AddTextColumn(UpcomingBirthdaysGrid, "Date of birth", "DateOfBirth"); AddTextColumn(UpcomingBirthdaysGrid, "Next birthday", "NextBirthday"); AddTextColumn(UpcomingDeparturesGrid, "Employee no.", "EmployeeNumber"); AddTextColumn(UpcomingDeparturesGrid, "Name", "Name"); AddTextColumn(UpcomingDeparturesGrid, "Department", "Department"); AddTextColumn(UpcomingDeparturesGrid, "End date", "EndDate");

        BuildCustomerView(); BuildEmployeeView(); BuildLogsView(); BuildLoginView();
        ((System.ComponentModel.ISupportInitialize)CustomersSplitView).EndInit(); ((System.ComponentModel.ISupportInitialize)EmployeesSplitView).EndInit(); ((System.ComponentModel.ISupportInitialize)CustomerNotesSplitView).EndInit(); ResumeLayout(false);
    }

    private void ConfigureMetric(GroupBox box, string title, Label value) { box.Text = title; box.Dock = DockStyle.Fill; value.Text = "—"; value.Dock = DockStyle.Fill; value.Font = new Font(Font, FontStyle.Bold); value.Font = new Font(value.Font.FontFamily, 24, FontStyle.Bold); value.TextAlign = ContentAlignment.MiddleCenter; box.Controls.Add(value); }
    private void ConfigureGrid(DataGridView grid) { grid.Dock = DockStyle.Fill; grid.ReadOnly = true; grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.AllowUserToResizeRows = false; grid.AutoGenerateColumns = false; grid.MultiSelect = false; grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; }
    private void AddTextColumn(DataGridView grid, string header, string property) { grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = header, DataPropertyName = property, Name = property, SortMode = DataGridViewColumnSortMode.NotSortable }); }
    private void BuildCustomerView()
    {
        CustomersSplitView.Dock = DockStyle.Fill; CustomersSplitView.Orientation = Orientation.Vertical; CustomersSplitView.IsSplitterFixed = false;
        CustomerListLayout.Dock = DockStyle.Fill; CustomerListLayout.RowCount = 2; CustomerListLayout.ColumnCount = 1; CustomerListLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); CustomerListLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        CustomerSearchArea.Padding = new Padding(12, 8, 12, 8); CustomerSearchInput.Dock = DockStyle.Fill; CustomerSearchInput.PlaceholderText = "Search customers…"; CustomerSearchInput.TextChanged += SearchCustomers; CustomerSearchArea.Controls.Add(CustomerSearchInput); CustomerListLayout.Controls.Add(CustomerSearchArea, 0, 0); ConfigureGrid(CustomersGrid); AddTextColumn(CustomersGrid, "Customer name", "CustomerName"); AddTextColumn(CustomersGrid, "Company", "Company"); AddTextColumn(CustomersGrid, "Email", "Email"); AddTextColumn(CustomersGrid, "Phone", "Phone"); AddTextColumn(CustomersGrid, "Status", "Status"); CustomersGrid.SelectionChanged += SelectCustomer; CustomerListLayout.Controls.Add(CustomersGrid, 0, 1); CustomersSplitView.Panel1.Controls.Add(CustomerListLayout);
        CustomerDetailsLayout.Dock = DockStyle.Fill; CustomerDetailsLayout.RowCount = 2; CustomerDetailsLayout.ColumnCount = 1; CustomerDetailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); CustomerDetailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        ConfigureButton(CreateCustomer, "New customer", CreateNewCustomer); ConfigureButton(EditCustomer, "Edit", EditCustomerDetails); ConfigureButton(DeleteCustomer, "Delete", DeleteSelectedCustomer); ConfigureButton(ViewCustomerNotes, "Contact notes", ShowCustomerNotesView); ConfigureButton(SaveCustomer, "Save", SaveCustomerDetails); ConfigureButton(CancelCustomerEdit, "Cancel", CancelCustomerEditMode); SaveCustomer.Visible = false; CancelCustomerEdit.Visible = false; CustomerActions.Dock = DockStyle.Fill; CustomerActions.WrapContents = false; CustomerActions.AutoScroll = false; CustomerActions.Padding = new Padding(8, 8, 8, 4); CustomerActions.Controls.AddRange(new Control[] { CreateCustomer, EditCustomer, DeleteCustomer, ViewCustomerNotes, SaveCustomer, CancelCustomerEdit }); CustomerDetailsLayout.Controls.Add(CustomerActions, 0, 0);
        CustomerDetailsHost.Dock = DockStyle.Fill; CustomerDetailsScrollView.Dock = DockStyle.Fill; CustomerDetailsScrollView.AutoScroll = true; CustomerDetails.Text = "Customer details"; CustomerDetails.Dock = DockStyle.Top; CustomerDetails.AutoSize = true; CustomerDetailsLayout.Dock = DockStyle.Fill;
        CustomerDetailsFields.Dock = DockStyle.Fill; CustomerDetailsFields.AutoSize = true; CustomerDetailsFields.ColumnCount = 2; CustomerDetailsFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38)); CustomerDetailsFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        AddDetailRow(CustomerDetailsFields, "Title", CustomerTitleInput, 0);
        AddDetailRow(CustomerDetailsFields, "First name", CustomerFirstNameInput, 1);
        AddDetailRow(CustomerDetailsFields, "Last name", CustomerLastNameInput, 2);
        AddDetailRow(CustomerDetailsFields, "Date of birth", CustomerDateOfBirthInput, 3);
        AddDetailRow(CustomerDetailsFields, "Gender", CustomerGenderInput, 4);
        AddDetailRow(CustomerDetailsFields, "Job title", CustomerJobTitleInput, 5);
        AddDetailRow(CustomerDetailsFields, "Business phone", CustomerBusinessPhoneInput, 6);
        AddDetailRow(CustomerDetailsFields, "Mobile phone", CustomerMobilePhoneInput, 7);
        AddDetailRow(CustomerDetailsFields, "Email", CustomerEmailInput, 8);
        AddDetailRow(CustomerDetailsFields, "Active", CustomerActiveInput, 9);
        AddDetailRow(CustomerDetailsFields, "Company", CustomerCompanyInput, 10);
        ConfigureReadOnlyCustomerFields(); CustomerDetails.Controls.Add(CustomerDetailsFields); CustomerDetailsScrollView.Controls.Add(CustomerDetails); CustomerDetailsHost.Controls.Add(CustomerDetailsScrollView); BuildCustomerNotes(); CustomerDetailsHost.Controls.Add(CustomerNotesView);
        CustomerDetailsLayout.Controls.Add(CustomerDetailsHost, 0, 1);
        CustomersSplitView.Panel2.Controls.Add(CustomerDetailsLayout);
        CustomersTab.Controls.Add(CustomersSplitView);
    }

    private void BuildCustomerNotes()
    {
        CustomerNotesView.Dock = DockStyle.Fill; CustomerNotesView.Visible = false; CustomerNotesLayout.Dock = DockStyle.Fill; CustomerNotesLayout.RowCount = 3; CustomerNotesLayout.ColumnCount = 1; CustomerNotesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); CustomerNotesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); CustomerNotesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
        ConfigureButton(BackToCustomerDetails, "Back to details", HideCustomerNotesView); ConfigureButton(AddCustomerNote, "Add note", AddNewCustomerNote); ConfigureButton(SaveCustomerNote, "Save note", SaveNewCustomerNote); ConfigureButton(CancelCustomerNote, "Cancel", CancelNewCustomerNote); SaveCustomerNote.Visible = false; CancelCustomerNote.Visible = false; CustomerNotesActions.Dock = DockStyle.Fill; CustomerNotesActions.Controls.AddRange(new Control[] { BackToCustomerDetails, AddCustomerNote, SaveCustomerNote, CancelCustomerNote }); CustomerNotesLayout.Controls.Add(CustomerNotesActions, 0, 0);
        CustomerNotesSplitView.Dock = DockStyle.Fill; CustomerNotesSplitView.SplitterDistance = 280; ConfigureGrid(CustomerContactEntriesGrid); AddTextColumn(CustomerContactEntriesGrid, "Contact date", "CreatedAt"); CustomerContactEntriesGrid.SelectionChanged += SelectCustomerNote; CustomerNotesSplitView.Panel1.Controls.Add(CustomerContactEntriesGrid); CustomerNoteContent.Multiline = true; CustomerNoteContent.ReadOnly = true; CustomerNoteContent.ScrollBars = ScrollBars.Both; CustomerNoteContent.Dock = DockStyle.Fill; CustomerNotesSplitView.Panel2.Controls.Add(CustomerNoteContent); CustomerNotesLayout.Controls.Add(CustomerNotesSplitView, 0, 1); NewCustomerNoteInput.Multiline = true; NewCustomerNoteInput.ScrollBars = ScrollBars.Vertical; NewCustomerNoteInput.Dock = DockStyle.Fill; NewCustomerNoteArea.Padding = new Padding(8); NewCustomerNoteArea.Visible = false; NewCustomerNoteArea.Controls.Add(NewCustomerNoteInput); CustomerNotesLayout.Controls.Add(NewCustomerNoteArea, 0, 2); CustomerNotesView.Controls.Add(CustomerNotesLayout);
    }

    private void BuildEmployeeView()
    {
        EmployeesSplitView.Dock = DockStyle.Fill; EmployeesSplitView.Orientation = Orientation.Vertical; EmployeesSplitView.IsSplitterFixed = false; EmployeeListLayout.Dock = DockStyle.Fill; EmployeeListLayout.RowCount = 2; EmployeeListLayout.ColumnCount = 1; EmployeeListLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); EmployeeListLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); EmployeeSearchArea.Padding = new Padding(12, 8, 12, 8); EmployeeSearchInput.Dock = DockStyle.Fill; EmployeeSearchInput.PlaceholderText = "Search employees…"; EmployeeSearchInput.TextChanged += SearchEmployees; EmployeeSearchArea.Controls.Add(EmployeeSearchInput); EmployeeListLayout.Controls.Add(EmployeeSearchArea, 0, 0); ConfigureGrid(EmployeesGrid); foreach (var column in new[] { ("Employee no.", "EmployeeNumber"), ("Name", "Name"), ("Department", "Department"), ("Job title", "JobTitle"), ("Status", "Status"), ("Leaving date", "EmploymentEndDate"), ("Employee type", "EmployeeType") }) AddTextColumn(EmployeesGrid, column.Item1, column.Item2); EmployeesGrid.SelectionChanged += SelectEmployee; EmployeeListLayout.Controls.Add(EmployeesGrid, 0, 1); EmployeesSplitView.Panel1.Controls.Add(EmployeeListLayout);
        EmployeeDetailsLayout.Dock = DockStyle.Fill; EmployeeDetailsLayout.RowCount = 2; EmployeeDetailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); EmployeeDetailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); ConfigureButton(CreateEmployee, "New employee", CreateNewEmployee); ConfigureButton(EditEmployee, "Edit", EditEmployeeDetails); ConfigureButton(DeleteEmployee, "Delete", DeleteSelectedEmployee); ConfigureButton(SaveEmployee, "Save", SaveEmployeeDetails); ConfigureButton(CancelEmployeeEdit, "Cancel", CancelEmployeeEditMode); SaveEmployee.Visible = false; CancelEmployeeEdit.Visible = false; EmployeeActions.Dock = DockStyle.Fill; EmployeeActions.WrapContents = false; EmployeeActions.AutoScroll = false; EmployeeActions.Padding = new Padding(8, 8, 8, 4); EmployeeActions.Controls.AddRange(new Control[] { CreateEmployee, EditEmployee, DeleteEmployee, SaveEmployee, CancelEmployeeEdit }); EmployeeDetailsLayout.Controls.Add(EmployeeActions, 0, 0);
        EmployeeDetailsScrollView.Dock = DockStyle.Fill; EmployeeDetailsScrollView.AutoScroll = true; EmployeeDetails.Text = "Employee details"; EmployeeDetails.Dock = DockStyle.Top; EmployeeDetails.AutoSize = true;
        EmployeeFirstNameInput.ReadOnly = true; EmployeeLastNameInput.ReadOnly = true;
        EmployeeDetailsFields.Dock = DockStyle.Fill; EmployeeDetailsFields.ColumnCount = 2; EmployeeDetailsFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38)); EmployeeDetailsFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        ConfigureEmployeeTypeSelection(); AddDetailRow(EmployeeDetailsFields, "Employee type", EmployeeTypeSelection, 0);
        AddDetailRow(EmployeeDetailsFields, "Employee number", EmployeeNumberInput, 1);
        AddDetailRow(EmployeeDetailsFields, "First name", EmployeeFirstNameInput, 2);
        AddDetailRow(EmployeeDetailsFields, "Last name", EmployeeLastNameInput, 3);
        AddDetailRow(EmployeeDetailsFields, "Department", EmployeeDepartmentInput, 4);
        AddDetailRow(EmployeeDetailsFields, "AHV number", EmployeeAhvNumberInput, 5);
        AddDetailRow(EmployeeDetailsFields, "Nationality", EmployeeNationalityInput, 6);
        AddDetailRow(EmployeeDetailsFields, "City", EmployeeCityInput, 7);
        AddDetailRow(EmployeeDetailsFields, "Address", EmployeeAddressInput, 8);
        AddDetailRow(EmployeeDetailsFields, "Postal code", EmployeePostalCodeInput, 9);
        AddDetailRow(EmployeeDetailsFields, "Employment start date", EmployeeStartDateInput, 10);
        AddDetailRow(EmployeeDetailsFields, "Employment end date", EmployeeEndDateInput, 11);
        AddDetailRow(EmployeeDetailsFields, "Employment percentage", EmployeeEmploymentPercentageInput, 12);
        AddDetailRow(EmployeeDetailsFields, "Office location", EmployeeOfficeLocationInput, 13);
        AddDetailRow(EmployeeDetailsFields, "Management level", EmployeeManagementLevelInput, 14);
        AddDetailRow(EmployeeDetailsFields, "Apprenticeship duration", ApprenticeshipDurationInput, 15);
        AddDetailRow(EmployeeDetailsFields, "Current apprenticeship year", CurrentApprenticeshipYearInput, 16);
        EmployeeDetailsFields.Dock = DockStyle.Fill;
        EmployeeDetailsFields.AutoSize = true;
        EmployeeDetailsTypeDefaults(); EmployeeDetails.Controls.Add(EmployeeDetailsFields); EmployeeDetailsScrollView.Controls.Add(EmployeeDetails); EmployeeDetailsLayout.Controls.Add(EmployeeDetailsScrollView, 0, 1);
        EmployeesSplitView.Panel2.Controls.Add(EmployeeDetailsLayout);
        EmployeesTab.Controls.Add(EmployeesSplitView);
    }

    private void BuildLogsView() { LogsLayout.Dock = DockStyle.Fill; LogsLayout.RowCount = 2; LogsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); LogsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); ConfigureButton(RefreshLogs, "Refresh", RefreshLogView); LogsActions.Dock = DockStyle.Fill; LogsActions.Padding = new Padding(8); LogsActions.Controls.Add(RefreshLogs); LogsContent.Dock = DockStyle.Fill; LogsContent.Multiline = true; LogsContent.ReadOnly = true; LogsContent.ScrollBars = ScrollBars.Both; LogsContent.WordWrap = false; LogsContent.Font = new Font(FontFamily.GenericMonospace, 9); LogsContent.Text = "No log file has been connected yet."; LogsLayout.Controls.Add(LogsActions, 0, 0); LogsLayout.Controls.Add(LogsContent, 0, 1); LogsTab.Controls.Add(LogsLayout); }
    private void BuildLoginView() { LoginPreviewArea.Dock = DockStyle.Fill; LoginPreview = new TableLayoutPanel { Dock = DockStyle.None, Size = new Size(480, 260), ColumnCount = 1, RowCount = 5, Padding = new Padding(28), BackColor = SystemColors.Control }; LoginPreview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Absolute, 44)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); LoginTitle.Text = "Sign in"; LoginTitle.Font = new Font(Font, FontStyle.Bold); LoginTitle.Font = new Font(LoginTitle.Font.FontFamily, 16, FontStyle.Bold); LoginTitle.Dock = DockStyle.Fill; LoginTitle.TextAlign = ContentAlignment.MiddleCenter; LoginUserNameInput.PlaceholderText = "Username"; LoginUserNameInput.Dock = DockStyle.Fill; LoginPasswordInput.PlaceholderText = "Password"; LoginPasswordInput.UseSystemPasswordChar = true; LoginPasswordInput.Dock = DockStyle.Fill; PreviewLogin.Text = "Preview login"; PreviewLogin.Anchor = AnchorStyles.None; PreviewLogin.AutoSize = true; PreviewLogin.Click += PreviewLoginMessage; LoginPreviewInfo.Text = "Authentication will be connected later."; LoginPreviewInfo.Dock = DockStyle.Fill; LoginPreviewInfo.TextAlign = ContentAlignment.MiddleCenter; LoginPreviewInfo.ForeColor = Color.DimGray; LoginPreview.Controls.Add(LoginTitle, 0, 0); LoginPreview.Controls.Add(LoginUserNameInput, 0, 1); LoginPreview.Controls.Add(LoginPasswordInput, 0, 2); LoginPreview.Controls.Add(PreviewLogin, 0, 3); LoginPreview.Controls.Add(LoginPreviewInfo, 0, 4); LoginPreviewArea.Controls.Add(LoginPreview); LoginPreviewArea.Resize += CenterLoginPreview; LoginPreviewTab.Controls.Add(LoginPreviewArea); }
    private void ConfigureButton(Button button, string text, EventHandler handler) { button.Text = text; button.AutoSize = true; button.Click += handler; }
    private void AddDetailRow(TableLayoutPanel layout, string labelText, Control input, int row) { layout.RowCount = Math.Max(layout.RowCount, row + 1); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38)); var label = new Label { Text = labelText, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(8, 10, 4, 4) }; input.Dock = DockStyle.Fill; input.Margin = new Padding(4, 5, 8, 4); layout.Controls.Add(label, 0, row); layout.Controls.Add(input, 1, row); }
    private void ConfigureReadOnlyCustomerFields() { foreach (var input in new Control[] { CustomerTitleInput, CustomerDateOfBirthInput, CustomerGenderInput, CustomerActiveInput }) input.Enabled = false; foreach (var input in new TextBox[] { CustomerFirstNameInput, CustomerLastNameInput, CustomerJobTitleInput, CustomerBusinessPhoneInput, CustomerMobilePhoneInput, CustomerEmailInput, CustomerCompanyInput }) input.ReadOnly = true; CustomerGenderInput.DropDownStyle = ComboBoxStyle.DropDownList; }
    private void ConfigureEmployeeTypeSelection() { EmployeeTypeSelection.AutoSize = true; EmployeeTypeSelection.WrapContents = false; EmployeeTypeEmployeeOption.Text = "Employee"; EmployeeTypeEmployeeOption.AutoSize = true; EmployeeTypeEmployeeOption.Checked = true; EmployeeTypeApprenticeOption.Text = "Apprentice"; EmployeeTypeApprenticeOption.AutoSize = true; EmployeeTypeEmployeeOption.CheckedChanged += EmployeeTypeChanged; EmployeeTypeApprenticeOption.CheckedChanged += EmployeeTypeChanged; EmployeeTypeSelection.Controls.AddRange(new Control[] { EmployeeTypeEmployeeOption, EmployeeTypeApprenticeOption }); }
    private void EmployeeDetailsTypeDefaults() { EmployeeNumberInput.ReadOnly = true; EmployeeEmploymentPercentageInput.Minimum = 5; EmployeeEmploymentPercentageInput.Maximum = 100; EmployeeEmploymentPercentageInput.Value = 100; ApprenticeshipDurationInput.Minimum = 1; ApprenticeshipDurationInput.Maximum = 10; CurrentApprenticeshipYearInput.Minimum = 1; CurrentApprenticeshipYearInput.Maximum = 10; SetApprenticeFieldsVisible(false); }

    #endregion
}
