using Events.Application.Core.Read.Events;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Events.Domain.Specifications;
using Moq;

namespace Events.Application.Core.Tests.Read.Events;

[TestClass]
public class SearchEventsTests
{
    private SearchEvents _searchEvents = null!;
    private Mock<IRepository<Event>> _repositoryMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _repositoryMock = new Mock<IRepository<Event>>();
        _searchEvents = new SearchEvents(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task ExecuteAsync_ValidSearchString_ReturnsMatchingEvents()
    {
        // Arrange
        var searchString = "test";
        var cancellationToken = CancellationToken.None;
        var expectedEvents = new List<Event>
            {
                new() { Id = Guid.NewGuid(), Title = "Event 1" },
                new() { Id = Guid.NewGuid(), Title = "Event 2" }
            };
        
        _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<SearchEventsSpecification>(), cancellationToken))
            .ReturnsAsync(expectedEvents);

        // Act
        var result = await _searchEvents.ExecuteAsync(searchString, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        CollectionAssert.AreEqual(expectedEvents, (List<Event>)result);
    }

    [TestMethod]
    public async Task ExecuteAsync_NullSearchString_ReturnsAllEvents()
    {
        // Arrange
        string? searchString = null;
        var cancellationToken = CancellationToken.None;
        var expectedEvents = new List<Event>
            {
                new() { Id = Guid.NewGuid(), Title = "Event 1" },
                new() { Id = Guid.NewGuid(), Title = "Event 2" }
            };
        
        _repositoryMock.Setup(repo => repo.ListAsync(It.IsAny<SearchEventsSpecification>(), cancellationToken))
            .ReturnsAsync(expectedEvents);

        // Act
        var result = await _searchEvents.ExecuteAsync(searchString, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        CollectionAssert.AreEqual(expectedEvents, (List<Event>)result);
    }
}