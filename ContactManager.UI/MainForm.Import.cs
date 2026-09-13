using ContactManager.Logic;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.UI;

public partial class MainForm
{
	/// <summary>Lets the user select, review, and import contacts from a CSV or vCard file.</summary>
	private void ImportContacts(object? sender, EventArgs e)
	{
		if (customerEditMode || employeeEditMode)
		{
			MessageBox.Show(
				this,
				"Finish or cancel the current edit before importing contacts.",
				"Import contacts",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);
			return;
		}

		using OpenFileDialog fileDialog = new()
		{
			Title = "Select contacts to import",
			Filter = "Supported contact files (*.csv;*.vcf;*.vcard)|*.csv;*.vcf;*.vcard|CSV files (*.csv)|*.csv|vCard files (*.vcf;*.vcard)|*.vcf;*.vcard|All files (*.*)|*.*",
			FilterIndex = 1,
			CheckFileExists = true,
			Multiselect = false,
			RestoreDirectory = true
		};

		if (fileDialog.ShowDialog(this) != DialogResult.OK)
		{
			return;
		}

		try
		{
			ContactImportResult result = contactImportService.ReadFile(fileDialog.FileName);
			IReadOnlyList<ContactImportIssue> duplicateWarnings = contactDuplicateDetector.FindPotentialDuplicates(
				result.Candidates,
				personManager!.GetAll());
			result = result with
			{
				Issues = result.Issues.Concat(duplicateWarnings).ToList().AsReadOnly()
			};
			using ImportPreviewForm preview = new(Path.GetFileName(fileDialog.FileName), result);
			if (preview.ShowDialog(this) != DialogResult.OK)
			{
				return;
			}

			IReadOnlyList<ContactImportCandidate> selectedCandidates = preview.SelectedCandidates;
			int importedCount = personManager.Import(
				selectedCandidates.Select(candidate => candidate.Contact));
			LoadInitialData(this, EventArgs.Empty);

			int skippedCount = result.Issues
				.Where(issue => issue.Severity == ContactImportIssueSeverity.Error)
				.Select(issue => issue.Source)
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.Count();
			string skippedText = skippedCount > 0
				? $"\n\n{skippedCount} invalid source {(skippedCount == 1 ? "entry was" : "entries were")} skipped."
				: string.Empty;
			int duplicateSkippedCount = result.Candidates.Count - selectedCandidates.Count;
			string duplicateSkippedText = duplicateSkippedCount > 0
				? $"\n{duplicateSkippedCount} possible {(duplicateSkippedCount == 1 ? "duplicate was" : "duplicates were")} not imported."
				: string.Empty;
			MessageBox.Show(
				this,
				$"{importedCount} {(importedCount == 1 ? "contact was" : "contacts were")} imported successfully.{skippedText}{duplicateSkippedText}",
				"Import contacts",
				MessageBoxButtons.OK,
				MessageBoxIcon.Information);
		}
		catch (UnauthorizedAccessException exception)
		{
			ShowImportError("The selected file could not be accessed.", exception);
		}
		catch (IOException exception)
		{
			ShowImportError("The selected file could not be read or the contacts could not be saved.", exception);
		}
		catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
		{
			ShowImportError("The contacts could not be imported.", exception);
		}
		catch (Exception exception)
		{
			ShowImportError("An unexpected error occurred while importing the contacts.", exception);
		}
	}

	/// <summary>Displays a recoverable contact-import error.</summary>
	private void ShowImportError(string message, Exception exception)
	{
		MessageBox.Show(
			this,
			$"{message}\n\n{exception.Message}",
			"Import contacts",
			MessageBoxButtons.OK,
			MessageBoxIcon.Error);
	}

	/// <summary>Provides keyboard access to global application actions.</summary>
	private void HandleKeyboardShortcuts(object? sender, KeyEventArgs e)
	{
		if (!e.Control || e.KeyCode != Keys.I)
		{
			return;
		}

		ImportContacts(this, EventArgs.Empty);
		e.SuppressKeyPress = true;
	}
}
