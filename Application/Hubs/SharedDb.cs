using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Hubs
{
    public class SharedDb
    {
        private readonly ConcurrentDictionary<string, UserConnection> _connections = new();

        public ConcurrentDictionary<string, UserConnection> connections => _connections;

        public void AddConnection(string connectionId, UserConnection userConnection)
        {
            _connections[connectionId] = userConnection;
        }

        public bool RemoveConnection(string connectionId)
        {
            return _connections.TryRemove(connectionId, out _);
        }

        public UserConnection? GetConnection(string connectionId)
        {
            _connections.TryGetValue(connectionId, out var userConnection);
            return userConnection;
        }


        public  UserConnection GetByUserId(string userId)
        {
            return _connections
               .Where(kvp => kvp.Value.UserId == userId)
               .Select(kvp => kvp.Value)
               .FirstOrDefault();
        }
    }
}
