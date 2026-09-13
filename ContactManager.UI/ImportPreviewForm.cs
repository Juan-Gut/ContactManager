using ContactManager.Logic;
using ContactManager.Models;

namespace ContactManager.UI;

/// <summary>
/// Displays parsed contacts and import issues before the user commits an import.
/// </summary>
public sealed class ImportPreviewForm : Form
{
	private readonly List<ContactPreviewRow> contactRows;
	private readonly DataGridView contactsGrid;
	private readonly Button importButton;

	/// <summary>Initializes the preview for a parsed import file.</summary>
	/// <param name="fileName">The selected file's display name.</param>
	/// <param name="result">The parsed import result to present.</param>
	public ImportPreviewForm(string fileName, ContactImportResult result)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
		Result = result ?? throw new ArgumentNullException(nameof(result));
		contactRows = CreateContactRows(result);
		contactsGrid = CreateGrid(allowEdits: true);
		importButton = new Button { AutoSize = true };

		Text = "Preview contact import";
		StartPosition = FormStartPosition.CenterParent;
		MinimumSize = new Size(780, 520);
		ClientSize = new Size(980, 640);
		ShowInTaskbar = false;

		TableLayoutPanel layout = new()
		{
			Dock = DockStyle.Fill,
			Padding = new Padding(12),
			ColumnCount = 1,
			RowCount = 3
		};
		layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
		layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
		layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));

		Label summary = new()
		{
			Dock = DockStyle.Fill,
			AutoEllipsis = true,
			Text = CreateSummary(fileName, result),
			Padding = new Padding(4),
			TextAlign = ContentAlignment.MiddleLeft
		};
		layout.Controls.Add(summary, 0, 0);

		TabControl previewTabs = new() { Dock = DockStyle.Fill };
		previewTabs.TabPages.Add(CreateContactsTab());
		previewTabs.TabPages.Add(CreateIssuesTab(result));
		if (result.Candidates.Count == 0 && result.Issues.Count > 0)
		{
			previewTabs.SelectedIndex = 1;
		}
		layout.Controls.Add(previewTabs, 0, 1);

		FlowLayoutPanel actions = new()
		{
			Dock = DockStyle.Fill,
			FlowDirection = FlowDirection.RightToLeft,
			WrapContents = false,
			Padding = new Padding(4, 10, 4, 0)
		};
		Button cancelButton = new()
		{
			Text = "Cancel",
			AutoSize = true,
			DialogResult = DialogResult.Cancel
		};
		actions.Controls.Add(importButton);
		actions.Controls.Add(cancelButton);
		layout.Controls.Add(actions, 0, 2);
		importButton.Click += ConfirmImport;
		UpdateImportButton();

		AcceptButton = importButton;
		CancelButton = cancelButton;
		Controls.Add(layout);
	}

	/// <summary>Gets the parsed import result represented by this preview.</summary>
	public ContactImportResult Result { get; }

	/// <summary>Gets the contacts selected by the user when the preview is confirmed.</summary>
	public IReadOnlyList<ContactImportCandidate> SelectedCandidates => contactRows
		.Where(row => row.Import)
		.Select(row => row.Candidate)
		.ToList()
		.AsReadOnly();

	private TabPage CreateContactsTab()
	{
		TabPage tab = new($"Valid contacts ({contactRows.Count})");
		contactsGrid.Columns.Add(new DataGridViewCheckBoxColumn
		{
			HeaderText = "Import",
			DataPropertyName = "Import",
			Name = "Import",
			FillWeight = 8,
			ReadOnly = false,
			SortMode = DataGridViewColumnSortMode.Automatic
		});
		contactsGrid.Columns.Add(CreateColumn("Source", "Source", 14));
		contactsGrid.Columns.Add(CreateColumn("Type", "Type", 13));
		contactsGrid.Columns.Add(CreateColumn("First name", "FirstName", 15));
		contactsGrid.Columns.Add(CreateColumn("Last name", "LastName", 15));
		contactsGrid.Columns.Add(CreateColumn("Date of birth", "DateOfBirth", 14));
		contactsGrid.Columns.Add(CreateColumn("Email", "EmailAddress", 21));
		contactsGrid.Columns.Add(CreateColumn("Status", "Status", 20));
		contactsGrid.DataSource = contactRows;
		contactsGrid.CurrentCellDirtyStateChanged += (_, _) =>
		{
			if (contactsGrid.IsCurrentCellDirty)
			{
				contactsGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
			}
		};
		contactsGrid.CellValueChanged += (_, eventArgs) =>
		{
			if (eventArgs.RowIndex >= 0 && eventArgs.ColumnIndex == 0)
			{
				UpdateImportButton();
			}
		};
		tab.Controls.Add(contactsGrid);
		return tab;
	}

	private static TabPage CreateIssuesTab(ContactImportResult result)
	{
		TabPage tab = new($"Problems ({result.Issues.Count})");
		DataGridView grid = CreateGrid();
		grid.Columns.Add(CreateColumn("Severity", "Severity", 15));
		grid.Columns.Add(CreateColumn("Source", "Source", 18));
		grid.Columns.Add(CreateColumn("Description", "Message", 67));
		grid.DataSource = result.Issues.Select(issue => new ImportIssueRow
		{
			Severity = issue.Severity.ToString(),
			Source = issue.Source,
			Message = issue.Message
		}).ToList();
		tab.Controls.Add(grid);
		return tab;
	}

	private static DataGridView CreateGrid(bool allowEdits = false)
	{
		return new DataGridView
		{
			Dock = DockStyle.Fill,
			ReadOnly = !allowEdits,
			AllowUserToAddRows = false,
			AllowUserToDeleteRows = false,
			AllowUserToResizeRows = false,
			AutoGenerateColumns = false,
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
			SelectionMode = DataGridViewSelectionMode.FullRowSelect,
			MultiSelect = false,
			RowHeadersVisible = false,
			EditMode = allowEdits ? DataGridViewEditMode.EditOnEnter : DataGridViewEditMode.EditProgrammatically
		};
	}

	private static DataGridViewTextBoxColumn CreateColumn(string header, string property, float fillWeight)
	{
		return new DataGridViewTextBoxColumn
		{
			HeaderText = header,
			DataPropertyName = property,
			Name = property,
			FillWeight = fillWeight,
			ReadOnly = true,
			SortMode = DataGridViewColumnSortMode.Automatic
		};
	}

	private void ConfirmImport(object? sender, EventArgs e)
	{
		contactsGrid.EndEdit();
		if (contactRows.All(row => !row.Import))
		{
			MessageBox.Show(
				this,
				"Select at least one contact to import.",
				Text,
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);
			return;
		}

		DialogResult = DialogResult.OK;
		Close();
	}

	private void UpdateImportButton()
	{
		int selectedCount = contactRows.Count(row => row.Import);
		importButton.Text = selectedCount == 1 ? "Import 1 selected contact" : $"Import {selectedCount} selected contacts";
		importButton.Enabled = selectedCount > 0;
	}

	private static List<ContactPreviewRow> CreateContactRows(ContactImportResult result)
	{
		HashSet<string> possibleDuplicateSources = result.Issues
			.Where(issue => issue.Kind == ContactImportIssueKind.PotentialDuplicate)
			.Select(issue => issue.Source)
			.ToHashSet(StringComparer.OrdinalIgnoreCase);

		return result.Candidates.Select(candidate =>
		{
			bool possibleDuplicate = possibleDuplicateSources.Contains(candidate.Source);
			return new ContactPreviewRow
			{
				Candidate = candidate,
				Import = !possibleDuplicate,
				Source = candidate.Source,
				Type = candidate.Contact switch
				{
					Apprentice => "Apprentice",
					Employee => "Employee",
					Customer => "Customer",
					_ => candidate.Contact.GetType().Name
				},
				FirstName = candidate.Contact.FirstName,
				LastName = candidate.Contact.LastName,
				DateOfBirth = candidate.Contact.DateOfBirth.ToString("dd.MM.yyyy"),
				EmailAddress = candidate.Contact.EmailAddress,
				Status = possibleDuplicate ? "Possible duplicate" : "Ready"
			};
		}).ToList();
	}

	private static string CreateSummary(string fileName, ContactImportResult result)
	{
		int errorCount = result.Issues.Count(issue => issue.Severity == ContactImportIssueSeverity.Error);
		int warningCount = result.Issues.Count - errorCount;
		int duplicateCount = result.Issues
			.Where(issue => issue.Kind == ContactImportIssueKind.PotentialDuplicate)
			.Select(issue => issue.Source)
			.Distinct(StringComparer.OrdinalIgnoreCase)
			.Count();
		string validText = result.Candidates.Count == 1 ? "1 valid contact" : $"{result.Candidates.Count} valid contacts";
		string problemText = errorCount == 1 ? "1 error" : $"{errorCount} errors";
		string warningText = warningCount == 1 ? "1 warning" : $"{warningCount} warnings";
		string duplicateText = duplicateCount > 0
			? $" {duplicateCount} possible {(duplicateCount == 1 ? "duplicate is" : "duplicates are")} not selected by default."
			: string.Empty;
		return $"{fileName}\n{validText} ready to import; {problemText} and {warningText}. "
		       + $"Contacts with errors are skipped.{duplicateText} Review both tabs before continuing.";
	}

	private sealed class ContactPreviewRow
	{
		public ContactImportCandidate Candidate { get; init; } = null!;
		public bool Import { get; set; }
		public string Source { get; init; } = string.Empty;
		public string Type { get; init; } = string.Empty;
		public string FirstName { get; init; } = string.Empty;
		public string LastName { get; init; } = string.Empty;
		public string DateOfBirth { get; init; } = string.Empty;
		public string EmailAddress { get; init; } = string.Empty;
		public string Status { get; init; } = string.Empty;
	}

	private sealed class ImportIssueRow
	{
		public string Severity { get; init; } = string.Empty;
		public string Source { get; init; } = string.Empty;
		public string Message { get; init; } = string.Empty;
	}
}
