using System.Globalization;
using BudgetWise.Core.Domain.Model.Settings;
using BudgetWise.Core.Domain.Model.Settings.ValueObjects;
using BudgetWise.Core.Persistence;
using BudgetWise.Mobile.Abstractions;
using BudgetWise.Mobile.Constants;
using BudgetWise.Mobile.Services;
using BudgetWise.Mobile.Views.Settings.Pages;

namespace BudgetWise.Mobile;

public partial class AppInit : BaseContentPage
{
    private readonly ISettingsRepository _settingsRepository;
    
    public AppInit(ISettingsRepository settingsRepository)
    {
        InitializeComponent();
        
        Routing.RegisterRoute(RoutesConstants.SetupPage, typeof(EditProfilePage));
        _settingsRepository = settingsRepository;
    }
    
    public event EventHandler<EventArgs> Initialized = null!;

    private void AppInit_OnLoaded(object? sender, EventArgs e)
    {
	    ProcessAction(async () =>
	    {
		    await InitializeApplication();
		    Initialized.Invoke(this, e);
	    });
    }
    
    private async Task InitializeApplication()
    {
    	await InitializeSettings();
    }

    private async Task InitializeSettings()
    {
    	var settings = await _settingsRepository.GetCurrentSettings();
    	var theme = settings?.Theme ?? Theme.System;
    	
    	ThemeService.ChangeTheme(theme);
    	
    	if (settings is not null)
    	{
    		LocalizationService.ChangeCurrentLanguage(settings.Language);
    		return;
    	}

    	var lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    	
    	if (LocalizationService.SupportedLanguages.Contains(lang))
    	{
    		LocalizationService.ChangeCurrentLanguage(lang);
    	}
    	else
    	{
    		lang = LocalizationService.CurrentLanguage;
    	}
    	
    	settings = new Settings(
    		Guid.NewGuid(), 
    		theme,
    		lang);

    	await _settingsRepository.CreateOrUpdate(settings);
    }
}