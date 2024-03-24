using Notification.Application.Core.UseCases;

namespace Notification.Application.Core.Tests.UseCases;
[TestClass]
public class CreateEventNotificationInputTests
{
    [TestMethod]
    public void CreateEventNotificationInput_PropertiesAreSetCorrectly()
    {
        // Arrange
        var destinationEmail = "test@example.com";
        var eventUuid = Guid.NewGuid();
        var notificationTime = DateTime.UtcNow;

        // Act
        var input = new CreateEventNotificationInput
        {
            DestinationEmail = destinationEmail,
            EventUuid = eventUuid,
            NotificationTime = notificationTime
        };

        // Assert
        Assert.AreEqual(destinationEmail, input.DestinationEmail);
        Assert.AreEqual(eventUuid, input.EventUuid);
        Assert.AreEqual(notificationTime, input.NotificationTime);
    }
}