using Application.Hubs.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Hubs.InMemoryDb
{
    public class NotificationDb
    {
        private readonly ConcurrentDictionary<string, NotificationDto> _notifications = new();

        public ConcurrentDictionary<string, NotificationDto> notifications => _notifications;

        public void AddNotification(string userId, NotificationDto notificationDto)
        {
            _notifications[userId] = notificationDto;
        }

        public NotificationDto GetByUserId(string userId)
        {
            var result = _notifications.TryGetValue(userId, out var notificationDto);
            return notificationDto;
        }
        public bool RemoveNotification(string sellerId)
        {
            return _notifications.TryRemove(sellerId, out _);
        }
    }
}
