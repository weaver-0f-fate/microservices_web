using Events.Application.Core.Read.Events;
using Events.Application.Requests.Events;
using Events.Domain.Aggregates;
using Events.Domain.Exceptions;
using Moq;

namespace Events.Application.Tests.Requests.Events;

[TestClass]
public class GetEventHandlerTests
{

    [TestMethod]
    public async Task Handle_ValidRequest_ShouldReturnExpectedResponse()
    {
        // Arrange
        var getEventsMock = new Mock<IGetEvents>();
        var handler = new GetEventsHandler(getEventsMock.Object);
        var cancellationToken = CancellationToken.None;

        var request = new GetEventsRequest
        {
            Category = "Music",
            Place = "Concert Hall",
            StartDate = TimeSpan.FromHours(18)
        };

        var fakeEvents = new List<Event>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Concert",
                Date = DateTime.Now,
                UtcTime = TimeSpan.FromHours(18),
                Recurrency = "Weekly",
                Category = "Music",
                Place = "Concert Hall"
            }
        };

        getEventsMock.Setup(mock => mock.ExecuteAsync(It.IsAny<GetEventsInput>(), cancellationToken))
            .ReturnsAsync(fakeEvents);

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Events);
        Assert.AreEqual(fakeEvents.Count, response.Events.Count());
    }

    [TestMethod]
    public async Task Handle_EventExists_ReturnsResponseWithEventDto()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var eventTitle = "Test Event";
        var existingEvent = new Event
        {
            Id = eventUuid,
            Title = eventTitle,
            Category = "Test Category",
            ImageUrl = "Test ImageUrl",
            Description = "Test Description",
            Place = "Test Place",
            Date = DateTime.UtcNow,
            UtcTime = TimeSpan.FromHours(18),
            AdditionalInfo = "Test AdditionalInfo",
            Recurrency = "Test Recurrency"
        };

        var getEventMock = new Mock<IGetEvent>();
        getEventMock
            .Setup(g => g.ExecuteAsync(eventUuid, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEvent);

        var handler = new GetEventHandler(getEventMock.Object);
        var request = new GetEventRequest { Uuid = eventUuid };

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Event);
        Assert.AreEqual(eventUuid, response.Event.Uuid);
        Assert.AreEqual(eventTitle, response.Event.Title);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task Handle_EventNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();

        var getEventMock = new Mock<IGetEvent>();
        getEventMock
            .Setup(g => g.ExecuteAsync(eventUuid, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Event with uuid {eventUuid} not found."));

        var handler = new GetEventHandler(getEventMock.Object);
        var request = new GetEventRequest { Uuid = eventUuid };

        // Act & Assert
        await handler.Handle(request, CancellationToken.None);
    }
}