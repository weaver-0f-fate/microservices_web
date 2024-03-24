using AutoMapper;
using Events.Application.Core.DTOs;
using Events.Application.Core.UseCases.Events;
using Events.Domain.Aggregates;
using Events.Domain.Aggregates.ApplicationLogs;
using Events.Domain.Aggregates.Base;
using Events.Domain.Exceptions;
using Events.Domain.Specifications;
using Microsoft.AspNetCore.JsonPatch;
using Moq;

namespace Events.Application.Core.Tests.UseCases.Events;

[TestClass]
public class PatchEventTests
{
    [TestMethod]
    public async Task InvokeAsync_EventFound_PatchesAndSavesEvent()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var patchDoc = new JsonPatchDocument<UpdateEventDto>();
        var cancellationToken = CancellationToken.None;

        var existingEvent = new Event { 
            Id = eventUuid,
            Title = "Existing Title",
            Category = "Existing Category",
            Place = "Existing Place",
            Description = "Existing Description",
            Date = DateTime.UtcNow,
            UtcTime = DateTime.UtcNow.TimeOfDay,
            AdditionalInfo = "Existing Additional Info",
            ImageUrl = "Existing Image Url",
            Recurrency = "Existing Recurrency",
        };
        var updateEventDto = new UpdateEventDto { /* Set properties to apply via patch */ };
        patchDoc.Replace(e => e.Title, "Updated Title");

        var repositoryMock = new Mock<IRepository<Event>>();
        repositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<EventByUuidSpecification>(), cancellationToken))
            .ReturnsAsync(existingEvent);

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Event>(It.IsAny<UpdateEventDto>())).Returns(existingEvent); // Mapper returns the same object

        var logsRepositoryMock = new Mock<ILogsRepository>();
        logsRepositoryMock
            .Setup(r => r.CreateLogAsync(It.IsAny<LogRecord>()));

        var patchEvent = new PatchEvent(repositoryMock.Object, mapperMock.Object, logsRepositoryMock.Object);

        var input = new PatchEventInput { EventUuid = eventUuid, PatchDoc = patchDoc };

        // Act
        await patchEvent.InvokeAsync(input, cancellationToken);

        // Assert
        repositoryMock.Verify(r => r.SingleOrDefaultAsync(It.IsAny<EventByUuidSpecification>(), cancellationToken), Times.Once);
        repositoryMock.Verify(r => r.SaveChangesAsync(cancellationToken), Times.Once);
        mapperMock.Verify(m => m.Map<Event>(It.IsAny<UpdateEventDto>()), Times.Once);
    }

    [TestMethod]
    public async Task InvokeAsync_EventNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var patchDoc = new JsonPatchDocument<UpdateEventDto>();
        var cancellationToken = CancellationToken.None;

        var repositoryMock = new Mock<IRepository<Event>>();
        repositoryMock.Setup(r => r.SingleOrDefaultAsync(It.IsAny<EventByUuidSpecification>(), cancellationToken))
            .ReturnsAsync((Event)null!);

        var logsRepositoryMock = new Mock<ILogsRepository>();
        logsRepositoryMock
            .Setup(r => r.CreateLogAsync(It.IsAny<LogRecord>()));

        var mapperMock = new Mock<IMapper>();

        var patchEvent = new PatchEvent(repositoryMock.Object, mapperMock.Object, logsRepositoryMock.Object);

        var input = new PatchEventInput { EventUuid = eventUuid, PatchDoc = patchDoc };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<NotFoundException>(() => patchEvent.InvokeAsync(input, cancellationToken));
    }
}