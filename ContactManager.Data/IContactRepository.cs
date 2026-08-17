namespace ContactManager.Data;

/// <summary>
/// Defines storage operations for all Contact Manager data.
/// </summary>
public interface IContactRepository
{
	/// <summary>
	/// Loads the persisted contact data.
	/// </summary>
	/// <returns>
	/// Returns the loaded contact data or a new instance of ContactData if there is an error when loading.
	/// </returns>
	public ContactData Load();

	/// <summary>
	/// Saves all contact data.
	/// </summary>
	/// <param name="data">The contact data to save.</param>
	public void Save(ContactData data);
}
