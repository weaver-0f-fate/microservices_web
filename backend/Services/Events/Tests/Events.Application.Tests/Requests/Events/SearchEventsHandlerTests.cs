using Events.Application.Core.Read.Events;
using Events.Application.Requests.Events;
using Events.Domain.Aggregates;
using Moq;

namespace Events.Application.Tests.Requests.Events;

[TestClass]
public class SearchEventsHandlerTests
{
    private SearchEventsHandler _handler;
    private Mock<ISearchEvents> _searchEventsMock;

    [TestInitialize]
    public void Initialize()
    {
        _searchEventsMock = new Mock<ISearchEvents>();
        _handler = new SearchEventsHandler(_searchEventsMock.Object);
    }

    [TestMethod]
    public async Task Handle_ValidRequest_ReturnsSearchEventsResponse()
    {
        // Arrange
        var searchString = "test";
        var cancellationToken = CancellationToken.None;

        var searchEventDtos = new List<Event>
            {
                new() { Id = Guid.NewGuid(), Title = "Event 1", Description = "Description 1", Place = "Place 1" },
                new() { Id = Guid.NewGuid(), Title = "Event 2", Description = "Description 2", Place = "Place 2" }
            };

        _searchEventsMock.Setup(se => se.ExecuteAsync(searchString, cancellationToken))
            .ReturnsAsync(searchEventDtos);

        var request = new SearchEventsRequest { SearchString = searchString };

        // Act
        var response = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsInstanceOfType(response, typeof(SearchEventsResponse));

        var searchEventResponses = response.Events.ToList();
        Assert.AreEqual(searchEventDtos.Count, searchEventResponses.Count);

        for (var i = 0; i < searchEventDtos.Count; i++)
        {
            Assert.AreEqual(searchEventDtos[i].Id, searchEventResponses[i].Uuid);
            Assert.AreEqual(searchEventDtos[i].Title, searchEventResponses[i].Title);
            Assert.AreEqual(searchEventDtos[i].Description, searchEventResponses[i].Description);
            Assert.AreEqual(searchEventDtos[i].Place, searchEventResponses[i].Place);
        }
    }
}
