using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Response;
using FoodSaverMaui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
            await Shell.Current.GoToAsync($"{nameof(KhaltiPaymentView)}?Amount={product.PricePerKg}&ProductId={product.Id}");
        }

        public async Task PinLocationTapped()
        {
            var location = new Location(product.Latitude,product.Longitude);
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
