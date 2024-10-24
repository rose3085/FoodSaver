using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class DeleteUser : ContentPage
{
	public DeleteUser(DeleteUserViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}