using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class PostSuccessfullPage : ContentPage
{
	public PostSuccessfullPage(PostSuccessfullViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

   


    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PostSuccessfullViewModel viewModel)
        {
            if (viewModel.OnBuyPost.CanExecute(null)) // Check if the command can be executed
            {
                viewModel.OnBuyPost.Execute(null); // Execute the command
            }
        }
        await Task.Delay(1000);
        await Shell.Current.GoToAsync("//HomePage");

    }


}