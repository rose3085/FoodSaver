using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.SignalRServices
{
    public interface ISignalRService
    {
        Task ConnectToHubAsync();
        Task SendNotification(string sellerId, string message, string buyerId);
        Task Dispose();
    }
}
