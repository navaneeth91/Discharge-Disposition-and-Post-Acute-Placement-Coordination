using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DischargeDisposition_Backend.Infrastructure.SignalR
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly UserConnectionManager _connectionManager;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(
            UserConnectionManager connectionManager,ILogger<NotificationHub> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userIdClaim =
                Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                _connectionManager.AddConnection(
                    userId,
                    Context.ConnectionId);
            }

            await base.OnConnectedAsync();
            _logger.LogInformation("User {UserId} connected with ConnectionId {ConnectionId}",
                userId,
                Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(
            Exception? exception)
        {
            var userIdClaim =
                Context.User?.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                _connectionManager.RemoveConnection(userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}