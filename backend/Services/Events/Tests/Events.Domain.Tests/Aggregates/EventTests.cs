using Events.Domain.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Events.Domain.Tests.Aggregates;

[TestClass]
public class EventTests
{
    [TestMethod]
    public void Update_UpdatesEventProperties()
    {
        // Arrange
        var originalEvent = new Event
        {
            Title = "Original Title",
            Category = "Original Category",
            Place = "Original Place",
            Date = DateTime.Today,
            UtcTime = new TimeSpan(14, 0, 0),
            Description = "Original Description",
            AdditionalInfo = "Original Additional Info",
            ImageUrl = "Original Image Url"
        };

        var updatedEvent = new Event
        {
            Title = "Updated Title",
            Category = "Updated Category",
            Place = "Updated Place",
            Date = DateTime.Today.AddDays(1),
            UtcTime = new TimeSpan(15, 0, 0),
            Description = "Updated Description",
            AdditionalInfo = "Updated Additional Info",
            ImageUrl = "Updated Image Url"
        };

        // Act
        originalEvent.Update(updatedEvent);

        // Assert
        Assert.AreEqual(updatedEvent.Title, originalEvent.Title);
        Assert.AreEqual(updatedEvent.Category, originalEvent.Category);
        Assert.AreEqual(updatedEvent.Place, originalEvent.Place);
        Assert.AreEqual(updatedEvent.Date, originalEvent.Date);
        Assert.AreEqual(updatedEvent.UtcTime, originalEvent.UtcTime);
        Assert.AreEqual(updatedEvent.Description, originalEvent.Description);
        Assert.AreEqual(updatedEvent.AdditionalInfo, originalEvent.AdditionalInfo);
        Assert.AreEqual(updatedEvent.ImageUrl, originalEvent.ImageUrl);
    }

    [TestMethod]
    public void GetDateWithTime_ReturnsCorrectDateTime()
    {
        // Arrange
        var @event = new Event
        {
            Date = DateTime.Today,
            UtcTime = new TimeSpan(14, 30, 0)
        };

        // Act
        var result = @event.GetDateWithTime();

        // Assert
        Assert.AreEqual(DateTime.Today.Add(new TimeSpan(14, 30, 0)), result);
    }

    [TestMethod]
    public void UpdateDateTime_UpdatesDateAndTime()
    {
        // Arrange
        var @event = new Event();
        var newDateTime = DateTime.Now;

        // Act
        @event.UpdateDateTime(newDateTime);

        // Assert
        Assert.AreEqual(newDateTime.Date, @event.Date);
        Assert.AreEqual(newDateTime.ToUniversalTime().TimeOfDay, @event.UtcTime);
    }

    [TestMethod]
    public void GetDateWithTime_BothDateAndTimeAssigned_ShouldReturnCombinedDateTime()
    {
        // Arrange
        var yourObject = new Event
        {
            Date = DateTime.Parse("2023-08-26"), // Assign a DateTime
            UtcTime = TimeSpan.Parse("14:30") // Assign a TimeSpan
        };

        // Act
        var result = yourObject.GetDateWithTime();

        // Assert
        var expectedDateTime = DateTime.Parse("2023-08-26 14:30");
        Assert.AreEqual(expectedDateTime, result);
    }

    [TestMethod]
    public void GetDateWithTime_NullDate_ShouldThrowValidationException()
    {
        // Arrange
        var yourObject = new Event
        {
            Date = null, // Date is not assigned
            UtcTime = TimeSpan.Parse("14:30") // Assign a TimeSpan
        };

        // Act and Assert
        Assert.ThrowsException<ValidationException>(() => yourObject.GetDateWithTime());
    }

    [TestMethod]
    public void GetDateWithTime_NullTime_ShouldThrowValidationException()
    {
        // Arrange
        var yourObject = new Event
        {
            Date = DateTime.Parse("2023-08-26"), // Assign a DateTime
            UtcTime = null // UtcTime is not assigned
        };

        // Act and Assert
        Assert.ThrowsException<ValidationException>(() => yourObject.GetDateWithTime());
    }

    [TestMethod]
    public void GetDateWithTime_NullDateAndTime_ShouldThrowValidationException()
    {
        // Arrange
        var yourObject = new Event
        {
            Date = null, // Date is not assigned
            UtcTime = null // UtcTime is not assigned
        };

        // Act and Assert
        Assert.ThrowsException<ValidationException>(() => yourObject.GetDateWithTime());
    }
}