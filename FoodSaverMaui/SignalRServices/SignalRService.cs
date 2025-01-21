using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.SignalRServices
{
    public class SignalRService : ISignalRService
    {
        private readonly HubConnection _hubConnection;
        public ObservableCollection<string> Notifications { get; } = new ObservableCollection<string>();
        public SignalRService()
        {
            
            _hubConnection = new HubConnectionBuilder()
                 .WithUrl($"https://0b11-2405-acc0-1504-cce4-d173-a702-8018-1210.ngrok-free.app/notificationHub", options =>
                 {
                     options.AccessTokenProvider = async () =>
                     {
                        
                         var token = await SecureStorage.GetAsync("token");
                         return token; 
                     };
                 })
                // .WithAutomaticReconnect()
                 .Build();
            //Messages ??= new ObservableCollection<string>();

            // Dispose();
            //ConnectToHubAsync();


            _hubConnection.On<string>("ReceiveMessage", (message) =>
            {
                // Add the message to the UI-bound collection
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    //Notifications.Add(message);
                    var request = new NotificationRequest
                    {
                        NotificationId = 1111,
                        Title = "Food Saver",
                        Subtitle = "Notifications",
                        Description = message,
                        BadgeNumber = 27,
                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = DateTime.Now.AddSeconds(1),
                        }

                    };
                    LocalNotificationCenter.Current.Show(request);
                });
            });

        }

        public async void Dispose()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
               // await _hubConnection.DisposeAsync();
            }
        }

        public async Task Notify()
        {
            if (Notifications.Count > 0)
            {
                var request = new NotificationRequest
                {
                    NotificationId = 1111,
                    Title = "Meow",
                    Subtitle = "maui",
                    Description = $"{Notifications}",
                    BadgeNumber = 27,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                    }

                };
                LocalNotificationCenter.Current.Show(request);
            }
        }

        public async  Task ConnectToHubAsync()
        {
            try
            {
               
                await _hubConnection.StartAsync();
               
               

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }
        }

        
       public async Task SendNotification(string productId, string message)
        {


            try
            {
                await _hubConnection.InvokeAsync("SendNotification",productId, message);
                // Messages.Add($" {Name} Joined chat room: {Message}");
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                //Messages.Add($"Failed to join chat room: {ex.Message}");
            }

            //Message = string.Empty;
        }

    }
}
