using BudgetWise.Core.Domain.Model.Shared.ValueObjects;
using BudgetWise.Mobile.Services;

namespace BudgetWise.Mobile.Models.Profiles;

public class SavedAmountModel
{
    public required string CurrencyName { get; set; }
    public required string Amount { get; set; }

    public static SavedAmountModel FromDomain(KeyValuePair<Currency, decimal> savedAmount)
    {
        return new SavedAmountModel
        {
            CurrencyName = CurrencyService.CurrencyNames[savedAmount.Key.Code],
            Amount = $"{savedAmount.Key.Symbol}{savedAmount.Value}",
        };
    }
}