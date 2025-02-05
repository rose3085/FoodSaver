using FoodSaverMaui.Model;
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
        private bool _isProcessingNotifications = false;
        public ObservableCollection<string> Notifications { get; set; } = new ObservableCollection<string>();
        public SignalRService()
        {
            
            _hubConnection = new HubConnectionBuilder()
                 .WithUrl($"{App.Settings.ApiBaseUrl}/notificationHub", options =>
                 {
                     options.AccessTokenProvider = async () =>
                     {
                        
                         var token = await SecureStorage.GetAsync("token");
                         return token; 
                     };
                 })
                // .WithAutomaticReconnect()
                 .Build();
         

            _hubConnection.On<List<string>>("ReceiveMessage", (message) =>
            {
                // Add the message to the UI-bound collection
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    int notificationCounter = 0;
                    foreach (var messages in message)
                    {
                        
                        Notifications.Add(messages);
                       

                        var request = new NotificationRequest
                        {
                            NotificationId = notificationCounter++,
                            Title = "Food Saver",
                            Subtitle = "Notifications",
                            Description = messages,
                            BadgeNumber = 27,
                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = DateTime.Now.AddSeconds(1),
                            }

                        };
                        LocalNotificationCenter.Current.Show(request);
                    }
                    //Notify();
                });
                
            });

        }

        public async Task Dispose()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
               // await _hubConnection.DisposeAsync();
            }
        }

        public async void Notify()
        {
            if (_isProcessingNotifications) return;

            _isProcessingNotifications = true;
            while (Notifications.Count > 0)
            {
                string message = Notifications[0];
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
                await Task.Delay(TimeSpan.FromSeconds(5));
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (Notifications.Count > 0)
                        Notifications.RemoveAt(0);
                });
            }
            _isProcessingNotifications = false;

        }

        public async  Task ConnectToHubAsync()
        {
            try
            {
               
                await _hubConnection.StartAsync();


                //await SendNotification("da282bb3-5d60-4bee-b44e-95655de3972a", "Rose wants to buy your product.", "2dd27b90-05b9-49a3-a2d6-5271d50b6c41");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }
        }


       
        
       public async Task SendNotification(string sellerId, string message, string buyerId)
        {


            try
            {
                
                await _hubConnection.InvokeAsync("SendNotification",sellerId, message);
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
