using Events.Application.Core.Read.Events;
using Events.Application.Requests.Events;
using Events.Domain.Aggregates;
using Moq;

namespace Events.Application.Tests.Requests.Events;

[TestClass]
public class GetEventsHandlerTests
{
    [TestMethod]
    public async Task Handle_ReturnsResponseWithEventBaseInfos()
    {
        // Arrange
        var events = new List<Event>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Event 1",
                Date = DateTime.UtcNow,
                UtcTime = TimeSpan.FromHours(0),
                Recurrency = "Rrule 1",
                Category = "Category 1",
                Place = "Place 1"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Event 2",
                Date = DateTime.UtcNow.AddHours(1),
                UtcTime = TimeSpan.FromHours(0),
                Recurrency = "Rrule 2",
                Category = "Category 2",
                Place = "Place 2"
            }
        };

        var getEventsMock = new Mock<IGetEvents>();
        getEventsMock
            .Setup(g => g.ExecuteAsync(new GetEventsInput(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(events);

        var handler = new GetEventsHandler(getEventsMock.Object);
        var request = new GetEventsRequest();

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Events);
        Assert.AreEqual(events.Count, response.Events.Count());

        // Assert the first EventBaseInfo in the response matches the first Event in the events list
        var firstEventInfo = response.Events.First();
        var firstEvent = events.First();

        Assert.AreEqual(firstEvent.Id, firstEventInfo.Uuid);
        Assert.AreEqual(firstEvent.Title, firstEventInfo.Title);
        Assert.AreEqual(firstEvent.Date, firstEventInfo.Date);
        Assert.AreEqual(firstEvent.Recurrency, firstEventInfo.Rrule);
        Assert.AreEqual(firstEvent.Category, firstEventInfo.Category);
        Assert.AreEqual(firstEvent.Place, firstEventInfo.Place);
    }

    [TestMethod]
    public async Task Handle_NoEvents_ReturnsResponseWithEmptyEventBaseInfos()
    {
        // Arrange
        var getEventsMock = new Mock<IGetEvents>();
        getEventsMock
            .Setup(g => g.ExecuteAsync(new GetEventsInput(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Event>());

        var handler = new GetEventsHandler(getEventsMock.Object);
        var request = new GetEventsRequest();

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Events);
        Assert.IsFalse(response.Events.Any());
    }
}