using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class UpdatePassword : ContentPage
{
	public UpdatePassword(UpdatePasswordViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}