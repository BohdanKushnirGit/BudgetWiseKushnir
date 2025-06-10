using Microsoft.Extensions.DependencyInjection;
using BudgetWise.Core.Domain.Model.Categories;
using BudgetWise.Core.Domain.Model.Profiles;
using BudgetWise.Core.Domain.Model.Settings;
using BudgetWise.Core.Domain.Model.Transactions;
using BudgetWise.Core.Persistence;
using BudgetWise.Infrastructure.Abstractions.Internal;
using BudgetWise.Infrastructure.Persistence.Sqlite.Configuration;
using BudgetWise.Infrastructure.Persistence.Sqlite.Mappers;
using BudgetWise.Infrastructure.Persistence.Sqlite.Models.Category;
using BudgetWise.Infrastructure.Persistence.Sqlite.Models.Profile;
using BudgetWise.Infrastructure.Persistence.Sqlite.Models.Settings;
using BudgetWise.Infrastructure.Persistence.Sqlite.Models.Transaction;
using BudgetWise.Infrastructure.Persistence.Sqlite.Repositories;

namespace BudgetWise.Infrastructure;

public static class InfrastructureRegistry
{
	public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, InfrastructureConfiguration configuration)
	{
		return services
			.RegisterPersistence(configuration)
			.RegisterMappers()
			.RegisterRepositories();
	}
	
	private static IServiceCollection RegisterPersistence(this IServiceCollection services, InfrastructureConfiguration configuration)
	{
		return services
			.AddSingleton(configuration)
			.AddTransient<DbConnection>();
	}

	private static IServiceCollection RegisterMappers(this IServiceCollection services)
	{
		return services
			.AddTransient<IInfrastructureMapper<Transaction, TransactionModel>, TransactionMapper>()
			.AddTransient<IInfrastructureMapper<Profile, ProfileModel>, ProfileMapper>()
			.AddTransient<IInfrastructureMapper<Category, CategoryModel>, CategoryMapper>()
			.AddTransient<IInfrastructureMapper<Settings, SettingsModel>, SettingsMapper>();
	}

	private static IServiceCollection RegisterRepositories(this IServiceCollection services)
	{
		return services
			.AddTransient<ITransactionRepository, TransactionRepository>()
			.AddTransient<IProfileRepository, ProfileRepository>()
			.AddTransient<ICategoryRepository, CategoryRepository>()
			.AddTransient<ISettingsRepository, SettingsRepository>();
	}
}