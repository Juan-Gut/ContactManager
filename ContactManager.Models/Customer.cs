namespace ContactManager.Models;

/// <summary>
/// Represents a customer and the history of contact with that customer.
/// </summary>
public sealed class Customer : Person
{
	/// <summary>
	/// Gets or sets the customer's company.
	/// </summary>
	public string Company { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the customer's contact-history entries.
	/// </summary>
	public List<CustomerContactEntry> ContactHistory { get; set; } = [];
}
