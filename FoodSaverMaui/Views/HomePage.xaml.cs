using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class HomePage : Shell
{
	public HomePage(HomePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}