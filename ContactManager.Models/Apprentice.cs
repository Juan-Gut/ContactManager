namespace ContactManager.Models;

/// <summary>
/// Represents an apprentice employed by the company.
/// </summary>
public sealed class Apprentice : Employee
{
	/// <summary>
	/// Gets or sets the total duration of the apprenticeship in years.
	/// </summary>
	public int ApprenticeshipYears { get; set; }

	/// <summary>
	/// Gets or sets the apprentice's current apprenticeship year.
	/// </summary>
	public int CurrentApprenticeshipYear { get; set; }
}
