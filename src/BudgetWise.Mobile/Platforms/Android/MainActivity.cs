using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace BudgetWise.Mobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
	ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
	                       ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Установить фиолетовый цвет статус-бара
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && Window != null)
        {
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#8B5FBF"));

            // Установить светлые иконки статус-бара (белые) для фиолетового фона
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M && Window.DecorView != null)
            {
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
            }
        }
    }
}