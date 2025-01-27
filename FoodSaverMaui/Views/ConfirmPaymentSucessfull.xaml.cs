using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class ConfirmPaymentSucessfull : ContentPage
{
	public ConfirmPaymentSucessfull(ConfirmPaymentSucessfullViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ConfirmPaymentSucessfullViewModel viewModel)
        {
            if (viewModel.OnPageMount.CanExecute(null)) // Check if the command can be executed
            {
                viewModel.OnPageMount.Execute(null); // Execute the command
            }

        }
        await Task.Delay(1000);
        await Shell.Current.GoToAsync("//HomePage");

    }
}