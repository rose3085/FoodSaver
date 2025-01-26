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
            Uri currentUri = new Uri(currentUrl);
            var queryParams = System.Web.HttpUtility.ParseQueryString(currentUri.Query);
            string status = queryParams["status"];
            if (status == "Completed")
            {
                if (BindingContext is PaymentUrlViewModel viewModel)
                {
                    if (viewModel.OnSendNotificationToSeller.CanExecute(null)) 
                    {
                        viewModel.OnSendNotificationToSeller.Execute(null);
                    }

                }

                await Task.Delay(1000);

                await Shell.Current.GoToAsync(nameof(PostSuccessfullPage));
            }
            if(status == "User canceled")
                  {
                await Task.Delay(1000);
                await Shell.Current.GoToAsync("..");
            }
        }
    }

}