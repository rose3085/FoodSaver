using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class EditProfile : ContentPage
{
	public EditProfile(EditProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}