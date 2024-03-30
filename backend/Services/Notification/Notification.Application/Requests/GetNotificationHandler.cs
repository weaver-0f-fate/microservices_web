using MediatR;
using Notification.Application.Core.Read;

namespace Notification.Application.Requests;

public class GetNotificationHandler(IGetNotification getNotification) : IRequestHandler<GetNotificationRequest, GetNotificationResponse>
{
    public async Task<GetNotificationResponse> Handle(GetNotificationRequest request, CancellationToken cancellationToken)
    {
        var res = await getNotification.ExecuteAsync(request.NotificationId, cancellationToken);

        return new GetNotificationResponse
        {
            DestinationEmail = res.DestinationEmail,
            IsSent = res.IsSent,
            NotificationContent = res.NotificationContent,
            NotificationTime = res.NotificationTime
        };
    }
}

public struct GetNotificationRequest : IRequest<GetNotificationResponse>
{
    public Guid NotificationId { get; set; }
}

public struct GetNotificationResponse
{
    public DateTime NotificationTime { get; set; }
    public string NotificationContent { get; set; }
    public string DestinationEmail { get; set; }
    public bool IsSent { get; set; }
}