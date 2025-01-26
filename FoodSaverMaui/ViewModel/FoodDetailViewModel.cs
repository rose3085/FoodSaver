
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Response;
using FoodSaverMaui.Views;



namespace FoodSaverMaui.ViewModel
{

    [QueryProperty(nameof(Product), "Product")]
    public partial class FoodDetailViewModel : BaseViewModel
    {

        [ObservableProperty]
        bool isBuyButtonVisible;

        private GetProductsResponse product;

        public GetProductsResponse Product // Define the property itself
        {
            get => product;
            set => SetProperty(ref product, value); // Use SetProperty to notify change
        }

        private readonly IJwtHelper _jwtHelper;

        public Command OnRemoveButtonPressed { get; }
        public Command OnPinLocationTapped { get; }
        public Command OnBuyButtonPressed { get; }
        public Command OnPageMount { get; }
        public FoodDetailViewModel(IJwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            OnRemoveButtonPressed = new Command(async() => await RemoveButtonPressed());
            OnPinLocationTapped = new Command(async () => await PinLocationTapped());
            OnBuyButtonPressed = new Command(async () => await BuyButtonPressed());
            OnPageMount = new Command(async() => await PageMount());
        }

        public async Task PageMount()
        {
            IsBuyButtonVisible = false;
            var token = await SecureStorage.GetAsync("token");
            if(token != null){
                var userName =  _jwtHelper.ExtractUserInfo(token);
                if(Product.UserName == userName)
                {
                   IsBuyButtonVisible = false;
                }
                else
                {
                   IsBuyButtonVisible = true;
                }
            }
        }
        public async Task RemoveButtonPressed()
        {
            await Shell.Current.GoToAsync("..");
        }
        public async Task BuyButtonPressed()
        {
            //var userName = await SecureStorage.GetAsync("userName");
            //if (Product.UserName == userName)
            //{
            //    IsBusy = false;
            //}

            var city = await Shell.Current.DisplayPromptAsync("Delivery Address","Enter your city name?");
            if (city != null)
            {
                var toleName = await Shell.Current.DisplayPromptAsync("Delivery Address", "Enter your tole name?");
                if (toleName != null)
                {
                    var wardNumber = await Shell.Current.DisplayPromptAsync("Delivery Address", "Enter your ward number?");
                    if (wardNumber != null)
                    {
                        await Shell.Current.GoToAsync($"{nameof(KhaltiPaymentView)}?Amount={product.PricePerKg}&ProductId={product.Id}&CityName={city}&ToleName={toleName}&WardNumber={wardNumber}");
                    }
                }
            }


           
        }

        public async Task PinLocationTapped()
        {
            var location = new Microsoft.Maui.Devices.Sensors.Location(product.Latitude,product.Longitude);
            var options = new MapLaunchOptions { Name = "Butwal" };

            try
            {
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                var toasts = Toast.Make($"Couldn't open GoogleMap", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await toasts.Show();

            }
        }

    }
}
