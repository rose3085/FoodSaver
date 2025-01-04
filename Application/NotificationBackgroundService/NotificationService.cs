using Application.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Application.NotificationBackgroundService
{
    public record Notification(string userId, string message);
    public interface INotificationSink
    {
        ValueTask PushAsync(Notification notification);
    }
    public class NotificationService : BackgroundService, INotificationSink
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationService> _logger;
        private readonly Channel<Notification> _channel;
        public NotificationService(
          IServiceProvider serviceProvider,
          ILogger<NotificationService> logger
      )
        {
            _channel = Channel.CreateUnbounded<Notification>();
            _serviceProvider = serviceProvider;
            _logger = logger;
           
        }
        public ValueTask PushAsync(Notification notification) =>  _channel.Writer.WriteAsync(notification);
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                try
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        return;
                    }

                    var (forUserId, message) = await _channel.Reader.ReadAsync(stoppingToken);

                    using var scope = _serviceProvider.CreateScope();

                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                    var payload = new { Message = message };
                    _logger.LogInformation($"Sending channel notification '{message}' to {forUserId}");
                    await hub.Clients.User(forUserId).SendAsync("Notify", payload, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error in notification service.");
                }
            }
        }
    }
}
