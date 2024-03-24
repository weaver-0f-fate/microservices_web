using Moq;
using Notification.Application.Commands;
using Notification.Application.Core.UseCases;

namespace Notification.Application.Tests.Commands;

[TestClass]
public class CreateEventNotificationHandlerTests
{
    [TestMethod]
    public async Task Handle_ShouldInvokeCreateEventNotification()
    {
        // Arrange
        var request = new CreateEventNotificationRequest
        {
            DestinationEmail = "test@example.com",
            EventUuid = Guid.NewGuid(),
            NotificationTime = DateTime.UtcNow
        };

        var createEventNotificationMock = new Mock<ICreateEventNotification>();
        var handler = new CreateEventNotificationHandler(createEventNotificationMock.Object);

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        createEventNotificationMock.Verify(mock => mock.InvokeAsync(It.IsAny<CreateEventNotificationInput>(), CancellationToken.None), Times.Once);
    }

    [TestMethod]
    public void CreateEventNotificationResponse_ShouldHaveNotificationUuid()
    {
        // Arrange
        var response = new CreateEventNotificationResponse();

        // Act

        // Assert
        Assert.IsNotNull(response.NotificationUuid);
        Assert.AreEqual(Guid.Empty, response.NotificationUuid);
    }
}