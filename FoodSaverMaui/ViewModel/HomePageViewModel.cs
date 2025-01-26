using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Helper.CacheHelper;
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
        private readonly HubConnection _hubConnection;
        private readonly ISignalRService _signalRService;
      public HomePageViewModel(ICacheService cacheService, ISignalRService signalRService)
        {
            OnPageMount = new Command(async() => await PageMount());
            _signalRService = signalRService;
            _signalRService.ConnectToHubAsync();

        }
        public async Task PageMount()
        {
            try {
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
                            IsSeller = true;
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
