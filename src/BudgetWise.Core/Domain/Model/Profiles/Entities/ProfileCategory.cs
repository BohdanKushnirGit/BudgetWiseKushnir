using BudgetWise.Core.Domain.SharedKernel;

namespace BudgetWise.Core.Domain.Model.Profiles.Entities;

public class ProfileCategory(Guid categoryId) : Entity<Guid>(categoryId)
{
	public required string Name { get; set; }
	public required decimal ActualAmount { get; set; }
	public decimal? PlannedAmount { get; set; }
}