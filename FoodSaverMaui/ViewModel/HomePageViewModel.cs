using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.SignalRServices;
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
    public partial class HomePageViewModel : BaseViewModel
    {
        private readonly HubConnection _hubConnection;
        private readonly ISignalRService _signalRService;
        public HomePageViewModel(ISignalRService signalRService)
        {
            _signalRService = signalRService;
            _signalRService.ConnectToHubAsync();

        }
       
    }
}
