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

	/// <summary>Reads and validates contact files before they are persisted.</summary>
	private readonly ContactImportService contactImportService;

	/// <summary>Detects contacts that may already exist before an import is confirmed.</summary>
	private readonly ContactDuplicateDetector contactDuplicateDetector;

	/// <summary>Indicates whether the customer editor is currently in edit mode.</summary>
	private bool customerEditMode;

	/// <summary>Indicates whether a new customer is being created.</summary>
	private bool creatingCustomer;

	/// <summary>Indicates whether the employee editor is currently in edit mode.</summary>
	private bool employeeEditMode;

	/// <summary>Indicates whether a new employee is being created.</summary>
	private bool creatingEmployee;

	/// <summary>Stores the customer whose contact notes are currently displayed.</summary>
	private Guid? notesCustomerId;

	/// <summary>Indicates whether customer selection events should leave the current view unchanged.</summary>
	private bool suppressCustomerSelectionReset;

	/// <summary>
	/// Initializes a new instance of the form for the visual designer.
	/// </summary>
	public MainForm()
	{
		InitializeComponent();
		contactImportService = new ContactImportService();
		contactDuplicateDetector = new ContactDuplicateDetector();
		KeyPreview = true;
		KeyDown += HandleKeyboardShortcuts;
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
		MainTabs.SelectedIndexChanged += RefreshDashboardOnTabSelection;
	}
}
