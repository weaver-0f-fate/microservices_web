using MediatR;
using Subscription.Application.Core.UseCases;

namespace Subscription.Application.Commands;

public class SubscribeUserHandler : IRequestHandler<SubscribeUserRequest, SubscribeUserResponse>
{
    private readonly ISubscribeUser _subscribeUser;

    public SubscribeUserHandler(ISubscribeUser subscribeUser)
    {
        _subscribeUser = subscribeUser;
    }

    public async Task<SubscribeUserResponse> Handle(SubscribeUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _subscribeUser.InvokeAsync(new SubscribeUserInput
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