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
            if (viewModel.OnClickTapped.CanExecute(null)) // Check if the command can be executed
            {
                viewModel.OnClickTapped.Execute(null); // Execute the command
            }
        }
    }
}