using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class KhaltiPaymentView : ContentPage
{
	public KhaltiPaymentView(KhaltiPaymentViewModel vm)
	{
        InitializeComponent();
		BindingContext = vm;
	}
}