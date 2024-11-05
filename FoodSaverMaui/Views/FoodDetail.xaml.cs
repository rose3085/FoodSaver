using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class FoodDetail : ContentPage
{
	public FoodDetail(FoodDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}