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
    protected override void OnAppearing()
    {
        base.OnAppearing();



        if (_isFirstLoad && BindingContext is HomePageViewModel viewModel)
        {
            _isFirstLoad = false;

        
            
            
            // Mark as loaded
                                  // RunOnFirstLoad();
            //if (viewModel.OnClickTapped.CanExecute(null)) // Check if the command can be executed
            //{
            //    viewModel.OnClickTapped.Execute(null); // Execute the command
            //}
        }
    }
}