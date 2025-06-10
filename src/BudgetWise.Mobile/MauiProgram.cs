using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using BudgetWise.Core;
using BudgetWise.Infrastructure;
using BudgetWise.Mobile.ViewModels.Categories;
using BudgetWise.Mobile.ViewModels.Home;
using BudgetWise.Mobile.ViewModels.Overview;
using BudgetWise.Mobile.ViewModels.Profiles;
using BudgetWise.Mobile.ViewModels.Settings;
using BudgetWise.Mobile.ViewModels.Transactions;
using BudgetWise.Mobile.Views.Home.Pages;
using BudgetWise.Mobile.Views.Overview.Pages;
using BudgetWise.Mobile.Views.Settings.Pages;
using BudgetWise.Mobile.Views.Transactions.Pages;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace BudgetWise.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		
		builder
			.UseSkiaSharp()
			.UseLiveCharts()
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Biome-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("Biome-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Ionicons.ttf", "Ionicons");
			})
			.RegisterAppServices()
			.RegisterViewModels()
			.RegisterViews();

		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		var infrastructureConfig = new InfrastructureConfiguration
		{
			AppDirectoryPath = FileSystem.AppDataDirectory 
		};
		
		builder.Services
			.RegisterInfrastructureServices(infrastructureConfig)
			.RegisterCoreServices()
			.AddSingleton<TransactionsPage>();

		return builder.Build();
	}
	
	private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
	{
		_ = mauiAppBuilder.Services
			.AddSingleton<AppShell>()
			.AddSingleton<AppInit>();

		return mauiAppBuilder;
	}

	private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
	{
		_ = mauiAppBuilder.Services
			.AddTransient<HomePageViewModel>()
			.AddTransient<EditTransactionPageViewModel>()
			.AddTransient<FilteredTransactionsPageViewModel>()
			.AddTransient<TransactionsPageViewModel>()
			.AddTransient<ExpenseCategoriesSettingsPageViewModel>()
			.AddTransient<EditExpenseCategoryPageViewModel>()
			.AddTransient<OverviewPageViewModel>()
			.AddTransient<LanguageSettingsViewModel>()
			.AddTransient<ProfileSettingsPageViewModel>()
			.AddTransient<TransactionsFiltersPageViewModel>()
			.AddTransient<EditProfilePageViewModel>()
			.AddTransient<ThemeSettingsPageViewModel>();
		
		return mauiAppBuilder;
	}

	private static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
	{
		_ = mauiAppBuilder.Services
			.AddTransient<HomePage>()
			.AddTransient<TransactionsPage>()
			.AddTransient<FilteredTransactionsPage>()
			.AddTransient<TransactionsFiltersPage>()
			.AddTransient<EditTransactionPage>()
			.AddTransient<ExpenseCategoriesSettingsPage>()
			.AddTransient<EditExpenseCategoryPage>()
			.AddTransient<OverviewPage>()
			.AddTransient<ProfilesSettingsPage>()
			.AddTransient<EditProfilePage>()
			.AddTransient<ThemeSettingsPage>()
			.AddTransient<LanguageSettingsPage>();
		
		return mauiAppBuilder;
	}
}