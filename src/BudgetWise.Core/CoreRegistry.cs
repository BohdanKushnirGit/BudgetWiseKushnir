using Microsoft.Extensions.DependencyInjection;
using BudgetWise.Core.Domain.Abstractions.Services;
using BudgetWise.Core.Domain.Services;

namespace BudgetWise.Core;

public static class CoreRegistry
{
    public static IServiceCollection RegisterCoreServices(this IServiceCollection services)
    {
        return services
            .AddTransient<IProfileService, ProfileService>()
            .AddTransient<ICalculationService, CalculationService>()
            .AddTransient<ICategoryService, CategoryService>()
            .AddTransient<ITransactionService, TransactionService>();
    } 
}