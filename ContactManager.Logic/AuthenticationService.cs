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
	private const string DemoPasswordHash = "24B87E87AE1016A75248E2F6C2EB58EC923EF4D82A6FA3EA9406DDC5BAB71789";

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
