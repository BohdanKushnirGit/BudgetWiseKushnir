using BudgetWise.Core.Domain.Model.Categories;
using BudgetWise.Mobile.Resources.Strings;

namespace BudgetWise.Mobile.Models.Categories;

public class CategoryModel
{
    public Guid? Id { get; set; }
    public required Guid ProfileId { get; set; }
    public required string Name { get; set; }
    public decimal? PlannedAmount { get; set; }

    public string? DisplayPlannedAmount { get; set; }

    public static CategoryModel FromDomain(Category category)
    {
        var displayPlannedAmount = category.PlannedAmount is not null
            ? category.PlannedAmount.ToString()
            : AppResources.Categories_NoLimits;
        
        return new CategoryModel
        {
            Id = category.Id,
            ProfileId = category.ProfileId,
            Name = category.Name,
            PlannedAmount = category.PlannedAmount,
            DisplayPlannedAmount = displayPlannedAmount
        };
    }
}