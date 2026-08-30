using ContactManager.Data;
using ContactManager.Models;

namespace ContactManager.Logic;

/// <summary>
/// Assigns unique, increasing employee numbers.
/// </summary>
public sealed class EmployeeNrGenerator
{
	/// <summary>Defines the first valid employee number.</summary>
	private const int FirstEmployeeNumber = 1000;

	/// <summary>
	/// Assigns the next available employee number and advances the persisted counter.
	/// </summary>
	/// <param name="employee">The new employee receiving the number.</param>
	/// <param name="data">The current persisted contact data.</param>
	/// <returns>The assigned employee number.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when no further employee number can be assigned.
	/// </exception>
	public int AssignNext(Employee employee, ContactData data)
	{
		ArgumentNullException.ThrowIfNull(employee);
		ArgumentNullException.ThrowIfNull(data);

		var assignedNumbers = data.Employees
			.Select(storedEmployee => storedEmployee.EmployeeNumber)
			.Concat(data.Apprentices.Select(apprentice => apprentice.EmployeeNumber))
			.Where(employeeNumber => employeeNumber >= FirstEmployeeNumber)
			.ToHashSet();

		int nextEmployeeNumber = Math.Max(data.NextEmployeeNumber, FirstEmployeeNumber);
		while (assignedNumbers.Contains(nextEmployeeNumber))
		{
			if (nextEmployeeNumber == int.MaxValue)
			{
				throw new InvalidOperationException("No further employee number can be assigned.");
			}

			nextEmployeeNumber++;
		}

		if (nextEmployeeNumber == int.MaxValue)
		{
			throw new InvalidOperationException("No further employee number can be assigned.");
		}

		employee.EmployeeNumber = nextEmployeeNumber;
		data.NextEmployeeNumber = nextEmployeeNumber + 1;
		return nextEmployeeNumber;
	}
}
