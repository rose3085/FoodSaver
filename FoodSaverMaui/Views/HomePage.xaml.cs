using FoodSaverMaui.ViewModel;
using Microsoft.Maui.Controls;
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

        //var shellItem = Shell.Current?.CurrentItem;
        var shellSection = Shell.Current.CurrentItem?.CurrentItem;
       
        // await Shell.Current.GoToAsync(nameof(LandingPage));
        //var tabBar = Shell.Current.Items.OfType<TabBar>().FirstOrDefault();
        //if (tabBar != null)
        //{
        //    var items = tabBar.Items.ToList();
        //    tabBar.Items.Clear();
        //    foreach (var item in items)
        //    {
        //        tabBar.Items.Add(item);
        //    }
        //}

        if (_isFirstLoad && BindingContext is HomePageViewModel viewModel)
        {
            _isFirstLoad = false;
            if (viewModel.OnPageMount.CanExecute(null)) 
            {
                viewModel.OnPageMount.Execute(null); 
            }
        }
     

    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        //RefreshTabBar();
    }

    private void RefreshTabBar()
    {
        var tabBar = Shell.Current.Items.OfType<TabBar>().FirstOrDefault();
        if (tabBar != null)
        {
            var items = tabBar.Items.ToList(); // Backup items
            tabBar.Items.Clear(); // Remove all items
            foreach (var item in items)
            {
                tabBar.Items.Add(item); // Re-add items (forces a refresh)
            }
        }
    }



}