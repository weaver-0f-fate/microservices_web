using MediatR;
using Notification.Application.Core.UseCases;

namespace Notification.Application.Commands;

public class CreateEventNotificationHandler(ICreateEventNotification createEventNotification) : IRequestHandler<CreateEventNotificationRequest, CreateEventNotificationResponse>
{
    public async Task<CreateEventNotificationResponse> Handle(CreateEventNotificationRequest request, CancellationToken cancellationToken)
    {
        await createEventNotification.InvokeAsync(new CreateEventNotificationInput
        {
            DestinationEmail = request.DestinationEmail,
            EventUuid = request.EventUuid,
            NotificationTime = request.NotificationTime
        }, cancellationToken);

        return new CreateEventNotificationResponse();
    }
}

public struct CreateEventNotificationRequest : IRequest<CreateEventNotificationResponse>
{
    public string DestinationEmail { get; set; }
    public Guid EventUuid { get; set; }
    public DateTime NotificationTime { get; set; }
}

public struct CreateEventNotificationResponse
{
    public Guid NotificationUuid { get; set; }
}