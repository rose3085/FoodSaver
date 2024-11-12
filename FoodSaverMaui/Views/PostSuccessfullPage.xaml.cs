using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class PostSuccessfullPage : ContentPage
{
	public PostSuccessfullPage(PostSuccessfullViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}