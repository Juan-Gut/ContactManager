namespace ContactManager.Models;

/// <summary>
/// Represents an audit entry for a successful data mutation.
/// </summary>
/// <remarks>
/// The entry intentionally stores only metadata. It does not contain a person's name,
/// changed field, old value, new value, or customer note text.
/// </remarks>
public sealed class MutationLogEntry
{
	/// <summary>
	/// Gets or sets the time at which the mutation was completed.
	/// </summary>
	public DateTimeOffset ChangedAt { get; init; } = DateTimeOffset.UtcNow;

	/// <summary>
	/// Gets or sets the stable identifier of the affected contact.
	/// </summary>
	public Guid ContactId { get; init; }

	/// <summary>
	/// Gets or sets the action that was completed.
	/// </summary>
	public string Action { get; init; } = string.Empty;
}

