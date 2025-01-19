using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class FoodDetail : ContentPage
{
	public FoodDetail(FoodDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
        
        if (BindingContext is FoodDetailViewModel viewModel)
		{
            if (viewModel.OnPageMount.CanExecute(null))
            {
                viewModel.OnPageMount.Execute(null);
            }
        }

    }
}