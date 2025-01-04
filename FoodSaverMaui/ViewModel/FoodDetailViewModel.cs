using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Model;
using FoodSaverMaui.Response;
using FoodSaverMaui.Views;



namespace FoodSaverMaui.ViewModel
{

    [QueryProperty(nameof(Product), "Product")]
    public partial class FoodDetailViewModel : BaseViewModel
    {
        
        private GetProductsResponse product;

        public GetProductsResponse Product // Define the property itself
        {
            get => product;
            set => SetProperty(ref product, value); // Use SetProperty to notify change
        }


        [ObservableProperty]
        public BuyFoodModel purchaseDetail = new();


        public Command OnRemoveButtonPressed { get; }
        public Command OnPinLocationTapped { get; }
        public Command OnBuyButtonPressed { get; }

        public FoodDetailViewModel()
        {
            OnRemoveButtonPressed = new Command(async() => await RemoveButtonPressed());
            OnPinLocationTapped = new Command(async () => await PinLocationTapped());
            OnBuyButtonPressed = new Command(async () => await BuyButtonPressed());
        }

        public async Task RemoveButtonPressed()
        {
            await Shell.Current.GoToAsync("..");
        }
        public async Task BuyButtonPressed()
        {
            var userName = await SecureStorage.GetAsync("userName");
            if (Product.UserName == userName)
            {
                IsBusy = false;
            }
            string cityName = await Shell.Current.DisplayPromptAsync("Delivery Address","Enter your city name ?");
            string wardNumber = await Shell.Current.DisplayPromptAsync("Delivery Address", "Enter your ward number ?",maxLength:2,keyboard:Keyboard.Numeric);
            string toleName = await Shell.Current.DisplayPromptAsync("Delivery Address", "Enter your tole name ?");

            if (cityName != null && wardNumber != null && toleName != null && product.PricePerKg != null && product.Id != null)
            {
                 purchaseDetail = new BuyFoodModel
                {
                    CityName = cityName,
                    WardNumber = wardNumber,
                    ToleName = toleName,
                    Amount = product.PricePerKg,
                    ProductId = product.Id
                };
                var navigationParameter = new Dictionary<string, object>
                {
                    { "PurchaseDetail",purchaseDetail }
                };
                await Shell.Current.GoToAsync(nameof(KhaltiPaymentView), navigationParameter);
                //await Shell.Current.GoToAsync($"{nameof(KhaltiPaymentView)}&Amount={product.PricePerKg}&ProductId={product.Id}&CityName=Uri.EscapeDataString({cityName})&WardNumber={wardNumber}&ToleName=Uri.EscapeDataString({toleName}");
            }
            else 
            {
                return;
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
