using ContactManager.Logic;
using ContactManager.Models;
using ContactManager.Models.Enums;

namespace ContactManager.UI;

public partial class MainForm
{
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

	/// <summary>Displays an error without allowing an expected operation failure to close the form.</summary>
	/// <param name="message">The user-friendly error message to display.</param>
	private void ShowErrorMessage(string message)
	{
		MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
	}
}
