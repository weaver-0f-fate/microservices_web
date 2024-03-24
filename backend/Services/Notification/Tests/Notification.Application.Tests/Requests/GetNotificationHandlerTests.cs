using Moq;
using Notification.Application.Core.Read;
using Notification.Application.Requests;
using Notification.Domain.Exceptions;

namespace Notification.Application.Tests.Requests;

[TestClass]
public class GetNotificationHandlerTests
{
    [TestMethod]
    public async Task Handle_WhenNotificationExists_ShouldReturnGetNotificationResponse()
    {
        // Arrange
        var notificationId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var getNotificationOutput = new GetNotificationOutput
        {
            DestinationEmail = "test@example.com",
            IsSent = true,
            NotificationContent = "Test Notification",
            NotificationTime = DateTime.Now
        };

        var getNotificationMock = new Mock<IGetNotification>();
        getNotificationMock
            .Setup(x => x.ExecuteAsync(notificationId, cancellationToken))
            .ReturnsAsync(getNotificationOutput);

        var handler = new GetNotificationHandler(getNotificationMock.Object);
        var request = new GetNotificationRequest { NotificationId = notificationId };

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        getNotificationMock.Verify(x => x.ExecuteAsync(notificationId, cancellationToken), Times.Once);

        Assert.IsNotNull(response);
        Assert.AreEqual(getNotificationOutput.DestinationEmail, response.DestinationEmail);
        Assert.AreEqual(getNotificationOutput.IsSent, response.IsSent);
        Assert.AreEqual(getNotificationOutput.NotificationContent, response.NotificationContent);
        Assert.AreEqual(getNotificationOutput.NotificationTime, response.NotificationTime);
    }

    [TestMethod]
    public async Task Handle_WhenNotificationDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var notificationId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var getNotificationMock = new Mock<IGetNotification>();
        getNotificationMock
            .Setup(x => x.ExecuteAsync(notificationId, cancellationToken))
            .ThrowsAsync(new NotFoundException("Notification not found"));

        var handler = new GetNotificationHandler(getNotificationMock.Object);
        var request = new GetNotificationRequest { NotificationId = notificationId };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            var response = await handler.Handle(request, cancellationToken);
        });

        getNotificationMock.Verify(x => x.ExecuteAsync(notificationId, cancellationToken), Times.Once);
    }
}