using System.Collections.ObjectModel;
using BudgetWise.Core.Domain.Model.Shared.ValueObjects;
using BudgetWise.Core.Persistence;
using BudgetWise.Core.Specifications;
using BudgetWise.Mobile.Abstractions;
using BudgetWise.Mobile.Models.Transactions;

namespace BudgetWise.Mobile.ViewModels.Transactions;

public class FilteredTransactionsPageViewModel : BaseNotifyObject
{
    private readonly ITransactionRepository _transactionRepository;

    public FilteredTransactionsPageViewModel(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public readonly ObservableCollection<TransactionModel> Transactions = [];

    public async Task Initialize(Guid profileId, Guid? categoryId, SpendingType? spendingType, DateTime dateFrom, DateTime dateTo)
    {
        Transactions.Clear();
        
        var specs = new TransactionsSpecification
        {
            ProfileId = profileId,
            CategoryId = categoryId,
            SpendingType = spendingType,
            FromDate = dateFrom,
            ToDate = dateTo
        };

        var transactions = await _transactionRepository.GetFiltered(specs);

        foreach (var transaction in transactions)
        {
            Transactions.Add(TransactionModel.FromDomain(transaction));
        }
    }
}