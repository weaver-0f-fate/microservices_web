using Events.Application.Commands.Events;
using Events.Application.Core.UseCases.Events;
using Events.Domain.Aggregates;
using Moq;

namespace Events.Application.Tests.Commands.Events;

[TestClass]
public class CreateEventHandlerTests
{
    [TestMethod]
    public async Task Handle_ReturnsResponseWithEventDto()
    {
        // Arrange
        var defaultEvent = new Event
        {
            Id = Guid.NewGuid(),
            Category = "Default",
            Title = "Default",
            ImageUrl = "Default",
            Place = "Default",
            Description = "Default",
            AdditionalInfo = "Default",
            Date = DateTime.Now.Date,
            UtcTime = DateTime.Now.TimeOfDay
        };

        var createEventMock = new Mock<ICreateEvent>();
        createEventMock
            .Setup(c => c.InvokeAsync(It.IsAny<CreateEventInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(defaultEvent);

        var handler = new CreateEventHandler(createEventMock.Object);
        var request = new CreateEventRequest();

        // Act
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Event);

        Assert.AreEqual(defaultEvent.Id, response.Event.Uuid);
        Assert.AreEqual(defaultEvent.Category, response.Event.Category);
        Assert.AreEqual(defaultEvent.Title, response.Event.Title);
        Assert.AreEqual(defaultEvent.ImageUrl, response.Event.ImageUrl);
        Assert.AreEqual(defaultEvent.Description, response.Event.Description);
        Assert.AreEqual(defaultEvent.Place, response.Event.Place);
        Assert.AreEqual(defaultEvent.Date, response.Event.Date.Date);
        Assert.AreEqual(defaultEvent.UtcTime, response.Event.Date.TimeOfDay);
        Assert.AreEqual(defaultEvent.AdditionalInfo, response.Event.AdditionalInfo);

        createEventMock.Verify(c => c.InvokeAsync(It.IsAny<CreateEventInput>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}