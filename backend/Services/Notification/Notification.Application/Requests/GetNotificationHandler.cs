using MediatR;
using Notification.Application.Core.Read;

namespace Notification.Application.Requests;

public class GetNotificationHandler : IRequestHandler<GetNotificationRequest, GetNotificationResponse>
{
    private readonly IGetNotification _getNotification;

    public GetNotificationHandler(IGetNotification getNotification)
    {
        _getNotification = getNotification;
    }

    public async Task<GetNotificationResponse> Handle(GetNotificationRequest request, CancellationToken cancellationToken)
    {
        var res = await _getNotification.ExecuteAsync(request.NotificationId, cancellationToken);

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