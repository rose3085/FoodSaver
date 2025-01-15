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
                 .WithUrl($"https://91e8-2405-acc0-1504-cce4-117a-683f-20a2-62cc.ngrok-free.app/notificationHub", options =>
                 {
                     options.AccessTokenProvider = async () =>
                     {
                        
                         var token = await SecureStorage.GetAsync("token");
                         return token; 
                     };
                 })
                 .WithAutomaticReconnect()
                 .Build();
            //Messages ??= new ObservableCollection<string>();
            _hubConnection.On<string>("ReceiveNotification", (message) =>
            {
                // Add the message to the UI-bound collection
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Notifications.Add(message);
                });
            });
            ConnectToHubAsync();
        }

        public async void Dispose()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
        }
        public async  Task ConnectToHubAsync()
        {
            try
            {
                if (_hubConnection.State != HubConnectionState.Disconnected)
                {
                    try
                    {
                        await _hubConnection.StopAsync();
                        Console.WriteLine("HubConnection stopped.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error stopping HubConnection: {ex.Message}");
                    }
                }
                await _hubConnection.StartAsync();
               
                if (Notifications.Count >0)
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
