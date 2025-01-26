
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
    protected override void OnAppearing()
    {
        base.OnAppearing();
        SettingsExpander.IsExpanded = false;

        if (BindingContext is UserProfileViewModel viewModel)
        {
            // Check and execute OnPostTapped command
            if (viewModel.OnPageLoad.CanExecute(null))
            {
                 viewModel.OnPageLoad.Execute(null);
            }

            //// Check and execute OnGetUserName command
            //if (viewModel.OnGetUserName.CanExecute(null))
            //{
            //    viewModel.OnGetUserName.Execute(null);
            //}
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        SettingsExpander.IsExpanded = false;
    }


}