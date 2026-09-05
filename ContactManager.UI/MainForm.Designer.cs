namespace ContactManager.UI;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private TableLayoutPanel MainLayout = null!;
    private MenuStrip MainMenu = null!;
    private ToolStripMenuItem FileMenu = null!;
    private ToolStripMenuItem ImportFromCsvMenuItem = null!;
    private ToolStripMenuItem ExportToCsvMenuItem = null!;
    private TabControl MainTabs = null!;
    private TabPage DashboardTab = null!;
    private TabPage CustomersTab = null!;
    private TabPage EmployeesTab = null!;
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
    private TableLayoutPanel CustomerActions = null!;
    private FlowLayoutPanel CustomerPrimaryActions = null!;
    private FlowLayoutPanel CustomerSecondaryActions = null!;
    private TableLayoutPanel CustomerEditActions = null!;
    private Button CreateCustomer = null!;
    private Button EditCustomer = null!;
    private Button DeleteCustomer = null!;
    private Button ViewCustomerNotes = null!;
    private Button ViewCustomerHistory = null!;
    private Button SaveCustomer = null!;
    private Button CancelCustomerEdit = null!;
    private Panel CustomerDetailsHost = null!;
    private Panel CustomerDetailsScrollView = null!;
    private GroupBox CustomerDetails = null!;
    private TableLayoutPanel CustomerDetailsFields = null!;
    private ComboBox CustomerTitleInput = null!;
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
    private Panel CustomerEditHistoryView = null!;
    private TableLayoutPanel CustomerEditHistoryLayout = null!;
    private FlowLayoutPanel CustomerEditHistoryActions = null!;
    private Button BackFromCustomerEditHistory = null!;
    private DataGridView CustomerEditHistoryGrid = null!;
    private TextBox CustomerEditHistoryContent = null!;
    private SplitContainer EmployeesSplitView = null!;
    private TableLayoutPanel EmployeeListLayout = null!;
    private Panel EmployeeSearchArea = null!;
    private TextBox EmployeeSearchInput = null!;
    private DataGridView EmployeesGrid = null!;
    private TableLayoutPanel EmployeeDetailsLayout = null!;
    private TableLayoutPanel EmployeeActions = null!;
    private FlowLayoutPanel EmployeePrimaryActions = null!;
    private FlowLayoutPanel EmployeeSecondaryActions = null!;
    private TableLayoutPanel EmployeeEditActions = null!;
    private Button CreateEmployee = null!;
    private Button EditEmployee = null!;
    private Button DeleteEmployee = null!;
    private Button ViewEmployeeHistory = null!;
    private Button SaveEmployee = null!;
    private Button CancelEmployeeEdit = null!;
    private Panel EmployeeDetailsScrollView = null!;
    private GroupBox EmployeeDetails = null!;
    private TableLayoutPanel EmployeeDetailsFields = null!;
    private TextBox EmployeeNumberInput = null!;
    private ComboBox EmployeeTitleInput = null!;
    private TextBox EmployeeDepartmentInput = null!;
    private TextBox EmployeeFirstNameInput = null!;
    private TextBox EmployeeLastNameInput = null!;
    private DateTimePicker EmployeeDateOfBirthInput = null!;
    private ComboBox EmployeeGenderInput = null!;
    private TextBox EmployeeJobTitleInput = null!;
    private TextBox EmployeeBusinessPhoneInput = null!;
    private TextBox EmployeeMobilePhoneInput = null!;
    private TextBox EmployeeEmailInput = null!;
    private CheckBox EmployeeActiveInput = null!;
    private TextBox EmployeeAhvNumberInput = null!;
    private TextBox EmployeeNationalityInput = null!;
    private TextBox EmployeeCityInput = null!;
    private TextBox EmployeeAddressInput = null!;
    private TextBox EmployeePostalCodeInput = null!;
    private DateTimePicker EmployeeStartDateInput = null!;
    private DateTimePicker EmployeeEndDateInput = null!;
    private TableLayoutPanel EmployeeEndDateArea = null!;
    private CheckBox EmployeeIndefiniteInput = null!;
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
    private Panel EmployeeEditHistoryView = null!;
    private TableLayoutPanel EmployeeEditHistoryLayout = null!;
    private FlowLayoutPanel EmployeeEditHistoryActions = null!;
    private Button BackFromEmployeeEditHistory = null!;
    private DataGridView EmployeeEditHistoryGrid = null!;
    private TextBox EmployeeEditHistoryContent = null!;
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
        MainLayout = new TableLayoutPanel(); MainMenu = new MenuStrip(); FileMenu = new ToolStripMenuItem(); ImportFromCsvMenuItem = new ToolStripMenuItem(); ExportToCsvMenuItem = new ToolStripMenuItem();
        MainTabs = new TabControl();
        DashboardTab = new TabPage("Dashboard"); CustomersTab = new TabPage("Customers"); EmployeesTab = new TabPage("Employees"); LoginPreviewTab = new TabPage("Login preview");
        DashboardLayout = new TableLayoutPanel(); DashboardHeading = new Label(); MetricsLayout = new TableLayoutPanel(); CustomerMetrics = new GroupBox(); EmployeeMetrics = new GroupBox(); ActiveContactMetrics = new GroupBox(); CustomerCount = new Label(); EmployeeCount = new Label(); ActiveContactCount = new Label(); DashboardListsLayout = new TableLayoutPanel(); UpcomingBirthdays = new GroupBox(); UpcomingDepartures = new GroupBox(); UpcomingBirthdaysGrid = new DataGridView(); UpcomingDeparturesGrid = new DataGridView();
        CustomersSplitView = new SplitContainer(); CustomerListLayout = new TableLayoutPanel(); CustomerSearchArea = new Panel(); CustomerSearchInput = new TextBox(); CustomersGrid = new DataGridView(); CustomerDetailsLayout = new TableLayoutPanel(); CustomerActions = new TableLayoutPanel(); CustomerPrimaryActions = new FlowLayoutPanel(); CustomerSecondaryActions = new FlowLayoutPanel(); CustomerEditActions = new TableLayoutPanel(); CreateCustomer = new Button(); EditCustomer = new Button(); DeleteCustomer = new Button(); ViewCustomerNotes = new Button(); ViewCustomerHistory = new Button(); SaveCustomer = new Button(); CancelCustomerEdit = new Button(); CustomerDetailsHost = new Panel(); CustomerDetailsScrollView = new Panel(); CustomerDetails = new GroupBox(); CustomerDetailsLayout = new TableLayoutPanel();
        CustomerTitleInput = new ComboBox(); CustomerFirstNameInput = new TextBox(); CustomerLastNameInput = new TextBox(); CustomerDateOfBirthInput = new DateTimePicker(); CustomerGenderInput = new ComboBox(); CustomerJobTitleInput = new TextBox(); CustomerBusinessPhoneInput = new TextBox(); CustomerMobilePhoneInput = new TextBox(); CustomerEmailInput = new TextBox(); CustomerActiveInput = new CheckBox(); CustomerCompanyInput = new TextBox(); CustomerDetailsFields = new TableLayoutPanel(); CustomerNotesView = new Panel(); CustomerNotesLayout = new TableLayoutPanel(); CustomerNotesActions = new FlowLayoutPanel(); BackToCustomerDetails = new Button(); AddCustomerNote = new Button(); SaveCustomerNote = new Button(); CancelCustomerNote = new Button(); CustomerNotesSplitView = new SplitContainer(); CustomerContactEntriesGrid = new DataGridView(); CustomerNoteContent = new TextBox(); NewCustomerNoteArea = new Panel(); NewCustomerNoteInput = new TextBox(); CustomerEditHistoryView = new Panel(); CustomerEditHistoryLayout = new TableLayoutPanel(); CustomerEditHistoryActions = new FlowLayoutPanel(); BackFromCustomerEditHistory = new Button(); CustomerEditHistoryGrid = new DataGridView(); CustomerEditHistoryContent = new TextBox();
        EmployeesSplitView = new SplitContainer(); EmployeeListLayout = new TableLayoutPanel(); EmployeeSearchArea = new Panel(); EmployeeSearchInput = new TextBox(); EmployeesGrid = new DataGridView(); EmployeeDetailsLayout = new TableLayoutPanel(); EmployeeActions = new TableLayoutPanel(); EmployeePrimaryActions = new FlowLayoutPanel(); EmployeeSecondaryActions = new FlowLayoutPanel(); EmployeeEditActions = new TableLayoutPanel(); CreateEmployee = new Button(); EditEmployee = new Button(); DeleteEmployee = new Button(); ViewEmployeeHistory = new Button(); SaveEmployee = new Button(); CancelEmployeeEdit = new Button(); EmployeeDetailsScrollView = new Panel(); EmployeeDetails = new GroupBox(); EmployeeDetailsFields = new TableLayoutPanel(); EmployeeNumberInput = new TextBox(); EmployeeTitleInput = new ComboBox(); EmployeeDepartmentInput = new TextBox(); EmployeeFirstNameInput = new TextBox(); EmployeeLastNameInput = new TextBox(); EmployeeDateOfBirthInput = new DateTimePicker(); EmployeeGenderInput = new ComboBox(); EmployeeJobTitleInput = new TextBox(); EmployeeBusinessPhoneInput = new TextBox(); EmployeeMobilePhoneInput = new TextBox(); EmployeeEmailInput = new TextBox(); EmployeeActiveInput = new CheckBox(); EmployeeAhvNumberInput = new TextBox(); EmployeeNationalityInput = new TextBox(); EmployeeCityInput = new TextBox(); EmployeeAddressInput = new TextBox(); EmployeePostalCodeInput = new TextBox(); EmployeeStartDateInput = new DateTimePicker(); EmployeeEndDateInput = new DateTimePicker(); EmployeeEndDateArea = new TableLayoutPanel(); EmployeeIndefiniteInput = new CheckBox(); EmployeeEmploymentPercentageInput = new NumericUpDown(); EmployeeOfficeLocationInput = new ComboBox(); EmployeeManagementLevelInput = new ComboBox(); EmployeeTypeSelection = new FlowLayoutPanel(); EmployeeTypeEmployeeOption = new RadioButton(); EmployeeTypeApprenticeOption = new RadioButton(); ApprenticeshipDurationInput = new NumericUpDown(); CurrentApprenticeshipYearInput = new NumericUpDown(); ApprenticeshipDurationLabel = new Label(); CurrentApprenticeshipYearLabel = new Label();
        EmployeeEditHistoryView = new Panel(); EmployeeEditHistoryLayout = new TableLayoutPanel(); EmployeeEditHistoryActions = new FlowLayoutPanel(); BackFromEmployeeEditHistory = new Button(); EmployeeEditHistoryGrid = new DataGridView(); EmployeeEditHistoryContent = new TextBox(); LoginPreviewArea = new Panel(); LoginPreview = new TableLayoutPanel(); LoginTitle = new Label(); LoginUserNameInput = new TextBox(); LoginPasswordInput = new TextBox(); PreviewLogin = new Button(); LoginPreviewInfo = new Label();

        SuspendLayout();
        Text = "Contact Manager"; StartPosition = FormStartPosition.CenterScreen; WindowState = FormWindowState.Maximized; MinimumSize = new Size(1100, 700); ClientSize = new Size(1400, 900); AutoScaleMode = AutoScaleMode.Font;
        MainLayout.Dock = DockStyle.Fill; MainLayout.ColumnCount = 1; MainLayout.RowCount = 2; MainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24)); MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); MainMenu.Dock = DockStyle.Fill; FileMenu.Text = "File"; ImportFromCsvMenuItem.Text = "Import from CSV"; ExportToCsvMenuItem.Text = "Export to CSV"; ImportFromCsvMenuItem.Click += ImportFromCsv; ExportToCsvMenuItem.Click += ExportToCsv; FileMenu.DropDownItems.AddRange(new ToolStripItem[] { ImportFromCsvMenuItem, ExportToCsvMenuItem }); MainMenu.Items.Add(FileMenu); MainLayout.Controls.Add(MainMenu, 0, 0);
        MainTabs.Dock = DockStyle.Fill; MainTabs.Padding = new Point(12, 6); MainTabs.SizeMode = TabSizeMode.Fixed; MainTabs.ItemSize = new Size(120, 32); MainTabs.DrawMode = TabDrawMode.OwnerDrawFixed; MainTabs.DrawItem += DrawMainTab; DashboardTab.Padding = new Padding(12); CustomersTab.Padding = new Padding(12); EmployeesTab.Padding = new Padding(12); LoginPreviewTab.Padding = new Padding(12); MainTabs.TabPages.AddRange(new[] { DashboardTab, CustomersTab, EmployeesTab, LoginPreviewTab }); MainLayout.Controls.Add(MainTabs, 0, 1); Controls.Add(MainLayout);

        DashboardLayout.Dock = DockStyle.Fill; DashboardLayout.Padding = new Padding(14); DashboardLayout.ColumnCount = 1; DashboardLayout.RowCount = 3; DashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); DashboardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 125)); DashboardLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); DashboardHeading.Text = "Dashboard"; DashboardHeading.Dock = DockStyle.Fill; DashboardHeading.Font = new Font(Font, FontStyle.Bold); DashboardHeading.Font = new Font(DashboardHeading.Font.FontFamily, 16, FontStyle.Bold); DashboardHeading.TextAlign = ContentAlignment.MiddleLeft; DashboardLayout.Controls.Add(DashboardHeading, 0, 0);
        MetricsLayout.Dock = DockStyle.Fill; MetricsLayout.ColumnCount = 3; MetricsLayout.RowCount = 1; for (var i = 0; i < 3; i++) MetricsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333f));
        ConfigureMetric(CustomerMetrics, "Customers", CustomerCount); ConfigureMetric(EmployeeMetrics, "Employees and apprentices", EmployeeCount); ConfigureMetric(ActiveContactMetrics, "Active contacts", ActiveContactCount); MetricsLayout.Controls.Add(CustomerMetrics, 0, 0); MetricsLayout.Controls.Add(EmployeeMetrics, 1, 0); MetricsLayout.Controls.Add(ActiveContactMetrics, 2, 0); DashboardLayout.Controls.Add(MetricsLayout, 0, 1);
        DashboardListsLayout.Dock = DockStyle.Fill; DashboardListsLayout.ColumnCount = 2; DashboardListsLayout.RowCount = 1; DashboardListsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); DashboardListsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); ConfigureGrid(UpcomingBirthdaysGrid); ConfigureGrid(UpcomingDeparturesGrid); UpcomingBirthdays.Text = "Upcoming birthdays"; UpcomingDepartures.Text = "Contracts ending within six months"; UpcomingBirthdays.Dock = DockStyle.Fill; UpcomingDepartures.Dock = DockStyle.Fill; UpcomingBirthdaysGrid.Dock = DockStyle.Fill; UpcomingDeparturesGrid.Dock = DockStyle.Fill; UpcomingBirthdays.Controls.Add(UpcomingBirthdaysGrid); UpcomingDepartures.Controls.Add(UpcomingDeparturesGrid); DashboardListsLayout.Controls.Add(UpcomingBirthdays, 0, 0); DashboardListsLayout.Controls.Add(UpcomingDepartures, 1, 0); DashboardLayout.Controls.Add(DashboardListsLayout, 0, 2); DashboardTab.Controls.Add(DashboardLayout);
        AddTextColumn(UpcomingBirthdaysGrid, "Person name", "PersonName"); AddTextColumn(UpcomingBirthdaysGrid, "Contact type", "ContactType"); AddTextColumn(UpcomingBirthdaysGrid, "Date of birth", "DateOfBirth"); AddTextColumn(UpcomingBirthdaysGrid, "Next birthday", "NextBirthday"); AddTextColumn(UpcomingDeparturesGrid, "Employee no.", "EmployeeNumber"); AddTextColumn(UpcomingDeparturesGrid, "Name", "Name"); AddTextColumn(UpcomingDeparturesGrid, "Department", "Department"); AddTextColumn(UpcomingDeparturesGrid, "End date", "EndDate");

        BuildCustomerView(); BuildEmployeeView(); BuildLoginView();
        ((System.ComponentModel.ISupportInitialize)CustomersSplitView).EndInit(); ((System.ComponentModel.ISupportInitialize)EmployeesSplitView).EndInit(); ((System.ComponentModel.ISupportInitialize)CustomerNotesSplitView).EndInit(); ResumeLayout(false);
    }

    private void ConfigureMetric(GroupBox box, string title, Label value) { box.Text = title; box.Dock = DockStyle.Fill; value.Text = "—"; value.Dock = DockStyle.Fill; value.Font = new Font(Font, FontStyle.Bold); value.Font = new Font(value.Font.FontFamily, 24, FontStyle.Bold); value.TextAlign = ContentAlignment.MiddleCenter; box.Controls.Add(value); }
    private void ConfigureGrid(DataGridView grid) { grid.Dock = DockStyle.Fill; grid.ReadOnly = true; grid.AllowUserToAddRows = false; grid.AllowUserToDeleteRows = false; grid.AllowUserToResizeRows = false; grid.AutoGenerateColumns = false; grid.MultiSelect = false; grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; }
    private void AddTextColumn(DataGridView grid, string header, string property, int width = 0) { DataGridViewTextBoxColumn column = new() { HeaderText = header, DataPropertyName = property, Name = property, SortMode = DataGridViewColumnSortMode.NotSortable }; if (width > 0) { column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None; column.Width = width; } grid.Columns.Add(column); }
    private void BuildCustomerView()
    {
        CustomersSplitView.Dock = DockStyle.Fill; CustomersSplitView.Orientation = Orientation.Vertical; CustomersSplitView.IsSplitterFixed = false;
        CustomerListLayout.Dock = DockStyle.Fill; CustomerListLayout.RowCount = 2; CustomerListLayout.ColumnCount = 1; CustomerListLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); CustomerListLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        CustomerSearchArea.Padding = new Padding(12, 8, 12, 8); CustomerSearchInput.Dock = DockStyle.Fill; CustomerSearchInput.PlaceholderText = "Search customers…"; CustomerSearchInput.TextChanged += SearchCustomers; CustomerSearchArea.Controls.Add(CustomerSearchInput); CustomerListLayout.Controls.Add(CustomerSearchArea, 0, 0); ConfigureGrid(CustomersGrid); CustomersGrid.ScrollBars = ScrollBars.Both; AddTextColumn(CustomersGrid, "Title", "Title", 60); foreach (var column in new[] { ("First name", "FirstName"), ("Last name", "LastName"), ("Date of birth", "DateOfBirth"), ("Gender", "Gender"), ("Company", "Company"), ("Job title", "JobTitle"), ("Email", "EmailAddress"), ("Business phone", "BusinessNumber"), ("Mobile phone", "MobileNumber"), ("Status", "Status"), ("Created at", "CreatedAt") }) AddTextColumn(CustomersGrid, column.Item1, column.Item2); CustomersGrid.SelectionChanged += SelectCustomer; CustomerListLayout.Controls.Add(CustomersGrid, 0, 1); CustomersSplitView.Panel1.Controls.Add(CustomerListLayout);
        CustomerDetailsLayout.Dock = DockStyle.Fill; CustomerDetailsLayout.RowCount = 2; CustomerDetailsLayout.ColumnCount = 1; CustomerDetailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); CustomerDetailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        ConfigureButton(CreateCustomer, "New customer", CreateNewCustomer); ConfigureButton(EditCustomer, "Edit", EditCustomerDetails); ConfigureButton(DeleteCustomer, "Delete", DeleteSelectedCustomer); ConfigureButton(ViewCustomerNotes, "Contact notes (0)", ShowCustomerNotesView); ConfigureButton(ViewCustomerHistory, "History", ShowCustomerEditHistoryView); ConfigureButton(SaveCustomer, "Save", SaveCustomerDetails); ConfigureButton(CancelCustomerEdit, "Cancel", CancelCustomerEditMode); SaveCustomer.Visible = false; CancelCustomerEdit.Visible = false; CustomerActions.Dock = DockStyle.Fill; CustomerActions.ColumnCount = 2; CustomerActions.RowCount = 1; CustomerActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); CustomerActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); CustomerPrimaryActions.Dock = DockStyle.Fill; CustomerPrimaryActions.WrapContents = false; CustomerPrimaryActions.Padding = new Padding(8, 8, 4, 4); CustomerPrimaryActions.Controls.AddRange(new Control[] { CreateCustomer, EditCustomer, DeleteCustomer }); CustomerSecondaryActions.Dock = DockStyle.Fill; CustomerSecondaryActions.FlowDirection = FlowDirection.RightToLeft; CustomerSecondaryActions.WrapContents = false; CustomerSecondaryActions.Padding = new Padding(4, 8, 8, 4); CustomerSecondaryActions.Controls.AddRange(new Control[] { ViewCustomerNotes, ViewCustomerHistory }); CustomerActions.Controls.Add(CustomerPrimaryActions, 0, 0); CustomerActions.Controls.Add(CustomerSecondaryActions, 1, 0); CustomerDetailsLayout.Controls.Add(CustomerActions, 0, 0);
        CustomerEditActions.Dock = DockStyle.Top; CustomerEditActions.Height = 48; CustomerEditActions.ColumnCount = 2; CustomerEditActions.RowCount = 1; CustomerEditActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); CustomerEditActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); CancelCustomerEdit.Anchor = AnchorStyles.Left; SaveCustomer.Anchor = AnchorStyles.Right; CustomerEditActions.Controls.Add(CancelCustomerEdit, 0, 0); CustomerEditActions.Controls.Add(SaveCustomer, 1, 0);
        CustomerDetailsHost.Dock = DockStyle.Fill; CustomerDetailsScrollView.Dock = DockStyle.Fill; CustomerDetailsScrollView.AutoScroll = true; CustomerDetails.Text = "Customer details"; CustomerDetails.Dock = DockStyle.Top; CustomerDetails.AutoSize = true; CustomerDetailsLayout.Dock = DockStyle.Fill;
        CustomerDetailsFields.Dock = DockStyle.Fill; CustomerDetailsFields.AutoSize = true; CustomerDetailsFields.ColumnCount = 2; CustomerDetailsFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38)); CustomerDetailsFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        AddDetailRow(CustomerDetailsFields, "Active", CustomerActiveInput, 0);
        AddDetailRow(CustomerDetailsFields, "Title", CustomerTitleInput, 1);
        AddDetailRow(CustomerDetailsFields, "First name", CustomerFirstNameInput, 2);
        AddDetailRow(CustomerDetailsFields, "Last name", CustomerLastNameInput, 3);
        AddDetailRow(CustomerDetailsFields, "Date of birth", CustomerDateOfBirthInput, 4);
        AddDetailRow(CustomerDetailsFields, "Gender", CustomerGenderInput, 5);
        AddDetailRow(CustomerDetailsFields, "Job title", CustomerJobTitleInput, 6);
        AddDetailRow(CustomerDetailsFields, "Business phone", CustomerBusinessPhoneInput, 7);
        AddDetailRow(CustomerDetailsFields, "Mobile phone", CustomerMobilePhoneInput, 8);
        AddDetailRow(CustomerDetailsFields, "Email", CustomerEmailInput, 9);
        AddDetailRow(CustomerDetailsFields, "Company", CustomerCompanyInput, 10);
        ConfigureReadOnlyCustomerFields(); CustomerDetails.Controls.Add(CustomerDetailsFields); CustomerDetailsScrollView.Controls.Add(CustomerEditActions); CustomerDetailsScrollView.Controls.Add(CustomerDetails); CustomerDetailsHost.Controls.Add(CustomerDetailsScrollView); BuildCustomerNotes(); CustomerDetailsHost.Controls.Add(CustomerNotesView);
        CustomerDetailsLayout.Controls.Add(CustomerDetailsHost, 0, 1);
        CustomersSplitView.Panel2.Controls.Add(CustomerDetailsLayout);
        CustomersTab.Controls.Add(CustomersSplitView);
    }

    private void BuildCustomerNotes()
    {
        CustomerNotesView.Dock = DockStyle.Fill; CustomerNotesView.Visible = false; CustomerNotesLayout.Dock = DockStyle.Fill; CustomerNotesLayout.RowCount = 3; CustomerNotesLayout.ColumnCount = 1; CustomerNotesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); CustomerNotesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); CustomerNotesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
        ConfigureButton(BackToCustomerDetails, "Back to details", HideCustomerNotesView); ConfigureButton(AddCustomerNote, "Add note", AddNewCustomerNote); ConfigureButton(SaveCustomerNote, "Save note", SaveNewCustomerNote); ConfigureButton(CancelCustomerNote, "Cancel", CancelNewCustomerNote); SaveCustomerNote.Visible = false; CancelCustomerNote.Visible = false; CustomerNotesActions.Dock = DockStyle.Fill; CustomerNotesActions.Controls.AddRange(new Control[] { BackToCustomerDetails, AddCustomerNote, SaveCustomerNote, CancelCustomerNote }); CustomerNotesLayout.Controls.Add(CustomerNotesActions, 0, 0);
        CustomerNotesSplitView.Dock = DockStyle.Fill; CustomerNotesSplitView.SplitterDistance = 280; ConfigureGrid(CustomerContactEntriesGrid); AddTextColumn(CustomerContactEntriesGrid, "Contact date", "CreatedAt"); CustomerContactEntriesGrid.SelectionChanged += SelectCustomerNote; CustomerNotesSplitView.Panel1.Controls.Add(CustomerContactEntriesGrid); CustomerNoteContent.Multiline = true; CustomerNoteContent.ReadOnly = true; CustomerNoteContent.ScrollBars = ScrollBars.Both; CustomerNoteContent.Dock = DockStyle.Fill; CustomerNotesSplitView.Panel2.Controls.Add(CustomerNoteContent); CustomerNotesLayout.Controls.Add(CustomerNotesSplitView, 0, 1); NewCustomerNoteInput.Multiline = true; NewCustomerNoteInput.ScrollBars = ScrollBars.Vertical; NewCustomerNoteInput.Dock = DockStyle.Fill; NewCustomerNoteArea.Padding = new Padding(8); NewCustomerNoteArea.Visible = false; NewCustomerNoteArea.Controls.Add(NewCustomerNoteInput); CustomerNotesLayout.Controls.Add(NewCustomerNoteArea, 0, 2); CustomerNotesView.Controls.Add(CustomerNotesLayout); BuildCustomerEditHistory();
    }

    private void BuildCustomerEditHistory()
    {
        CustomerEditHistoryView.Dock = DockStyle.Fill; CustomerEditHistoryView.Visible = false; CustomerEditHistoryLayout.Dock = DockStyle.Fill; CustomerEditHistoryLayout.RowCount = 2; CustomerEditHistoryLayout.ColumnCount = 1; CustomerEditHistoryLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); CustomerEditHistoryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        ConfigureButton(BackFromCustomerEditHistory, "Back to details", HideCustomerEditHistoryView); CustomerEditHistoryActions.Dock = DockStyle.Fill; CustomerEditHistoryActions.Padding = new Padding(8, 8, 8, 4); CustomerEditHistoryActions.Controls.Add(BackFromCustomerEditHistory); CustomerEditHistoryLayout.Controls.Add(CustomerEditHistoryActions, 0, 0);
        var historySplitView = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical, SplitterDistance = 280 }; ConfigureGrid(CustomerEditHistoryGrid); AddTextColumn(CustomerEditHistoryGrid, "Changed at", "ChangedAt"); AddTextColumn(CustomerEditHistoryGrid, "Action", "Action"); historySplitView.Panel1.Controls.Add(CustomerEditHistoryGrid); CustomerEditHistoryContent.Multiline = true; CustomerEditHistoryContent.ReadOnly = true; CustomerEditHistoryContent.ScrollBars = ScrollBars.Both; CustomerEditHistoryContent.Dock = DockStyle.Fill; CustomerEditHistoryContent.Text = "Edit history will appear here when mutation history is connected."; historySplitView.Panel2.Controls.Add(CustomerEditHistoryContent); CustomerEditHistoryLayout.Controls.Add(historySplitView, 0, 1); CustomerEditHistoryView.Controls.Add(CustomerEditHistoryLayout); CustomerDetailsHost.Controls.Add(CustomerEditHistoryView);
    }

    private void BuildEmployeeEditHistory()
    {
        EmployeeEditHistoryView.Dock = DockStyle.Fill; EmployeeEditHistoryView.Visible = false; EmployeeEditHistoryLayout.Dock = DockStyle.Fill; EmployeeEditHistoryLayout.RowCount = 2; EmployeeEditHistoryLayout.ColumnCount = 1; EmployeeEditHistoryLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); EmployeeEditHistoryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); ConfigureButton(BackFromEmployeeEditHistory, "Back to details", HideEmployeeEditHistoryView); EmployeeEditHistoryActions.Dock = DockStyle.Fill; EmployeeEditHistoryActions.Padding = new Padding(8, 8, 8, 4); EmployeeEditHistoryActions.Controls.Add(BackFromEmployeeEditHistory); EmployeeEditHistoryLayout.Controls.Add(EmployeeEditHistoryActions, 0, 0); ConfigureGrid(EmployeeEditHistoryGrid); AddTextColumn(EmployeeEditHistoryGrid, "Changed at", "ChangedAt"); AddTextColumn(EmployeeEditHistoryGrid, "Action", "Action"); EmployeeEditHistoryContent.Multiline = true; EmployeeEditHistoryContent.ReadOnly = true; EmployeeEditHistoryContent.ScrollBars = ScrollBars.Both; EmployeeEditHistoryContent.Dock = DockStyle.Fill; EmployeeEditHistoryContent.Text = "Edit history will appear here when mutation history is connected."; EmployeeEditHistoryLayout.Controls.Add(EmployeeEditHistoryContent, 0, 1); EmployeeEditHistoryView.Controls.Add(EmployeeEditHistoryLayout); EmployeeDetailsLayout.Controls.Add(EmployeeEditHistoryView, 0, 1);
    }

    private void BuildEmployeeView()
    {
        EmployeesSplitView.Dock = DockStyle.Fill; EmployeesSplitView.Orientation = Orientation.Vertical; EmployeesSplitView.IsSplitterFixed = false; EmployeeListLayout.Dock = DockStyle.Fill; EmployeeListLayout.RowCount = 2; EmployeeListLayout.ColumnCount = 1; EmployeeListLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45)); EmployeeListLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); EmployeeSearchArea.Padding = new Padding(12, 8, 12, 8); EmployeeSearchInput.Dock = DockStyle.Fill; EmployeeSearchInput.PlaceholderText = "Search employees…"; EmployeeSearchInput.TextChanged += SearchEmployees; EmployeeSearchArea.Controls.Add(EmployeeSearchInput); EmployeeListLayout.Controls.Add(EmployeeSearchArea, 0, 0); ConfigureGrid(EmployeesGrid); AddTextColumn(EmployeesGrid, "Employee no.", "EmployeeNumber", 90); foreach (var column in new[] { ("Title", "Title"), ("First name", "FirstName"), ("Last name", "LastName"), ("Date of birth", "DateOfBirth"), ("Gender", "Gender"), ("Job title", "JobTitle"), ("Email", "EmailAddress"), ("Business phone", "BusinessNumber"), ("Mobile phone", "MobileNumber"), ("Department", "Department"), ("Office location", "OfficeLocation"), ("Employee type", "EmployeeType"), ("Status", "Status"), ("Created at", "CreatedAt") }) AddTextColumn(EmployeesGrid, column.Item1, column.Item2); EmployeesGrid.SelectionChanged += SelectEmployee; EmployeeListLayout.Controls.Add(EmployeesGrid, 0, 1); EmployeesSplitView.Panel1.Controls.Add(EmployeeListLayout);
        EmployeeDetailsLayout.Dock = DockStyle.Fill; EmployeeDetailsLayout.RowCount = 2; EmployeeDetailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); EmployeeDetailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); ConfigureButton(CreateEmployee, "New employee", CreateNewEmployee); ConfigureButton(EditEmployee, "Edit", EditEmployeeDetails); ConfigureButton(DeleteEmployee, "Delete", DeleteSelectedEmployee); ConfigureButton(ViewEmployeeHistory, "History", ShowEmployeeEditHistoryView); ConfigureButton(SaveEmployee, "Save", SaveEmployeeDetails); ConfigureButton(CancelEmployeeEdit, "Cancel", CancelEmployeeEditMode); SaveEmployee.Visible = false; CancelEmployeeEdit.Visible = false; EmployeeActions.Dock = DockStyle.Fill; EmployeeActions.ColumnCount = 2; EmployeeActions.RowCount = 1; EmployeeActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); EmployeeActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); EmployeePrimaryActions.Dock = DockStyle.Fill; EmployeePrimaryActions.WrapContents = false; EmployeePrimaryActions.Padding = new Padding(8, 8, 4, 4); EmployeePrimaryActions.Controls.AddRange(new Control[] { CreateEmployee, EditEmployee, DeleteEmployee }); EmployeeSecondaryActions.Dock = DockStyle.Fill; EmployeeSecondaryActions.FlowDirection = FlowDirection.RightToLeft; EmployeeSecondaryActions.WrapContents = false; EmployeeSecondaryActions.Padding = new Padding(4, 8, 8, 4); EmployeeSecondaryActions.Controls.Add(ViewEmployeeHistory); EmployeeActions.Controls.Add(EmployeePrimaryActions, 0, 0); EmployeeActions.Controls.Add(EmployeeSecondaryActions, 1, 0); EmployeeDetailsLayout.Controls.Add(EmployeeActions, 0, 0);
        EmployeeEditActions.Dock = DockStyle.Top; EmployeeEditActions.Height = 48; EmployeeEditActions.ColumnCount = 2; EmployeeEditActions.RowCount = 1; EmployeeEditActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); EmployeeEditActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); CancelEmployeeEdit.Anchor = AnchorStyles.Left; SaveEmployee.Anchor = AnchorStyles.Right; EmployeeEditActions.Controls.Add(CancelEmployeeEdit, 0, 0); EmployeeEditActions.Controls.Add(SaveEmployee, 1, 0);
        EmployeeDetailsScrollView.Dock = DockStyle.Fill; EmployeeDetailsScrollView.AutoScroll = true; EmployeeDetails.Text = "Employee details"; EmployeeDetails.Dock = DockStyle.Top; EmployeeDetails.AutoSize = true;
        EmployeeFirstNameInput.ReadOnly = true; EmployeeLastNameInput.ReadOnly = true;
        EmployeeDetailsFields.Dock = DockStyle.Fill; EmployeeDetailsFields.ColumnCount = 2; EmployeeDetailsFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38)); EmployeeDetailsFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        ConfigureEmployeeTypeSelection(); ConfigureEmployeeEndDateArea(); AddDetailRow(EmployeeDetailsFields, "Employee number", EmployeeNumberInput, 0); AddDetailRow(EmployeeDetailsFields, "Employee type", EmployeeTypeSelection, 1); AddDetailRow(EmployeeDetailsFields, "Active", EmployeeActiveInput, 2);
        AddDetailRow(EmployeeDetailsFields, "Title", EmployeeTitleInput, 3); AddDetailRow(EmployeeDetailsFields, "First name", EmployeeFirstNameInput, 4); AddDetailRow(EmployeeDetailsFields, "Last name", EmployeeLastNameInput, 5); AddDetailRow(EmployeeDetailsFields, "Date of birth", EmployeeDateOfBirthInput, 6); AddDetailRow(EmployeeDetailsFields, "Gender", EmployeeGenderInput, 7); AddDetailRow(EmployeeDetailsFields, "Job title", EmployeeJobTitleInput, 8); AddDetailRow(EmployeeDetailsFields, "Business phone", EmployeeBusinessPhoneInput, 9); AddDetailRow(EmployeeDetailsFields, "Mobile phone", EmployeeMobilePhoneInput, 10); AddDetailRow(EmployeeDetailsFields, "Email", EmployeeEmailInput, 11); AddDetailRow(EmployeeDetailsFields, "Department", EmployeeDepartmentInput, 12); AddDetailRow(EmployeeDetailsFields, "AHV number", EmployeeAhvNumberInput, 13); AddDetailRow(EmployeeDetailsFields, "Nationality", EmployeeNationalityInput, 14); AddDetailRow(EmployeeDetailsFields, "City", EmployeeCityInput, 15); AddDetailRow(EmployeeDetailsFields, "Address", EmployeeAddressInput, 16); AddDetailRow(EmployeeDetailsFields, "Postal code", EmployeePostalCodeInput, 17); AddDetailRow(EmployeeDetailsFields, "Employment start date", EmployeeStartDateInput, 18); AddDetailRow(EmployeeDetailsFields, "Employment end date", EmployeeEndDateArea, 19); AddDetailRow(EmployeeDetailsFields, "Employment percentage", EmployeeEmploymentPercentageInput, 20); AddDetailRow(EmployeeDetailsFields, "Office location", EmployeeOfficeLocationInput, 21); AddDetailRow(EmployeeDetailsFields, "Management level", EmployeeManagementLevelInput, 22); AddDetailRow(EmployeeDetailsFields, "Apprenticeship duration", ApprenticeshipDurationInput, 23); AddDetailRow(EmployeeDetailsFields, "Current apprenticeship year", CurrentApprenticeshipYearInput, 24);
        EmployeeDetailsFields.Dock = DockStyle.Fill;
        EmployeeDetailsFields.AutoSize = true;
        EmployeeDetailsTypeDefaults(); EmployeeDetails.Controls.Add(EmployeeDetailsFields); EmployeeDetailsScrollView.Controls.Add(EmployeeEditActions); EmployeeDetailsScrollView.Controls.Add(EmployeeDetails); EmployeeDetailsLayout.Controls.Add(EmployeeDetailsScrollView, 0, 1); BuildEmployeeEditHistory();
        EmployeesSplitView.Panel2.Controls.Add(EmployeeDetailsLayout);
        EmployeesTab.Controls.Add(EmployeesSplitView);
    }

    private void BuildLoginView() { LoginPreviewArea.Dock = DockStyle.Fill; LoginPreview = new TableLayoutPanel { Dock = DockStyle.None, Size = new Size(480, 260), ColumnCount = 1, RowCount = 5, Padding = new Padding(28), BackColor = SystemColors.Control }; LoginPreview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Absolute, 44)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); LoginPreview.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); LoginTitle.Text = "Sign in"; LoginTitle.Font = new Font(Font, FontStyle.Bold); LoginTitle.Font = new Font(LoginTitle.Font.FontFamily, 16, FontStyle.Bold); LoginTitle.Dock = DockStyle.Fill; LoginTitle.TextAlign = ContentAlignment.MiddleCenter; LoginUserNameInput.PlaceholderText = "Username"; LoginUserNameInput.Dock = DockStyle.Fill; LoginPasswordInput.PlaceholderText = "Password"; LoginPasswordInput.UseSystemPasswordChar = true; LoginPasswordInput.Dock = DockStyle.Fill; PreviewLogin.Text = "Preview login"; PreviewLogin.Anchor = AnchorStyles.None; PreviewLogin.AutoSize = true; PreviewLogin.Click += PreviewLoginMessage; LoginPreviewInfo.Text = "Authentication will be connected later."; LoginPreviewInfo.Dock = DockStyle.Fill; LoginPreviewInfo.TextAlign = ContentAlignment.MiddleCenter; LoginPreviewInfo.ForeColor = Color.DimGray; LoginPreview.Controls.Add(LoginTitle, 0, 0); LoginPreview.Controls.Add(LoginUserNameInput, 0, 1); LoginPreview.Controls.Add(LoginPasswordInput, 0, 2); LoginPreview.Controls.Add(PreviewLogin, 0, 3); LoginPreview.Controls.Add(LoginPreviewInfo, 0, 4); LoginPreviewArea.Controls.Add(LoginPreview); LoginPreviewArea.Resize += CenterLoginPreview; LoginPreviewTab.Controls.Add(LoginPreviewArea); }
    private void ConfigureButton(Button button, string text, EventHandler handler) { button.Text = text; button.AutoSize = true; button.Click += handler; }
    private void AddDetailRow(TableLayoutPanel layout, string labelText, Control input, int row) { layout.RowCount = Math.Max(layout.RowCount, row + 1); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38)); var label = new Label { Text = labelText, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(8, 10, 4, 4) }; input.Dock = DockStyle.Fill; input.Margin = new Padding(4, 5, 8, 4); layout.Controls.Add(label, 0, row); layout.Controls.Add(input, 1, row); }
    private void ConfigureReadOnlyCustomerFields() { foreach (var input in new Control[] { CustomerTitleInput, CustomerDateOfBirthInput, CustomerGenderInput, CustomerActiveInput }) input.Enabled = false; foreach (var input in new TextBox[] { CustomerFirstNameInput, CustomerLastNameInput, CustomerJobTitleInput, CustomerBusinessPhoneInput, CustomerMobilePhoneInput, CustomerEmailInput, CustomerCompanyInput }) input.ReadOnly = true; CustomerTitleInput.DropDownStyle = ComboBoxStyle.DropDownList; CustomerGenderInput.DropDownStyle = ComboBoxStyle.DropDownList; CustomerDateOfBirthInput.Format = DateTimePickerFormat.Custom; CustomerDateOfBirthInput.CustomFormat = "dd.MM.yyyy"; CustomerDateOfBirthInput.Value = new DateTime(2000, 1, 1); }
    private void ConfigureEmployeeTypeSelection() { EmployeeTypeSelection.AutoSize = true; EmployeeTypeSelection.WrapContents = false; EmployeeTypeEmployeeOption.Text = "Employee"; EmployeeTypeEmployeeOption.AutoSize = true; EmployeeTypeEmployeeOption.Checked = true; EmployeeTypeApprenticeOption.Text = "Apprentice"; EmployeeTypeApprenticeOption.AutoSize = true; EmployeeTypeEmployeeOption.CheckedChanged += EmployeeTypeChanged; EmployeeTypeApprenticeOption.CheckedChanged += EmployeeTypeChanged; EmployeeTypeSelection.Controls.AddRange(new Control[] { EmployeeTypeEmployeeOption, EmployeeTypeApprenticeOption }); }
    private void ConfigureEmployeeEndDateArea() { EmployeeEndDateArea.Dock = DockStyle.Fill; EmployeeEndDateArea.Padding = new Padding(0); EmployeeEndDateArea.ColumnCount = 2; EmployeeEndDateArea.RowCount = 1; EmployeeEndDateArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); EmployeeEndDateArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); EmployeeEndDateArea.Controls.Add(EmployeeEndDateInput, 0, 0); EmployeeEndDateArea.Controls.Add(EmployeeIndefiniteInput, 1, 0); EmployeeEndDateInput.Dock = DockStyle.Fill; EmployeeEndDateInput.Margin = new Padding(0); EmployeeIndefiniteInput.Text = "Indefinite"; EmployeeIndefiniteInput.AutoSize = true; EmployeeIndefiniteInput.Anchor = AnchorStyles.Left; EmployeeIndefiniteInput.Margin = new Padding(8, 5, 4, 4); EmployeeIndefiniteInput.CheckedChanged += EmployeeIndefiniteChanged; }
    private void EmployeeDetailsTypeDefaults() { EmployeeNumberInput.ReadOnly = true; EmployeeEmploymentPercentageInput.Minimum = 5; EmployeeEmploymentPercentageInput.Maximum = 100; EmployeeEmploymentPercentageInput.Value = 66; EmployeeDateOfBirthInput.Format = DateTimePickerFormat.Custom; EmployeeDateOfBirthInput.CustomFormat = "dd.MM.yyyy"; EmployeeStartDateInput.Format = DateTimePickerFormat.Custom; EmployeeStartDateInput.CustomFormat = "dd.MM.yyyy"; EmployeeEndDateInput.Format = DateTimePickerFormat.Custom; EmployeeEndDateInput.CustomFormat = " "; EmployeeTitleInput.DropDownStyle = ComboBoxStyle.DropDownList; EmployeeGenderInput.DropDownStyle = ComboBoxStyle.DropDownList; EmployeeOfficeLocationInput.DropDownStyle = ComboBoxStyle.DropDownList; EmployeeManagementLevelInput.DropDownStyle = ComboBoxStyle.DropDownList; ApprenticeshipDurationInput.Minimum = 1; ApprenticeshipDurationInput.Maximum = 4; CurrentApprenticeshipYearInput.Minimum = 1; CurrentApprenticeshipYearInput.Maximum = 4; ApprenticeshipDurationInput.ValueChanged += (_, _) => { CurrentApprenticeshipYearInput.Maximum = Math.Max(CurrentApprenticeshipYearInput.Minimum, ApprenticeshipDurationInput.Value); if (CurrentApprenticeshipYearInput.Value > CurrentApprenticeshipYearInput.Maximum) CurrentApprenticeshipYearInput.Value = CurrentApprenticeshipYearInput.Maximum; }; SetApprenticeFieldsVisible(false); }

    #endregion
}
