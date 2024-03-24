namespace Notification.Domain.Tests.Aggregates;

[TestClass]
public class NotificationTests
{
    [TestMethod]
    public void Notification_Constructor_InitializesProperties()
    {
        // Arrange
        DateTime notificationTime = DateTime.UtcNow;
        string notificationContent = "Test notification";
        string destinationEmail = "test@example.com";

        // Act
        var notification = new Domain.Aggregates.Notification(notificationTime, notificationContent, destinationEmail);

        // Assert
        Assert.IsNotNull(notification.Id);
        Assert.AreEqual(notificationTime, notification.NotificationTime);
        Assert.AreEqual(notificationContent, notification.NotificationContent);
        Assert.AreEqual(destinationEmail, notification.DestinationEmail);
        Assert.IsFalse(notification.IsSent); // By default, IsSent should be false
    }

    [TestMethod]
    public void Notification_Uuid_IsGuid()
    {
        // Arrange
        DateTime notificationTime = DateTime.UtcNow;
        string notificationContent = "Test notification";
        string destinationEmail = "test@example.com";

        // Act
        var notification = new Domain.Aggregates.Notification(notificationTime, notificationContent, destinationEmail);

        // Assert
        Assert.IsTrue(Guid.TryParse(notification.Id.ToString(), out _));
    }

    [TestMethod]
    public void Notification_SetIsSent_UpdatesIsSentProperty()
    {
        // Arrange
        DateTime notificationTime = DateTime.UtcNow;
        string notificationContent = "Test notification";
        string destinationEmail = "test@example.com";
        var notification = new Domain.Aggregates.Notification(notificationTime, notificationContent, destinationEmail);

        // Act
        notification.IsSent = true;

        // Assert
        Assert.IsTrue(notification.IsSent);
    }
}