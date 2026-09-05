using ContactManager.Data;
using ContactManager.Logic;
using System.Text.Json;

namespace ContactManager.UI;

/// <summary>
/// Provides the application entry point and startup error handling.
/// </summary>
internal static class Program
{
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	[STAThread]
	private static void Main()
	{
		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();

		try
		{
						AuthenticationService authenticationService = new();
						using LoginForm loginForm = new(authenticationService);
						if (loginForm.ShowDialog() != DialogResult.OK)
						{
								return;
						}

			IContactRepository repository = new FileRepository();
			PersonManager personManager = new(repository);
			Application.Run(new MainForm(personManager));
		}
		catch (JsonException exception)
		{
			ShowStartupError(exception);
		}
		catch (IOException exception)
		{
			ShowStartupError(exception);
		}
		catch (InvalidOperationException exception)
		{
			ShowStartupError(exception);
		}
	}

	/// <summary>
	/// Displays an error that prevented the application from starting.
	/// </summary>
	/// <param name="exception">The startup exception to describe.</param>
	private static void ShowStartupError(Exception exception)
	{
		MessageBox.Show(
			$"The application could not be started.\n\n{exception.Message}",
			"Contact Manager",
			MessageBoxButtons.OK,
			MessageBoxIcon.Error);
	}
}
