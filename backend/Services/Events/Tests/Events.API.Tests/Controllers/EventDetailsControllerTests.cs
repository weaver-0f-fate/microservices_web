namespace Events.API.Tests.Controllers;

using System;
using System.Threading;
using System.Threading.Tasks;
using Events.API.Controllers;
using Events.Application.Commands.Events;
using Events.Application.Core.DTOs;
using Events.Application.Requests.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class EventDetailsControllerTests
{
    private readonly EventDetailsController _controller;
    private readonly Mock<IMediator> _mediatorMock;

    public EventDetailsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new EventDetailsController(_mediatorMock.Object);
        
    }

    [TestMethod]
    public async Task Get_ValidUuid_ShouldReturnOkResult()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var expectedResult = new GetEventResponse { Event = new EventDto() };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetEventRequest>(), CancellationToken.None)).ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.Get(uuid, CancellationToken.None) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        Assert.AreEqual(expectedResult.Event, result.Value);
    }

    [TestMethod]
    public async Task Delete_ValidUuid_ShouldReturnNoContentResult()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteEventRequest>(), CancellationToken.None)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(uuid, CancellationToken.None) as NoContentResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status204NoContent, result.StatusCode);
    }

    [TestMethod]
    public async Task Patch_ValidData_ShouldReturnNoContentResult()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        var patchDoc = new JsonPatchDocument<UpdateEventDto>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<PatchEventRequest>(), CancellationToken.None)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Patch(uuid, patchDoc, CancellationToken.None) as NoContentResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status204NoContent, result.StatusCode);
    }

    [TestMethod]
    public async Task Patch_NullPatchDoc_ShouldReturnBadRequestResult()
    {
        // Arrange
        var uuid = Guid.NewGuid();
        JsonPatchDocument<UpdateEventDto>? patchDoc = null;

        // Act
        var result = await _controller.Patch(uuid, patchDoc, CancellationToken.None) as BadRequestResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
    }
}
