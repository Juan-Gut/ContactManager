using ContactManager.Logic;
using ContactManager.Models;

namespace ContactManager.UI;

/// <summary>
/// Displays parsed contacts and import issues before the user commits an import.
/// </summary>
public sealed class ImportPreviewForm : Form
{
	/// <summary>Initializes the preview for a parsed import file.</summary>
	/// <param name="fileName">The selected file's display name.</param>
	/// <param name="result">The parsed import result to present.</param>
	public ImportPreviewForm(string fileName, ContactImportResult result)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
		Result = result ?? throw new ArgumentNullException(nameof(result));

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
		previewTabs.TabPages.Add(CreateContactsTab(result));
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
		Button importButton = new()
		{
			Text = result.Candidates.Count == 1
				? "Import 1 contact"
				: $"Import {result.Candidates.Count} contacts",
			AutoSize = true,
			Enabled = result.Candidates.Count > 0,
			DialogResult = DialogResult.OK
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

		AcceptButton = importButton;
		CancelButton = cancelButton;
		Controls.Add(layout);
	}

	/// <summary>Gets the parsed import result represented by this preview.</summary>
	public ContactImportResult Result { get; }

	private static TabPage CreateContactsTab(ContactImportResult result)
	{
		TabPage tab = new($"Valid contacts ({result.Candidates.Count})");
		DataGridView grid = CreateGrid();
		grid.Columns.Add(CreateColumn("Source", "Source", 16));
		grid.Columns.Add(CreateColumn("Type", "Type", 15));
		grid.Columns.Add(CreateColumn("First name", "FirstName", 17));
		grid.Columns.Add(CreateColumn("Last name", "LastName", 17));
		grid.Columns.Add(CreateColumn("Date of birth", "DateOfBirth", 16));
		grid.Columns.Add(CreateColumn("Email", "EmailAddress", 24));
		grid.DataSource = result.Candidates.Select(candidate => new ContactPreviewRow
		{
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
			EmailAddress = candidate.Contact.EmailAddress
		}).ToList();
		tab.Controls.Add(grid);
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

	private static DataGridView CreateGrid()
	{
		return new DataGridView
		{
			Dock = DockStyle.Fill,
			ReadOnly = true,
			AllowUserToAddRows = false,
			AllowUserToDeleteRows = false,
			AllowUserToResizeRows = false,
			AutoGenerateColumns = false,
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
			SelectionMode = DataGridViewSelectionMode.FullRowSelect,
			MultiSelect = false,
			RowHeadersVisible = false
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
			SortMode = DataGridViewColumnSortMode.Automatic
		};
	}

	private static string CreateSummary(string fileName, ContactImportResult result)
	{
		int errorCount = result.Issues.Count(issue => issue.Severity == ContactImportIssueSeverity.Error);
		int warningCount = result.Issues.Count - errorCount;
		string validText = result.Candidates.Count == 1 ? "1 valid contact" : $"{result.Candidates.Count} valid contacts";
		string problemText = errorCount == 1 ? "1 error" : $"{errorCount} errors";
		string warningText = warningCount == 1 ? "1 warning" : $"{warningCount} warnings";
		return $"{fileName}\n{validText} ready to import; {problemText} and {warningText}. "
		       + "Contacts with errors are skipped. Review both tabs before continuing.";
	}

	private sealed class ContactPreviewRow
	{
		public string Source { get; init; } = string.Empty;
		public string Type { get; init; } = string.Empty;
		public string FirstName { get; init; } = string.Empty;
		public string LastName { get; init; } = string.Empty;
		public string DateOfBirth { get; init; } = string.Empty;
		public string EmailAddress { get; init; } = string.Empty;
	}

	private sealed class ImportIssueRow
	{
		public string Severity { get; init; } = string.Empty;
		public string Source { get; init; } = string.Empty;
		public string Message { get; init; } = string.Empty;
	}
}
