using Events.API.Controllers;
using Events.Application.Commands.Events;
using Events.Application.Requests.Events;
using Events.Domain.Aggregates;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;

namespace Events.API.Tests.Controllers;

[TestClass]
public class EventsControllerTests
{
    private EventsController _controller;
    private Mock<IMediator> _mediatorMock;

    [TestInitialize]
    public void Initialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new EventsController(_mediatorMock.Object);
    }

    [TestMethod]
    public async Task Get_ValidQueryParameters_ShouldReturnOkResult()
    {
        // Arrange
        var category = "Category";
        var place = "Place";
        var time = "12:00:00";
        var expectedResult = new GetEventsResponse { Events = new[] { new EventBaseInfo() } };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEventsRequest>(), CancellationToken.None)).ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Get(category, place, time, CancellationToken.None) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        Assert.AreSame(expectedResult.Events, result.Value);
    }

    [TestMethod]
    public async Task Post_ShouldReturnCreatedResult()
    {
        // Arrange
        var mockHttpContext = new Mock<HttpContext>();
        var mockControllerContext = new ControllerContext
        {
            HttpContext = mockHttpContext.Object
        };
        _controller.ControllerContext = mockControllerContext;
        mockHttpContext.SetupGet(hc => hc.Request.Scheme).Returns("http");
        mockHttpContext.SetupGet(hc => hc.Request.Host).Returns(new HostString("example.com"));

        var expectedResult = new CreateEventResponse
        {
            Event = new EventDto
            {
                Uuid = Guid.NewGuid()
            }
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateEventRequest>(), CancellationToken.None)).ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Post(It.IsAny<CreateEventRequest>(), CancellationToken.None) as CreatedResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
        Assert.AreEqual(expectedResult.Event, result.Value);
    }

    [TestMethod]
    public async Task SearchEvents_ValidSearchString_ReturnsOkResult()
    {
        // Arrange
        var searchString = "test";
        var searchEventsResponse = new SearchEventsResponse
        {
            Events = new List<SearchEventDto>
            {
                    new() { Uuid = Guid.NewGuid(), Title= "Event 1" },
                    new() { Uuid = Guid.NewGuid(), Title = "Event 2" }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<SearchEventsRequest>(), CancellationToken.None))
            .ReturnsAsync(searchEventsResponse);

        // Act
        var result = await _controller.SearchEvents(searchString, CancellationToken.None) as ObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        var events = result.Value as List<SearchEventDto>;
        Assert.IsNotNull(events);
        Assert.AreEqual(2, events.Count);
    }

    [TestMethod]
    public async Task SearchEvents_NullSearchString_ReturnsOkResult()
    {
        // Arrange
        string searchString = null;
        var searchEventsResponse = new SearchEventsResponse
        {
            Events = new List<SearchEventDto>
            {
                new() { Uuid = Guid.NewGuid(), Title= "Event 1" },
                new() { Uuid = Guid.NewGuid(), Title = "Event 2" }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<SearchEventsRequest>(), CancellationToken.None))
            .ReturnsAsync(searchEventsResponse);

        // Act
        var result = await _controller.SearchEvents(searchString, CancellationToken.None) as ObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);

        var events = result.Value as List<SearchEventDto>;
        Assert.IsNotNull(events);
        Assert.AreEqual(2, events.Count);
    }
}