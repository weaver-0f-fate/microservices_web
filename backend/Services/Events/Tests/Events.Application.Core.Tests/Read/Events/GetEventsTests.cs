using Events.Application.Core.Read.Events;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Events.Domain.Specifications;
using Moq;

namespace Events.Application.Core.Tests.Read.Events;

[TestClass]
public class GetEventsTests
{
    [TestMethod]
    public async Task ExecuteAsync_ReturnsListOfEvents()
    {
        // Arrange
        var events = new List<Event>
        {
            new() { Id = Guid.NewGuid(), Title = "Event 1" },
            new() { Id = Guid.NewGuid(), Title = "Event 2" },
            new() { Id = Guid.NewGuid(), Title = "Event 3" }
        };

        var repositoryMock = new Mock<IRepository<Event>>();
        repositoryMock
            .Setup(r => r.ListAsync(It.IsAny<EventSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(events);

        var getEvents = new GetEvents(repositoryMock.Object);

        // Act
        var result = await getEvents.ExecuteAsync(new GetEventsInput(), CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(events.Count, result.Count());

        foreach (var expectedEvent in events)
        {
            Assert.IsTrue(result.Any(e => e.Id == expectedEvent.Id && e.Title == expectedEvent.Title));
        }
    }
}