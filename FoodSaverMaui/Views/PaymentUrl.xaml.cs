using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class PaymentUrl : ContentPage
{
	public PaymentUrl(PaymentUrlViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();

    //    // Confirm that the URL is being passed correctly
    //    Console.WriteLine($"URL: {((PaymentUrlViewModel)BindingContext).Url}");
    //}
}