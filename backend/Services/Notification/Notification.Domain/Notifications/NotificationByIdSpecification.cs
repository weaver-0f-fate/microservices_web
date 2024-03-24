using Ardalis.Specification;

namespace Notification.Domain.Notifications;

public sealed class NotificationByIdSpecification : SingleResultSpecification<Aggregates.Notification>
{
    public NotificationByIdSpecification(Guid notificationId)
    {
        Query.Where(notification => notification.Id == notificationId);
    }
}