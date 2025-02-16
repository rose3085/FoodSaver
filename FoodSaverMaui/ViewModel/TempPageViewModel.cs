using FoodSaverMaui.SignalRServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
   public partial class TempPageViewModel : BaseViewModel
    {
        private readonly ISignalRService _signalRService;

        public TempPageViewModel(ISignalRService signalRService)
        {
            _signalRService = signalRService;

            _signalRService.ConnectToHubAsync();
        }
    }
}
