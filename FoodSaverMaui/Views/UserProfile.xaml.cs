using CommunityToolkit.Maui.Layouts;
using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class UserProfile : ContentPage
{
	public UserProfile(UserProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext=vm;
    
    }
    
    
}