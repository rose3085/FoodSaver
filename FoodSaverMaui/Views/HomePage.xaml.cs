using FoodSaverMaui.ViewModel;
using Plugin.LocalNotification;

namespace FoodSaverMaui.Views;

public partial class HomePage : Shell
{
    private bool _isFirstLoad = true;
    public HomePage(HomePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await SecureStorage.SetAsync("isLoggedOut", "no");
        if (_isFirstLoad && BindingContext is HomePageViewModel viewModel)
        {
            _isFirstLoad = false;
            if (viewModel.OnPageMount.CanExecute(null)) 
            {
                viewModel.OnPageMount.Execute(null); 
            }
        }
     

    }
    protected async override void OnDisappearing()
    {
        base.OnDisappearing();
      
    }
   
    
}