using BudgetWise.Core.Domain.Model.Shared.ValueObjects;
using BudgetWise.Mobile.Abstractions;
using BudgetWise.Mobile.Models.Categories;
using BudgetWise.Mobile.Resources.Strings;
using BudgetWise.Mobile.ViewModels.Home;
using BudgetWise.Mobile.Views.Transactions.Pages;

namespace BudgetWise.Mobile.Views.Home.Pages;

public partial class HomePage : BaseContentPage
{
	private readonly HomePageViewModel _viewModel;
	
	public HomePage(HomePageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = _viewModel = viewModel;
		SavedAmountsCollectionView.ItemsSource = _viewModel.SavedAmounts;
		// This is a hot fix of a known issue related to collection view in iOS
		SavedAmountsCollectionView.ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem;
		
		CategoriesExpensesCollectionView.ItemsSource = _viewModel.CategoriesExpenses;
	}

	private void HomePage_OnNavigated(object? sender, EventArgs e)
	{
		ProcessAction(async () =>
		{
			await _viewModel.Initialize();
		});
	}

	private void CategoryLayout_OnTapped(object? sender, EventArgs e)
	{
		ProcessAction(async () =>
		{
			await OpenFilteredTransactionsByCategoryPage(sender);	
		});
	}
	
	private void MainSpendingTypeLayout_OnTapped(object? sender, EventArgs e)
	{
		SpendingTypeLayout_OnTapped(SpendingType.Main);
	}

	private void SecondarySpendingTypeLayout_OnTapped(object? sender, EventArgs e)
	{
		SpendingTypeLayout_OnTapped(SpendingType.Secondary);
	}

	private void SavedSpendingTypeLayout_OnTapped(object? sender, EventArgs e)
	{
		SpendingTypeLayout_OnTapped(SpendingType.Saved);
	}
	
	private void SpendingTypeLayout_OnTapped(SpendingType type)
	{
		ProcessAction(async () =>
		{
			await OpenFilteredTransactionsBySpendingTypePage(type);
		});
	}

	private async Task OpenFilteredTransactionsByCategoryPage(object? sender)
	{
		if ((sender as StackLayout)?.BindingContext is not CategoryExpenseModel category)
		{
			throw new Exception(AppResources.CommonError_GetCategoryInfo);
		}
		
		var filteredPage = Handler?.MauiContext?.Services.GetService<FilteredTransactionsPage>();

		if (filteredPage is null)
		{
			throw new Exception(AppResources.CommonError_ShowFilteredTransactions);
		}
		
		await filteredPage.Initialize(
			_viewModel.ProfileId,
			category.Id, 
			spendingType: null, 
			dateFrom: DateTime.Parse(_viewModel.DateFrom),
			dateTo: DateTime.Parse(_viewModel.DateTo));
		
		await Navigation.PushModalAsync(filteredPage);
	}
	
	private async Task OpenFilteredTransactionsBySpendingTypePage(SpendingType type)
	{
		var filteredPage = Handler?.MauiContext?.Services.GetService<FilteredTransactionsPage>();
		
		if (filteredPage is null)
		{
			throw new Exception(AppResources.CommonError_ShowFilteredTransactions);
		}
		
		await filteredPage.Initialize(
			_viewModel.ProfileId,
			categoryId: null, 
			type,
			dateFrom: DateTime.Parse(_viewModel.DateFrom),
			dateTo: DateTime.Parse(_viewModel.DateTo));

		await Navigation.PushModalAsync(filteredPage);
	}
}