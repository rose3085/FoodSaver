using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class HomePage : Shell
{
	public HomePage(HomePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await SecureStorage.SetAsync("isLoggedOut", "no");
        if (BindingContext is HomePageViewModel viewModel)
        {
            if (viewModel.OnPageMount.CanExecute(null)) 
            {
                viewModel.OnPageMount.Execute(null); 
            }
        }
     

    }
    protected async override void OnDisappearing()
    {
        base.OnDisappearing();
        Preferences.Set("HomePage", "//HomePage");
    }
}