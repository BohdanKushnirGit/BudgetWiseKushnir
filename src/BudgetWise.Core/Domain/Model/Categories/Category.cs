using BudgetWise.Core.Domain.SharedKernel;

namespace BudgetWise.Core.Domain.Model.Categories;

public class Category: AggregateRoot<Guid>
{
	internal Category(Guid id) : base(id)
	{ }
	
	public required Guid ProfileId { get; set; }
	public required string Name { get; set; }
	public decimal? PlannedAmount { get; set; }
}