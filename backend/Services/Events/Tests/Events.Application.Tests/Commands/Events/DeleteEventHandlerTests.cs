using Events.Application.Commands.Events;
using Events.Application.Core.UseCases.Events;
using Moq;

namespace Events.Application.Tests.Commands.Events;

[TestClass]
public class DeleteEventHandlerTests
{
    [TestMethod]
    public async Task Handle_ValidUuid_ShouldInvokeDeleteEvent()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var deleteEventMock = new Mock<IDeleteEvent>();
        var handler = new DeleteEventHandler(deleteEventMock.Object);
        var request = new DeleteEventRequest { Uuid = uuid };
        var cancellationToken = CancellationToken.None;

        // Act
        await handler.Handle(request, cancellationToken);

        // Assert
        deleteEventMock.Verify(de => de.InvokeAsync(uuid, cancellationToken), Times.Once());
    }
}