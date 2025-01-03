using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class EditProfile : ContentPage
{
	public EditProfile(EditProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EditProfileViewModel viewModel)
        {
            if (viewModel.OnGetUserInfo.CanExecute(null)) // Check if the command can be executed
            {
                viewModel.OnGetUserInfo.Execute(null); // Execute the command
            }
        }
        //await Task.Delay(1000);
        //await Shell.Current.GoToAsync("//HomePage");

    }
}