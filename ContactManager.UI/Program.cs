using ContactManager.Logic;

namespace ContactManager.UI;

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

		AuthenticationService authenticationService = new();
		using LoginForm loginForm = new(authenticationService);
		if (loginForm.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		Application.Run(new MainForm());
	}
}
