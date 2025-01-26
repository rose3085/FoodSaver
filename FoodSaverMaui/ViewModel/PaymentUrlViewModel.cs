using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Services.Food;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.SignalRServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    [QueryProperty(nameof(url), "url")]
    public partial class PaymentUrlViewModel : BaseViewModel
    {

        private string _url;
        private readonly ISignalRService _signalRService;
        private readonly IJwtHelper _jwtHelper;
        private readonly UserProfileService _userProfileService;
        private readonly FoodService _foodService;

        public string url
        {
            get => _url;
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged(nameof(url)); // Manual property change notification
                }
            }
        }

        public Command OnSendNotificationToSeller { get; }

        //public Command NavigatedPage { get; }
        public PaymentUrlViewModel(ISignalRService signalRService,IJwtHelper jwtHelper,UserProfileService userProfileService,FoodService foodService)
        {
            _signalRService = signalRService;
            _jwtHelper = jwtHelper;
            _userProfileService = userProfileService;
            _foodService = foodService;
            OnSendNotificationToSeller = new Command(async() => await SendNotificationToSeller());
           // NavigatedPage = new Command(async() => await OnPageNavigation());
        }



        public async Task SendNotificationToSeller()
        {
            try
            {
                var productId = await SecureStorage.GetAsync("productId");
              

                
                var product = await _foodService.GetProductById(productId);
                if (product != null)
                {
                    var user = await _userProfileService.GetUserByName();
                    var buyerId = user.Id;
                    var sellerId = product.SellerId;
                    var token = await SecureStorage.GetAsync("token");
                    var buyerName = _jwtHelper.ExtractUserInfo(token);
                    if (sellerId != null && buyerName != null && buyerId != null)
                    {

                        var message = $"{buyerName} has bought your product";
                        await _signalRService.SendNotification(sellerId, message,buyerId);

                    }
                }
            }
            catch { }
        
        }
        
    }
}
