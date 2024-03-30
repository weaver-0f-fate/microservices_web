using Notification.Application.Core.Contracts;
using Notification.Domain.Aggregates.Base;
using Notification.Domain.Exceptions;
using Notification.Domain.Notifications;

namespace Notification.Application.Core.Read;

public interface IGetNotification : IQuery<Guid, GetNotificationOutput> { }

public struct GetNotificationOutput
{
    public DateTime NotificationTime { get; set; }
    public string NotificationContent { get; set; }
    public string DestinationEmail { get; set; }
    public bool IsSent { get; set; }
}

public class GetNotification(IRepository<Domain.Aggregates.Notification> repository) : IGetNotification
{
    public async Task<GetNotificationOutput> ExecuteAsync(Guid eventUuid, CancellationToken cancellationToken)
    {
        var spec = new NotificationByIdSpecification(eventUuid);
        var notification = await repository.SingleOrDefaultAsync(spec, cancellationToken);

        return notification is null
            ? throw new NotFoundException($"Notification with Id {eventUuid} was not found.")
            : new GetNotificationOutput
        {
            DestinationEmail = notification.DestinationEmail,
            IsSent = notification.IsSent,
            NotificationContent = notification.NotificationContent,
            NotificationTime = notification.NotificationTime
        };
    }
}