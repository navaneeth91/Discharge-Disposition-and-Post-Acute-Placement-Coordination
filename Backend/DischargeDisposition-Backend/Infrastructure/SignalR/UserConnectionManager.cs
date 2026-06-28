using System.Collections.Concurrent;

namespace DischargeDisposition_Backend.Infrastructure.SignalR
{
    public class UserConnectionManager
    {
        private readonly ConcurrentDictionary<int, string> _connections = new();

        /// Registers a user's SignalR connection
        public void AddConnection(int userId, string connectionId)
        {
            _connections[userId] = connectionId;
        }
        /// Removes a user's SignalR connection
        public void RemoveConnection(int userId)
        {
            _connections.TryRemove(userId, out _);
        }
        /// Returns the active connection for a user
        public string? GetConnectionId(int userId)
        {
            _connections.TryGetValue(userId, out var connectionId);

            return connectionId;
        }
    }
}