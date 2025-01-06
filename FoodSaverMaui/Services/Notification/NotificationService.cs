using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Services.Notification
{
    public class NotificationService
    {
        private readonly HubConnection _hubConnection;
        public NotificationService()
        {
           var userName = SecureStorage.GetAsync("userName");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{App.Settings.ApiBaseUrl}/notificationHub")
            .Build();

            _hubConnection.On<string>("Notify", (payload) =>
            {
              
                ShowNotification(payload);
            });
        }

        public async Task StartConnectionAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                // Handle connection errors
            }
        }

        private void ShowNotification(string message)
        {
            // Display the notification, e.g., using a toast or updating the UI
            Console.WriteLine($"Notification received: {message}");
        }
    }
}
