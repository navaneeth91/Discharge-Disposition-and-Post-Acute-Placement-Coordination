using DischargeDisposition_Backend.Infrastructure.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace DischargeDisposition_Backend.Infrastructure.SignalR
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserConnectionManager _connectionManager;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IHubContext<NotificationHub> hubContext,
            UserConnectionManager connectionManager,
            ILogger<NotificationService> logger)
        {
            _hubContext = hubContext;
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task SendToUserAsync(
            NotificationDto notification)
        {
            var connectionId =
                _connectionManager.GetConnectionId(
                    notification.TargetUserId);

            if (string.IsNullOrWhiteSpace(connectionId))
            {
                _logger.LogInformation(
                    "User {UserId} is offline. Notification skipped.",
                    notification.TargetUserId);

                return;
            }

            await _hubContext.Clients
                .Client(connectionId)
                .SendAsync(
                    "ReceiveNotification",
                    notification);

            _logger.LogInformation(
                "Notification sent successfully to User {UserId}.",
                notification.TargetUserId);
        }

        public async Task RefreshAssignments()
        {
            await _hubContext.Clients
                .All
                .SendAsync("RefreshAssignments");

            _logger.LogInformation(
                "RefreshAssignments event sent.");
        }

        public async Task RefreshDashboard()
        {
            await _hubContext.Clients
                .All
                .SendAsync("RefreshDashboard");

            _logger.LogInformation(
                "RefreshDashboard event sent.");
        }
        public async Task RefreshReferrals()
        {
            await _hubContext.Clients
                .All
                .SendAsync("RefreshReferrals");
        }
        public async Task RefreshAuthorizations()
        {
            await _hubContext.Clients
                .All
                .SendAsync("RefreshAuthorizations");
        }
    }
}