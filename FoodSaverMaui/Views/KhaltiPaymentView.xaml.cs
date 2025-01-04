using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class KhaltiPaymentView : ContentPage
{
	public KhaltiPaymentView(KhaltiPaymentViewModel vm)
	{
        InitializeComponent();
		BindingContext = vm;
	}


    //protected async override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    if (BindingContext is KhaltiPaymentViewModel viewModel)
    //    {
    //        if (viewModel.OnPageLoad.CanExecute(null)) // Check if the command can be executed
    //        {
    //            viewModel.OnPageLoad.Execute(null); // Execute the command
    //        }
    //    }
    //}
    }