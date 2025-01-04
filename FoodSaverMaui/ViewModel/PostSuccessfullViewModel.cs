using FoodSaverMaui.Helper;
using FoodSaverMaui.Model;
using FoodSaverMaui.Services.Food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    public partial class PostSuccessfullViewModel : BaseViewModel
    {
        public readonly FoodService _foodService;
        private readonly IJwtHelper _jwtHelper;

        public Command OnBuyPost { get; }
        public PostSuccessfullViewModel(FoodService foodService,IJwtHelper jwtHelper)
        {
            _foodService = foodService;
            _jwtHelper = jwtHelper;
            OnBuyPost = new Command(async () => await BuyPost());
        }

        public async Task BuyPost()
        {
            var pidx = await SecureStorage.GetAsync("pidx");
            var amount = await SecureStorage.GetAsync("amount");
            var productId = await SecureStorage.GetAsync("productId");
            var token = await SecureStorage.GetAsync("token");
            var cityName = await SecureStorage.GetAsync("cityName");
            var wardNumber = await SecureStorage.GetAsync("wardNumber");
            var toleName = await SecureStorage.GetAsync("toleName");
            var userName = _jwtHelper.ExtractUserInfo(token);
            var requestModel = new BuyFoodModel()
            {
                PidX = pidx,
                Amount = double.Parse(amount),
                ProductId = productId,
                BuyerName = userName,
                ToleName = toleName,
                CityName = cityName,
                WardNumber = wardNumber,
            };
            var request = await _foodService.BuyFood(requestModel);
            if (request.IsSuccess == true)
            {
                SecureStorage.Remove("cityName");
                SecureStorage.Remove("wardNumber");
                SecureStorage.Remove("toleName");
                SecureStorage.Remove("pidx");
                SecureStorage.Remove("amount");
                SecureStorage.Remove("productId");
            }
            
        }
    }
}
