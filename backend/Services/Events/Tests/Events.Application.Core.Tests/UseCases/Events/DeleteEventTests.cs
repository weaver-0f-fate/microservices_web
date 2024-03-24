using Events.Application.Core.UseCases.Events;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.ApplicationLogs;
using Events.Domain.Aggregates.Base;
using Events.Domain.Exceptions;
using Events.Domain.Specifications;
using Moq;

namespace Events.Application.Core.Tests.UseCases.Events;

[TestClass]
public class DeleteEventTests
{
    [TestMethod]
    public async System.Threading.Tasks.Task InvokeAsync_EventExists_DeletesEvent()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var existingEvent = new Event { Id = eventUuid };
        var repositoryMock = new Mock<IRepository<Event>>();
        repositoryMock
            .Setup(r => r.SingleOrDefaultAsync(It.IsAny<EventByUuidSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEvent);

        var logsRepositoryMock = new Mock<ILogsRepository>();
        logsRepositoryMock
            .Setup(r => r.CreateLogAsync(It.IsAny<LogRecord>()));

        var deleteEvent = new DeleteEvent(repositoryMock.Object, logsRepositoryMock.Object);

        // Act
        await deleteEvent.InvokeAsync(eventUuid, CancellationToken.None);

        // Assert
        repositoryMock.Verify(r => r.DeleteAsync(existingEvent, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async System.Threading.Tasks.Task InvokeAsync_EventNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var repositoryMock = new Mock<IRepository<Event>>();
        repositoryMock
            .Setup(r => r.SingleOrDefaultAsync(It.IsAny<EventByUuidSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Event)null!);

        var logsRepositoryMock = new Mock<ILogsRepository>();
        logsRepositoryMock
            .Setup(r => r.CreateLogAsync(It.IsAny<LogRecord>()));

        var deleteEvent = new DeleteEvent(repositoryMock.Object, logsRepositoryMock.Object);

        // Act & Assert
        await deleteEvent.InvokeAsync(eventUuid, CancellationToken.None);
    }
}