using BudgetWise.Core.Domain.SharedKernel;

namespace BudgetWise.Core.Domain.Model.Transactions.Entities;

public class TransactionCategory(Guid categoryId) : Entity<Guid>(categoryId)
{
	public required string Name { get; set; }
}