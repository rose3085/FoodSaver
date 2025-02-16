using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class TempPage : ContentPage
{
	public TempPage(TempPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

	}
}