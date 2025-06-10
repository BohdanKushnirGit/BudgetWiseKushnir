using BudgetWise.Core.Domain.Model.Categories;
using BudgetWise.Core.Domain.Model.Categories.Factories;
using BudgetWise.Infrastructure.Abstractions.Internal;
using BudgetWise.Infrastructure.Persistence.Sqlite.Models.Category;

namespace BudgetWise.Infrastructure.Persistence.Sqlite.Mappers;

internal class CategoryMapper : IInfrastructureMapper<Category, CategoryModel>
{
	public Category MapToDomain(CategoryModel model)
	{
		return CategoryFactory.CreateCategory(
			model.Id,
			model.ProfileId,
			model.Name,
			model.PlannedAmount);
	}

	public CategoryModel MapToModel(Category entity)
	{
		return new CategoryModel
		{
			Id = entity.Id,
			Name = entity.Name,
			ProfileId = entity.ProfileId,
			PlannedAmount = entity.PlannedAmount
		};
	}
}