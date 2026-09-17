using System.Text;

namespace ContactManager.Logic;

/// <summary>
/// Reads supported contact files and delegates their content to the matching format parser.
/// </summary>
public sealed class ContactImportService
{
	private const long MaximumFileSize = 5 * 1024 * 1024;

	private readonly CsvContactImportParser _csvParser;
	private readonly VCardContactImportParser _vCardParser;

	/// <summary>Initializes a contact import service using the application validation rules.</summary>
	public ContactImportService()
		: this(new ValidationService())
	{
	}

	/// <summary>Initializes a contact import service with an explicit validation service.</summary>
	/// <param name="validationService">The service used to validate parsed contacts.</param>
	public ContactImportService(ValidationService validationService)
	{
		ContactImportCandidateFactory candidateFactory = new(
			validationService ?? throw new ArgumentNullException(nameof(validationService)));
		_csvParser = new CsvContactImportParser(candidateFactory);
		_vCardParser = new VCardContactImportParser(candidateFactory);
	}

	/// <summary>
	/// Reads and validates a supported contact import file.
	/// </summary>
	/// <param name="filePath">The CSV or vCard file to read.</param>
	/// <returns>A preview containing valid contacts and all detected issues.</returns>
	/// <exception cref="IOException">Thrown when the file cannot be read.</exception>
	/// <exception cref="UnauthorizedAccessException">Thrown when the file cannot be accessed.</exception>
	public ContactImportResult ReadFile(string filePath)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

		FileInfo file = new(filePath);
		if (!file.Exists)
		{
			throw new FileNotFoundException("The selected import file no longer exists.", filePath);
		}

		if (file.Length > MaximumFileSize)
		{
			return ContactImportResultFactory.CreateFailure(
				"File",
				"The selected file is larger than the supported limit of 5 MB.");
		}

		string content = File.ReadAllText(file.FullName, Encoding.UTF8);
		return file.Extension.ToLowerInvariant() switch
		{
			".csv" => ReadCsv(content),
			".vcf" or ".vcard" => ReadVCard(content),
			_ => ContactImportResultFactory.CreateFailure(
				"File",
				"Only CSV (.csv) and vCard (.vcf, .vcard) files are supported.")
		};
	}

	/// <summary>Parses CSV text using comma or semicolon delimiters.</summary>
	/// <param name="content">The complete CSV content.</param>
	/// <returns>The import preview.</returns>
	public ContactImportResult ReadCsv(string content)
	{
		return _csvParser.Parse(content);
	}

	/// <summary>Parses vCard 3.0 or 4.0 text.</summary>
	/// <param name="content">The complete vCard content.</param>
	/// <returns>The import preview.</returns>
	public ContactImportResult ReadVCard(string content)
	{
		return _vCardParser.Parse(content);
	}
}
