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


       
        public async Task BuyPost()
        {
            var pidx = await SecureStorage.GetAsync("pidx");
            var amount = await SecureStorage.GetAsync("amount");
            var productId = await SecureStorage.GetAsync("productId");
            var token = await SecureStorage.GetAsync("token");
            var userName = _jwtHelper.ExtractUserInfo(token);
            var requestModel = new BuyFoodModel()
            {
                PidX = pidx,
                Amount = double.Parse(amount),
                ProductId = productId,
                BuyerName = userName,
            };
            var request = await _foodService.BuyFood(requestModel);
            if (request.IsSuccess == true)
            {
                var message = $"{userName} wants to buy your product.";
                await _signalRService.SendNotification(productId,message);
               // SendNotification(productId, message);
                 SecureStorage.Remove("pidx");
                SecureStorage.Remove("amount");
                SecureStorage.Remove("productId");
            }
            
        }
    }
}
