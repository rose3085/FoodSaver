using FoodSaverMaui.ViewModel;

namespace FoodSaverMaui.Views;

public partial class ConfirmPaymentUrl : ContentPage
{
	public ConfirmPaymentUrl(ConfirmPaymentUrlViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
    private async void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
    {
       
        var webView = sender as WebView;
        var currentUrl = e.Url;
        await SecureStorage.SetAsync("khaltiReturnUrl", currentUrl);
        string baseUrl = $"{App.Settings.ApiBaseUrl}/PaymentReturn/ReturnUrl";
        if (currentUrl.StartsWith(baseUrl))
        {
            Uri currentUri = new Uri(currentUrl);
            var queryParams = System.Web.HttpUtility.ParseQueryString(currentUri.Query);
            string status = queryParams["status"];
            if (status == "Completed")
            {
               

                await Task.Delay(1000);

                await Shell.Current.GoToAsync(nameof(ConfirmPaymentSucessfull));
            }
            if (status == "User canceled")
            {
                await Task.Delay(1000);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
    }