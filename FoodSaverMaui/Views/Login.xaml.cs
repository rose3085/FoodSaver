using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class Login : ContentPage
{
	public Login(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}