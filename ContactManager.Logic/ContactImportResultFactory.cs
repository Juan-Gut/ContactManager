namespace ContactManager.Logic;

/// <summary>
/// Creates import results for conditions that prevent any contact from being parsed.
/// </summary>
internal static class ContactImportResultFactory
{
	/// <summary>Creates a result containing one file-level error and no candidates.</summary>
	/// <param name="source">The source associated with the error.</param>
	/// <param name="message">The user-readable error message.</param>
	/// <returns>An import result representing the failure.</returns>
	internal static ContactImportResult CreateFailure(string source, string message)
	{
		return new ContactImportResult(
			Array.Empty<ContactImportCandidate>(),
			new[] { new ContactImportIssue(source, message) });
	}
}
