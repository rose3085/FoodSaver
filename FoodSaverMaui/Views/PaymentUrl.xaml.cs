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
        string baseUrl = $"{App.Settings.ApiBaseUrl}/PaymentReturn/ReturnUrl";

        if (currentUrl.StartsWith(baseUrl))
        {
            await Task.Delay(3000);
            await Shell.Current.GoToAsync("..");
        }
    }

}