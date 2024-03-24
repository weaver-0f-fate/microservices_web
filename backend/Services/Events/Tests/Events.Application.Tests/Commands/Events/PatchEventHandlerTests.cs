using Events.Application.Commands.Events;
using Events.Application.Core.DTOs;
using Events.Application.Core.UseCases.Events;
using Microsoft.AspNetCore.JsonPatch;
using Moq;

namespace Events.Application.Tests.Commands.Events;

[TestClass]
public class PatchEventHandlerTests
{
    [TestMethod]
    public async Task Handle_ValidRequest_CallsPatchEvent()
    {
        // Arrange
        var eventUuid = Guid.NewGuid();
        var patchDoc = new JsonPatchDocument<UpdateEventDto>();
        var cancellationToken = CancellationToken.None;

        var patchEventMock = new Mock<IPatchEvent>();
        var handler = new PatchEventHandler(patchEventMock.Object);

        var request = new PatchEventRequest { EventUuid = eventUuid, PatchDoc = patchDoc };

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        patchEventMock.Verify(
            p => p.InvokeAsync(It.Is<PatchEventInput>(input =>
                    input.EventUuid == eventUuid &&
                    input.PatchDoc == patchDoc),
                cancellationToken), Times.Once);
    }
}