using Events.Application.Core.UseCases.Events;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.ApplicationLogs;
using Events.Domain.Aggregates.Base;
using Moq;

namespace Events.Application.Core.Tests.UseCases.Events;

[TestClass]
public class CreateEventTests
{
    [TestMethod]
    public async System.Threading.Tasks.Task InvokeAsync_CreatesEventAndReturnsIt()
    {
        // Arrange
        var input = new CreateEventInput
        {
            Category = "Category",
            Title = "Event Title",
            ImageUrl = "Image Url",
            Place = "Event Place",
            Description = "Event Description",
            AdditionalInfo = "Additional Info",
            Date = DateTimeOffset.Now
        };

        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Category = input.Category,
            Title = input.Title,
            ImageUrl = input.ImageUrl,
            Place = input.Place,
            Description = input.Description,
            AdditionalInfo = input.AdditionalInfo,
        };
        newEvent.UpdateDateTime(input.Date);

        var repositoryMock = new Mock<IRepository<Event>>();
        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newEvent);

        var logsRepositoryMock = new Mock<ILogsRepository>();
        logsRepositoryMock
            .Setup(r => r.CreateLogAsync(It.IsAny<LogRecord>()));

        var createEvent = new CreateEvent(repositoryMock.Object, logsRepositoryMock.Object);

        // Act
        var result = await createEvent.InvokeAsync(input, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(input.Category, result.Category);
        Assert.AreEqual(input.Title, result.Title);
        Assert.AreEqual(input.ImageUrl, result.ImageUrl);
        Assert.AreEqual(input.Place, result.Place);
        Assert.AreEqual(input.Description, result.Description);
        Assert.AreEqual(input.AdditionalInfo, result.AdditionalInfo);
        Assert.AreEqual(input.Date.Date, result.Date);
        Assert.AreEqual(input.Date.UtcDateTime.TimeOfDay, result.UtcTime);

        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}