using BudgetWise.Core.Domain.Model.Shared.ValueObjects;

namespace BudgetWise.Core.Domain.Model.Profiles.ValueObjects;

/// <summary>
/// Settings of profile
/// </summary>
public struct ProfileSettings
{
	public required Currency Currency { get; set; }
}