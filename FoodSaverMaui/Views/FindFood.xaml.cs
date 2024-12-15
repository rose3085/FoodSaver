using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class FindFood : ContentPage
{
	public FindFood(FindFoodViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Execute the command directly
        if (BindingContext is FindFoodViewModel viewModel)
        {
            if (viewModel.OnClickTapped.CanExecute(null)) 
            {
                viewModel.OnClickTapped.Execute(null); 
            }
        }
    }
}