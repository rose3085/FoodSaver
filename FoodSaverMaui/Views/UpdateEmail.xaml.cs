using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class UpdateEmail : ContentPage
{
	public UpdateEmail(UpdateEmailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}