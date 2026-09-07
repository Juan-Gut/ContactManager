using ContactManager.Logic;

namespace ContactManager.UI;

/// <summary>
/// Collects credentials before the Contact Manager main window is opened.
/// </summary>
public sealed partial class LoginForm : Form
{
	private readonly AuthenticationService _authenticationService;

	/// <summary>
	/// Initializes a new login form.
	/// </summary>
	/// <param name="authenticationService">The service used to validate credentials.</param>
	public LoginForm(AuthenticationService authenticationService)
	{
		_authenticationService = authenticationService
			?? throw new ArgumentNullException(nameof(authenticationService));
		InitializeComponent();
	}

	private void AttemptLogin(object? sender, EventArgs e)
	{
		LoginError.Text = string.Empty;

		if (string.IsNullOrWhiteSpace(UserNameInput.Text)
			|| string.IsNullOrEmpty(PasswordInput.Text))
		{
			LoginError.Text = "Please enter a username and password.";
			return;
		}

		if (!_authenticationService.Authenticate(UserNameInput.Text, PasswordInput.Text))
		{
			LoginError.Text = "The username or password is incorrect.";
			PasswordInput.Clear();
			PasswordInput.Focus();
			return;
		}

		DialogResult = DialogResult.OK;
		Close();
	}

	/// <summary>
	/// TO DELETE WHEN APP IS FINISHED: Bypasses authentication to speed up local development.
	/// </summary>
	/// <param name="sender">The control that raised the event.</param>
	/// <param name="e">The event data.</param>
	private void SkipLoginForDevelopment(object? sender, EventArgs e)
	{
		// TO DELETE WHEN APP IS FINISHED: Returning OK lets Program start the main form without authentication.
		DialogResult = DialogResult.OK;
		Close();
	}
}
