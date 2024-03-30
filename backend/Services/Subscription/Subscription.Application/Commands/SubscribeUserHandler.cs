using MediatR;
using Subscription.Application.Core.UseCases;

namespace Subscription.Application.Commands;

public class SubscribeUserHandler(ISubscribeUser subscribeUser) : IRequestHandler<SubscribeUserRequest, SubscribeUserResponse>
{
    public async Task<SubscribeUserResponse> Handle(SubscribeUserRequest request, CancellationToken cancellationToken)
    {
        var result = await subscribeUser.InvokeAsync(new SubscribeUserInput
        {
            EventId = request.EventUuid,
            SubscribedEmail = request.SubscribedEmail,
            NotificationTime = request.NotificationTime
        }, cancellationToken);

        return new SubscribeUserResponse
        {
            SubscriptionUuid = result
        };
    }
}

public struct SubscribeUserRequest : IRequest<SubscribeUserResponse>
{
    public Guid EventUuid { get; set; }
    public string SubscribedEmail { get; set; }
    public DateTime NotificationTime { get; set; }
}

public struct SubscribeUserResponse
{
    public Guid SubscriptionUuid { get; set; }
}