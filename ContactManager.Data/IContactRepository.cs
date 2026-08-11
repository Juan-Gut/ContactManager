namespace ContactManager.Data;

/// <summary>
/// Defines storage operations for all Contact Manager data.
/// </summary>
public interface IContactRepository
{
	/// <summary>
	/// Loads the persisted contact data, or returns empty data when no storage file exists.
	/// </summary>
	/// <returns>The loaded contact data.</returns>
	public ContactData Load();

	/// <summary>
	/// Saves all contact data.
	/// </summary>
	/// <param name="data">The contact data to save.</param>
	public void Save(ContactData data);
}
