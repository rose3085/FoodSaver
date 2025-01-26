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
                 .WithUrl($"https://34bf-2405-acc0-1504-cce4-f915-d307-b1b4-24af.ngrok-free.app/notificationHub", options =>
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

            //_hubConnection.On<string>("ReceivePendingMessage", (message) =>
            //{
            //    // Add the message to the UI-bound collection
            //    MainThread.BeginInvokeOnMainThread(() =>
            //    {
            //        Notify(message);
            //    });
            //});

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
                    //Notify(message);
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

        public async Task Notify(string message)
        {
           
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
