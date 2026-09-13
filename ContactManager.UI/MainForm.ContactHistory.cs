using ContactManager.Logic;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.UI;

public partial class MainForm
{
	/// <summary>Displays the in-place customer notes view.</summary>
	private void ShowCustomerNotesView(object? sender, EventArgs e)
	{
		Customer? selectedCustomer = GetSelectedCustomer();
		if (selectedCustomer is null)
		{
			return;
		}

		notesCustomerId = selectedCustomer.Id;
		RefreshCustomerNotes();
		ShowCustomerNotes(true);
	}

	/// <summary>Returns from customer notes to the detail view.</summary>
	private void HideCustomerNotesView(object? sender, EventArgs e)
	{
		CancelNewCustomerNote(sender, e);
		notesCustomerId = null;
		ShowCustomerNotes(false);
	}

	/// <summary>Displays the selected customer's edit history view.</summary>
	private void ShowCustomerEditHistoryView(object? sender, EventArgs e)
	{
		RefreshCustomerEditHistory();
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
		RefreshEmployeeEditHistory();
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

	/// <summary>Loads the selected customer's metadata-only mutation history.</summary>
	private void RefreshCustomerEditHistory()
	{
		Customer? selectedCustomer = GetSelectedCustomer();
		IReadOnlyList<MutationLogEntry> history = selectedCustomer is null
			? []
			: personManager!.GetMutationHistory(selectedCustomer.Id);
		CustomerEditHistoryGrid.DataSource = history
			.Select(CreateMutationHistoryRow)
			.ToList();
	}

	/// <summary>Loads the selected employee's metadata-only mutation history.</summary>
	private void RefreshEmployeeEditHistory()
	{
		Employee? selectedEmployee = GetSelectedEmployee();
		IReadOnlyList<MutationLogEntry> history = selectedEmployee is null
			? []
			: personManager!.GetMutationHistory(selectedEmployee.Id);
		EmployeeEditHistoryGrid.DataSource = history
			.Select(CreateMutationHistoryRow)
			.ToList();
	}

	/// <summary>Creates a display row for a mutation without exposing contact values.</summary>
	/// <param name="mutation">The metadata-only mutation entry.</param>
	/// <returns>A history row containing only its timestamp and action.</returns>
	private static MutationHistoryRow CreateMutationHistoryRow(MutationLogEntry mutation)
	{
		return new MutationHistoryRow
		{
			ChangedAt = mutation.ChangedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"),
			Action = mutation.Action
		};
	}

	/// <summary>Shows or hides customer notes without hiding the customer detail inputs.</summary>
	private void ShowCustomerNotes(bool visible)
	{
		CustomerDetailsScrollView.Visible = !visible;
		CustomerNotesView.Visible = visible;
		CustomerEditHistoryView.Visible = false;
		if (!visible)
		{
			notesCustomerId = null;
		}

		if (visible)
		{
			CenterCustomerNotesSplitView();
		}
	}

	/// <summary>Enters new-note mode and gives the note editor balanced space.</summary>
	private void AddNewCustomerNote(object? sender, EventArgs e)
	{
		if (GetSelectedCustomer() is null)
		{
			return;
		}

		NewCustomerNoteArea.Visible = true;
		SaveCustomerNote.Visible = true;
		CancelCustomerNote.Visible = true;
		AddCustomerNote.Visible = false;
		CustomerNotesLayout.RowStyles[1] = new RowStyle(SizeType.Percent, 30);
		CustomerNotesLayout.RowStyles[2] = new RowStyle(SizeType.Percent, 70);
		NewCustomerNoteInput.Focus();
	}

	/// <summary>Returns from new-note state without persistence.</summary>
	private void CancelNewCustomerNote(object? sender, EventArgs e)
	{
		NewCustomerNoteArea.Visible = false;
		SaveCustomerNote.Visible = false;
		CancelCustomerNote.Visible = false;
		AddCustomerNote.Visible = true;
		NewCustomerNoteInput.Clear();
		CustomerNotesLayout.RowStyles[1] = new RowStyle(SizeType.Absolute, 0);
		CustomerNotesLayout.RowStyles[2] = new RowStyle(SizeType.Percent, 100);
	}

	/// <summary>Validates and persists a new note for the selected customer.</summary>
	private void SaveNewCustomerNote(object? sender, EventArgs e)
	{
		Customer? selectedCustomer = GetSelectedCustomer();
		if (selectedCustomer is null)
		{
			CancelNewCustomerNote(sender, e);
			return;
		}

		try
		{
			if (!personManager!.AddCustomerContact(selectedCustomer.Id, NewCustomerNoteInput.Text))
			{
				ShowErrorMessage("The selected customer could not be found.");
				return;
			}

			CancelNewCustomerNote(sender, e);
			ReloadCustomers(selectedCustomer.Id, preserveCurrentView: true);
			RefreshCustomerNotes();
		}
		catch (ArgumentException exception)
		{
			ShowErrorMessage("The customer note could not be saved. Please correct the following:\n\n" + exception.Message);
		}
		catch (Exception exception)
		{
			ShowErrorMessage("The customer note could not be saved.\n\n" + exception.Message);
		}
	}

	/// <summary>Displays the complete text of the selected customer note.</summary>
	private void SelectCustomerNote(object? sender, EventArgs e)
	{
		CustomerNoteContent.Text = CustomerContactEntriesGrid.SelectedRows.Count == 1
			&& CustomerContactEntriesGrid.SelectedRows[0].DataBoundItem is CustomerContactNoteRow selectedNote
			? selectedNote.Note
			: string.Empty;
	}

	/// <summary>Loads the selected customer's notes into the note-history list.</summary>
	private void RefreshCustomerNotes()
	{
		Customer? selectedCustomer = GetSelectedCustomer();
		if (selectedCustomer is null)
		{
			CustomerContactEntriesGrid.DataSource = null;
			CustomerNoteContent.Clear();
			return;
		}

		CustomerContactEntriesGrid.DataSource = (selectedCustomer.ContactHistory ?? [])
			.OrderByDescending(entry => entry.CreatedAt)
			.Select(entry => new CustomerContactNoteRow
			{
				Id = entry.Id,
				CreatedAt = entry.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
				Preview = CreateNotePreview(entry.Note),
				Note = entry.Note
			})
			.ToList();

		if (CustomerContactEntriesGrid.Rows.Count > 0)
		{
			CustomerContactEntriesGrid.Rows[0].Selected = true;
			CustomerContactEntriesGrid.CurrentCell = CustomerContactEntriesGrid.Rows[0].Cells[0];
			SelectCustomerNote(CustomerContactEntriesGrid, EventArgs.Empty);
		}
		else
		{
			CustomerNoteContent.Clear();
		}
	}

	/// <summary>Creates a concise single-line preview for the note-history list.</summary>
	/// <param name="note">The complete note text.</param>
	/// <returns>A trimmed preview suitable for a grid cell.</returns>
	private static string CreateNotePreview(string note)
	{
		string preview = string.Join(' ', note.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
		return preview.Length <= 80 ? preview : preview[..77] + "...";
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
}
