using System.Security.Cryptography;
using System.Text;

namespace ContactManager.Logic;

/// <summary>
/// Authenticates users of the Contact Manager demo application.
/// </summary>
public sealed class AuthenticationService
{
	private const int PasswordHashLength = 32;
	private const int PasswordHashIterations = 100_000;
	private const string DemoUserName = "admin";
	private const string DemoPasswordSalt = "ContactManagerDemoLogin";
	private const string DemoPasswordHash = "78BB531B6019FD1653872E78E248D477B26FD92D72BAB2B349DB887B009B8906";

	/// <summary>
	/// Checks whether the supplied credentials belong to the demo user.
	/// </summary>
	/// <param name="userName">The user name entered in the login form.</param>
	/// <param name="password">The password entered in the login form.</param>
	/// <returns><see langword="true"/> when the credentials are valid; otherwise, <see langword="false"/>.</returns>
	public bool Authenticate(string? userName, string? password)
	{
		if (!string.Equals(userName?.Trim(), DemoUserName, StringComparison.OrdinalIgnoreCase)
			|| string.IsNullOrEmpty(password))
		{
			return false;
		}

		byte[] passwordHash = Rfc2898DeriveBytes.Pbkdf2(
			password,
			Encoding.UTF8.GetBytes(DemoPasswordSalt),
			PasswordHashIterations,
			HashAlgorithmName.SHA256,
			PasswordHashLength);

		return CryptographicOperations.FixedTimeEquals(
			passwordHash,
			Convert.FromHexString(DemoPasswordHash));
	}
}
