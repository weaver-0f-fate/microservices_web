using Moq;
using Subscription.Application.Commands;
using Subscription.Application.Core.UseCases;

namespace Subscription.Application.Tests.Commands;

[TestClass]
public class SubscribeUserHandlerTests
{
    [TestMethod]
    public async Task Handle_ValidRequest_ReturnsValidResponse()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var subscribedEmail = "test@example.com";
        var notificationTime = DateTime.UtcNow;
        var request = new SubscribeUserRequest
        {
            EventUuid = eventUuid,
            SubscribedEmail = subscribedEmail,
            NotificationTime = notificationTime
        };

        var subscriptionUuid = Guid.NewGuid();
        var subscribeUserMock = new Mock<ISubscribeUser>();
        subscribeUserMock.Setup(mock => mock.InvokeAsync(It.IsAny<SubscribeUserInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscriptionUuid);

        var handler = new SubscribeUserHandler(subscribeUserMock.Object);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.AreEqual(subscriptionUuid, response.SubscriptionUuid);
    }
}