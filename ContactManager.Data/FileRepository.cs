using System.Text.Json;

namespace ContactManager.Data;

/// <summary>
/// Stores and loads contact data from a JSON file in the local application data.
/// </summary>
public sealed class FileRepository : IContactRepository
{
	/// <summary>
	/// Path to directory in LocalAppData where the contact data file is stored.
	/// </summary>
	private static readonly string _localAppDataDir = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
		"ContactManager"
	);

	/// <summary>
	/// Path to file that stores contact data.
	/// </summary>
	private static readonly string _filePath = Path.Combine(_localAppDataDir, "contacts.json");

	/// <summary>
	/// Options for the JSONSerializer.
	/// </summary>
	private static readonly JsonSerializerOptions _options = new()
	{
		WriteIndented = true
	};

	/// <summary>
	/// Loads the contact data from the JSON file.
	/// </summary>
	/// <returns>
	/// Returns the loaded contact data or a new instance of ContactData if there is an error when loading.
	/// </returns>
	public ContactData Load()
	{
		if (!File.Exists(_filePath))
		{
			return new ContactData();
		}

		try
		{
			string jsonString = File.ReadAllText(_filePath);
			ContactData? deserializedData = JsonSerializer.Deserialize<ContactData>(jsonString);
			return deserializedData ?? new ContactData();
		}
		catch (JsonException jsonException)
		{
			File.Move(_filePath, $"{_filePath}.corrupt", true);
			//TODO: Show an error message on UI -> could not load corrupt JSON
			Console.WriteLine("Err: JSON is invalid. Unable to load data.");
			return new ContactData();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			throw;
		}
	}

	/// <summary>
	/// Saves the contact data to the JSON file.
	/// </summary>
	/// <remarks>
	/// Saves the data into a .tmp file first before moving it to the final location to avoid data corruption.
	/// </remarks>
	/// <param name="data">The contact data to save.</param>
	public void Save(ContactData data)
	{
		Directory.CreateDirectory(_localAppDataDir);
		string serializedData = JsonSerializer.Serialize(data, _options);
		string tempFilePath = Path.Combine(
			_localAppDataDir,
			$"contacts.{Guid.NewGuid():N}.tmp"
		);

		try
		{
			File.WriteAllText(tempFilePath, serializedData);
			File.Move(tempFilePath, _filePath, true);
		}
		catch (FileNotFoundException noFileEx)
		{
			Console.WriteLine("Err: Source file was not found.");
			Console.WriteLine(noFileEx.Message);
			throw;
		}
		catch (DirectoryNotFoundException noDirectoryEx)
		{
			Console.WriteLine("Err: Directory does not exist.");
			Console.WriteLine(noDirectoryEx.Message);
			throw;
		}
		catch (Exception e)
		{
			Console.WriteLine($"Err: {e.Message}");
			throw;
		}
		finally
		{
			if (File.Exists(tempFilePath))
			{
				File.Delete(tempFilePath);
			}
		}
	}
}
