using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Helper;
using FoodSaverMaui.Helper.CacheHelper;
using FoodSaverMaui.Services.User;
using FoodSaverMaui.SignalRServices;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
    public partial class HomePageViewModel : BaseViewModel
    {
        public Command OnPageMount { get; }

        private readonly IJwtHelper _jwtHelper;
        private readonly HubConnection _hubConnection;
        private readonly ISignalRService _signalRService;
        private readonly UserProfileService _userProfileService;

        public HomePageViewModel(ICacheService cacheService, ISignalRService signalRService,IJwtHelper jwtHelper,UserProfileService userProfileService)
        {
            OnPageMount = new Command(async() => await PageMount());
            _jwtHelper = jwtHelper;
            _userProfileService = userProfileService;
            _signalRService = signalRService;
           
            _signalRService.ConnectToHubAsync();

        }
        public async Task PageMount()
        {
            try {

                var shellItem = Shell.Current?.CurrentItem;



                IsSeller = false;
                IsBuyer = false;
              
                var roles = await SecureStorage.GetAsync("roles");
                if (roles != null)
                {
                    var rolesList = JsonSerializer.Deserialize<IList<string>>(roles);
                    if (rolesList != null)
                    {
                        if (rolesList.Count() > 0 && rolesList.Contains("Seller"))
                        {
                           
                            var user =await _userProfileService.GetUserByName();
                            if (user != null)
                            {
                                if (user.CanPost == true)
                                {
                                    IsSeller = true;
                                }
                                else { IsSeller = false; }
                            }
                        }

                        if (rolesList.Count() > 0 && rolesList.Contains("Buyer"))
                        {
                            IsBuyer = true;
                        }
                       
                    }
                }
            }
            catch
            { 
            
            }
            
        }
   
       
    }
}
