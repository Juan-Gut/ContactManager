namespace ContactManager.Models;

/// <summary>
/// Represents a timestamped note about contact with a customer.
/// </summary>
public sealed class CustomerContactEntry
{
	/// <summary>
	///     Gets or sets the unique identifier of the contact entry.
	/// </summary>
	public Guid Id { get; init; } = Guid.NewGuid();

	/// <summary>
	/// Gets or sets when the entry was created.
	/// </summary>
	public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;

	/// <summary>
	/// Gets or sets the contact note.
	/// </summary>
	public string Note { get; set; } = string.Empty;
}
