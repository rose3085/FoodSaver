using CommunityToolkit.Mvvm.ComponentModel;
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
        private readonly UserProfileService _userProfileService;

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


        //public Command NavigatedPage { get; }
        public PaymentUrlViewModel(ISignalRService signalRService,UserProfileService userProfileService)
        {
            _signalRService = signalRService;
            _userProfileService = userProfileService;
           // NavigatedPage = new Command(async() => await OnPageNavigation());
        }



        public async Task SendNotificationToSeller()
        {
            try
            {
                var productId = await SecureStorage.GetAsync("productId");
                var user = await _userProfileService.GetUserByName();
                if (user != null)
                {
                    var userId = user.Id;
                    await _signalRService.SendNotification("dd27b90-05b9-49a3-a2d6-5271d50b6c41", "MeowwwwwwwwwwwwwwBhowwwwwwwwww");

                }
            }
            catch { }
        
        }
        
    }
}
