using Moq;
using Notification.Domain.Notifications;
using Notification.Application.Core.Read;
using Notification.Domain.Aggregates.Base;
using Notification.Domain.Exceptions;

namespace Notification.Application.Core.Tests.Read;

[TestClass]
public class GetNotificationTests
{
    [TestMethod]
    public async Task ExecuteAsync_WhenNotificationExists_ShouldReturnGetNotificationOutput()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var notification = new Domain.Aggregates.Notification(
            DateTime.Now, 
            "Test Notification", 
            "test@example.com");

        var repositoryMock = new Mock<IRepository<Domain.Aggregates.Notification>>();
        repositoryMock
            .Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<NotificationByIdSpecification>(), cancellationToken))
            .ReturnsAsync(notification);

        var getNotification = new GetNotification(repositoryMock.Object);

        // Act
        var result = await getNotification.ExecuteAsync(eventUuid, cancellationToken);

        // Assert
        repositoryMock.Verify(repo => repo.SingleOrDefaultAsync(It.IsAny<NotificationByIdSpecification>(), cancellationToken), Times.Once);

        Assert.IsNotNull(result);
        Assert.AreEqual(notification.DestinationEmail, result.DestinationEmail);
        Assert.AreEqual(notification.IsSent, result.IsSent);
        Assert.AreEqual(notification.NotificationContent, result.NotificationContent);
        Assert.AreEqual(notification.NotificationTime, result.NotificationTime);
    }

    [TestMethod]
    public async Task ExecuteAsync_WhenNotificationDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var repositoryMock = new Mock<IRepository<Domain.Aggregates.Notification>>();
        repositoryMock
            .Setup(repo => repo.SingleOrDefaultAsync(It.IsAny<NotificationByIdSpecification>(), cancellationToken))
            .ReturnsAsync((Domain.Aggregates.Notification)null!);

        var getNotification = new GetNotification(repositoryMock.Object);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            var result = await getNotification.ExecuteAsync(eventUuid, cancellationToken);
        });

        repositoryMock.Verify(repo => repo.SingleOrDefaultAsync(It.IsAny<NotificationByIdSpecification>(), cancellationToken), Times.Once);
    }
}


[TestClass]
public class GetNotificationOutputTests
{
    [TestMethod]
    public void DefaultConstructor_ShouldInitializePropertiesWithDefaultValues()
    {
        // Arrange & Act
        var notificationOutput = new GetNotificationOutput();

        // Assert
        Assert.AreEqual(default, notificationOutput.NotificationTime);
        Assert.IsNull(notificationOutput.NotificationContent);
        Assert.IsNull(notificationOutput.DestinationEmail);
        Assert.IsFalse(notificationOutput.IsSent);
    }

    [TestMethod]
    public void ParameterizedConstructor_ShouldInitializePropertiesWithProvidedValues()
    {
        // Arrange
        var notificationTime = DateTime.Now;
        var notificationContent = "Test Notification";
        var destinationEmail = "test@example.com";
        var isSent = true;

        // Act
        var notificationOutput = new GetNotificationOutput
        {
            NotificationTime = notificationTime,
            NotificationContent = notificationContent,
            DestinationEmail = destinationEmail,
            IsSent = isSent
        };

        // Assert
        Assert.AreEqual(notificationTime, notificationOutput.NotificationTime);
        Assert.AreEqual(notificationContent, notificationOutput.NotificationContent);
        Assert.AreEqual(destinationEmail, notificationOutput.DestinationEmail);
        Assert.AreEqual(isSent, notificationOutput.IsSent);
    }

    [TestMethod]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var notificationOutput = new GetNotificationOutput
        {
            // Act
            NotificationTime = DateTime.Now,
            NotificationContent = "Test Notification",
            DestinationEmail = "test@example.com",
            IsSent = true
        };

        // Assert
        Assert.IsNotNull(notificationOutput.NotificationTime);
        Assert.IsNotNull(notificationOutput.NotificationContent);
        Assert.IsNotNull(notificationOutput.DestinationEmail);
        Assert.IsTrue(notificationOutput.IsSent);
    }
}