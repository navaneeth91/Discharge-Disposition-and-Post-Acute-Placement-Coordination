namespace DischargeDisposition_Backend.Infrastructure.Notifications
{
    /// <summary>
    /// Represents a real-time notification
    /// sent through SignalR.
    /// </summary>
    public class NotificationDto
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public NotificationPriority Priority { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? PatientId { get; set; }

        public int TargetUserId { get; set; }
    }
}