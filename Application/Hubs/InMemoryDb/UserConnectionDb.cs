using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Hubs.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Hubs.InMemoryDb
{
    public class UserConnectionDb
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connections = new();

        public ConcurrentDictionary<string, UserConnection> connections => _connections;

         public void AddConnection(string userId, UserConnection userConnection)
        {
            _connections[userId] = userConnection;
        }



        public bool RemoveConnection(string connectionId)
        {
            return _connections.TryRemove(connectionId, out _);
        }

        public UserConnection? GetConnection(string userId)
        {
            _connections.TryGetValue(userId, out var userConnection);
            return userConnection;
        }


        public UserConnection GetByUserId(string userId)
        {
            var result = _connections.TryGetValue(userId, out var userConnection);
            return userConnection;
        }
    }
}
