using Events.Application.Core.Read.Events;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.Base;
using Events.Domain.Exceptions;
using Events.Domain.Specifications;
using Moq;

namespace Events.Application.Core.Tests.Read.Events;

[TestClass]
public class GetEventTests
{
    [TestMethod]
    public async Task ExecuteAsync_EventExists_ReturnsEvent()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var expectedEvent = new Event { Id = eventUuid };
        var repositoryMock = new Mock<IRepository<Event>>();
        repositoryMock
            .Setup(r => r.SingleOrDefaultAsync(It.IsAny<EventByUuidSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEvent);

        var getEvent = new GetEvent(repositoryMock.Object);

        // Act
        var result = await getEvent.ExecuteAsync(eventUuid, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(eventUuid, result.Id);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task ExecuteAsync_EventNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var repositoryMock = new Mock<IRepository<Event>>();
        repositoryMock
            .Setup(r => r.SingleOrDefaultAsync(It.IsAny<EventByUuidSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        var getEvent = new GetEvent(repositoryMock.Object);

        // Act & Assert
        await getEvent.ExecuteAsync(eventUuid, CancellationToken.None);
    }
}