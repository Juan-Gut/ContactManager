using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Indicates how an import issue affects the corresponding contact.
/// </summary>
public enum ContactImportIssueSeverity
{
	/// <summary>The issue is informational and the contact can still be imported.</summary>
	Warning,

	/// <summary>The issue prevents the corresponding contact from being imported.</summary>
	Error
}

/// <summary>
/// Describes a problem found while reading a contact import file.
/// </summary>
/// <param name="Source">The CSV row or vCard number in which the issue occurred.</param>
/// <param name="Message">A user-readable explanation of the problem.</param>
/// <param name="Severity">The effect the problem has on the import.</param>
public sealed record ContactImportIssue(
	string Source,
	string Message,
	ContactImportIssueSeverity Severity = ContactImportIssueSeverity.Error);

/// <summary>
/// Associates a valid imported contact with its location in the source file.
/// </summary>
/// <param name="Source">The CSV row or vCard number from which the contact was read.</param>
/// <param name="Contact">The validated contact.</param>
public sealed record ContactImportCandidate(string Source, Person Contact);

/// <summary>
/// Contains the valid contacts and diagnostics produced when an import file is parsed.
/// </summary>
/// <param name="Candidates">Contacts that are ready to be imported.</param>
/// <param name="Issues">Warnings and errors encountered during parsing.</param>
public sealed record ContactImportResult(
	IReadOnlyList<ContactImportCandidate> Candidates,
	IReadOnlyList<ContactImportIssue> Issues);
