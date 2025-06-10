using BudgetWise.Core.Domain.Model.Profiles;
using BudgetWise.Core.Domain.Model.Profiles.Factories;
using BudgetWise.Core.Domain.Model.Shared.ValueObjects;
using BudgetWise.Infrastructure.Abstractions.Internal;
using BudgetWise.Infrastructure.Persistence.Sqlite.Models.Profile;

namespace BudgetWise.Infrastructure.Persistence.Sqlite.Mappers;

internal class ProfileMapper : IInfrastructureMapper<Profile, ProfileModel>
{
	public Profile MapToDomain(ProfileModel model)
	{
		var builder = new ProfileBuilder(model.Id)
			.AddBalance(model.Balance)
			.AddName(model.Name)
			.AddStartDate(model.StartTimestamp, model.InitialBalance)
			.AddCurrency(Currency.AvailableCurrencies.All[model.CurrencyCode])
			.AddIsCurrent(model.IsCurrent);

		if (model.Categories is null)
		{
			return builder.Build();
		}
		
		foreach (var modelCategory in model.Categories)
		{
			builder.AddCategoryExpense(
				modelCategory.CategoryId, 
				modelCategory.Name, 
				modelCategory.PlannedAmount);
		}

		return builder.Build();
	}

	public ProfileModel MapToModel(Profile entity)
	{
		var categories = entity.CategoriesExpenses
			.Select(c => new ProfileCategoryModel
			{
				CategoryId = c.Id,
				Name = c.Name,
				PlannedAmount = c.PlannedAmount
			})
			.ToList();
		
		return new ProfileModel
		{
			Id = entity.Id,
			Name = entity.Name,
			StartTimestamp = entity.StartDate.Timestamp,
			InitialBalance = entity.StartDate.InitialBalance,
			Balance = entity.Balance,
			IsCurrent = entity.IsCurrent,
			Categories = categories,
			CurrencyCode = entity.Settings.Currency.Code
		};
	}
}