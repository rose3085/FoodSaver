using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class CreateAccount : ContentPage
{
	public CreateAccount(CreateAccountViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}