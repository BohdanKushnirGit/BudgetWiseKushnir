using BudgetWise.Core.Domain.Model.Shared.ValueObjects;
using BudgetWise.Mobile.Resources.Strings;

namespace BudgetWise.Mobile.Services;

public static class CurrencyService
{
    public static readonly Dictionary<string, string> CurrencyNames = new();

    static CurrencyService()
    {
        foreach (var currency in Currency.AvailableCurrencies.All.Values)
        {
            var resourceName = $"Currencies_{currency.Code}";
            var currencyName = AppResources.ResourceManager.GetString(resourceName, AppResources.Culture);

            if (string.IsNullOrWhiteSpace(currencyName))
            {
                continue;   
            }
            
            CurrencyNames.Add(currency.Code, currencyName);
        }
    }
}