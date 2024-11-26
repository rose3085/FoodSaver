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



    private async void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
    {
        // Check if the URL matches the expected one
        var webView = sender as WebView;
        var currentUrl = e.Url;
        await SecureStorage.SetAsync("khaltiReturnUrl",currentUrl);
        // Define the URL you want to detect
        string baseUrl = "https://1b30-2405-acc0-1504-b3c0-bce9-5e05-9a37-a8ad.ngrok-free.app/PaymentReturn/ReturnUrl";

        if (currentUrl.StartsWith(baseUrl))
        {
            await Task.Delay(3000);
            await Shell.Current.GoToAsync("..");
        }
    }

}