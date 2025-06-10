using BudgetWise.Core.Domain.Model.Shared.ValueObjects;
using BudgetWise.Core.Domain.Model.Transactions.ValueObjects;

namespace BudgetWise.Core.Specifications;
/// <summary>
/// Представляє специфікацію для фільтрації
/// транзакцій на основі різних критеріїв
/// </summary>
public record struct TransactionsSpecification
{
    public Guid? ProfileId { get; init; }
    public Guid? CategoryId { get; init; }
    public TransactionType? TransactionType { get; init; }
    public SpendingType? SpendingType { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public TransactionDestination? Destination { get; init; }
    public bool? IsMultiCurrency { get; init; }
    public string? Description { get; init; }
    public string? CurrencyCode { get; init; }
    public bool? IsGreaterThanAmount { get; init; }
    public decimal? Amount { get; init; }
}