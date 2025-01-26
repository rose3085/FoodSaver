using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Model;
using FoodSaverMaui.Services.Food;
using FoodSaverMaui.SignalRServices;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace FoodSaverMaui.ViewModel
{
    public partial class PostSuccessfullViewModel : BaseViewModel
    {
      
        public readonly FoodService _foodService;
        private readonly IJwtHelper _jwtHelper;
        private readonly ISignalRService _signalRService;

        public Command OnBuyPost { get; }
        public PostSuccessfullViewModel(FoodService foodService,IJwtHelper jwtHelper,ISignalRService signalRService)
        {
            _foodService = foodService;
            _jwtHelper = jwtHelper;
            _signalRService = signalRService;
           
            OnBuyPost = new Command(async () => await BuyPost());
            
        }


        public async Task<Location> GetLocation(string cityName, string wardNumber, string toleName)
        {
            if (!string.IsNullOrEmpty(wardNumber) && !string.IsNullOrEmpty(toleName)
                    && !string.IsNullOrEmpty(cityName))
            {
                var address = $"{cityName} {toleName}";

                IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);
                Location location = locations?.FirstOrDefault();
                return location;
            }
            else { return null; }
        }

       
        public async Task BuyPost()
        {
            var pidx = await SecureStorage.GetAsync("pidx");
            var amount = await SecureStorage.GetAsync("amount");
            var productId = await SecureStorage.GetAsync("productId");
            var cityName = await SecureStorage.GetAsync("cityName");
            var toleName = await SecureStorage.GetAsync("toleName");
            var wardNumber = await SecureStorage.GetAsync("wardNumber");
            var token = await SecureStorage.GetAsync("token");
           
            var userName = _jwtHelper.ExtractUserInfo(token);
            if (!string.IsNullOrEmpty(wardNumber) && !string.IsNullOrEmpty(toleName)
                    && !string.IsNullOrEmpty(cityName))
            {
                var location = await GetLocation(cityName, wardNumber, toleName);
                if (location != null)
                {
                    var latitude = location.Latitude;
                    var longitude = location.Longitude;



                    var requestModel = new BuyFoodModel()
                    {
                        PidX = pidx,
                        Amount = double.Parse(amount),
                        ProductId = productId,
                        BuyerName = userName,
                        ToleName = toleName,
                        CityName = cityName,
                        WardNumber = wardNumber,
                        Latitude = latitude,
                        Longitude = longitude,
                    };
                    var request = await _foodService.BuyFood(requestModel);
                    if (request.IsSuccess == true)
                    {
                        var message = $"{userName} wants to buy your product.";
                        // await _signalRService.SendNotification(productId,message);
                        // SendNotification(productId, message);
                        SecureStorage.Remove("pidx");
                        SecureStorage.Remove("amount");
                        SecureStorage.Remove("productId");
                    }
                }
            }
            
        }
    }
}
