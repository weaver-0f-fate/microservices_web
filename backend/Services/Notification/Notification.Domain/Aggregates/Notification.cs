using Notification.Domain.Aggregates.Base;

namespace Notification.Domain.Aggregates;

public class Notification : EntityBase, IAggregateRoot
{
    public DateTime NotificationTime { get; set; }
    public string NotificationContent { get; set; } = string.Empty;
    public string DestinationEmail { get; set; } = string.Empty;
    public bool IsSent { get; set; }

    private Notification()
    {
        Id = Guid.NewGuid();
    }

    public Notification(DateTime notificationTime, string notificationContent, string destinationEmail) : this()
    {
        NotificationTime = notificationTime;
        NotificationContent = notificationContent;
        DestinationEmail = destinationEmail;
    }
}