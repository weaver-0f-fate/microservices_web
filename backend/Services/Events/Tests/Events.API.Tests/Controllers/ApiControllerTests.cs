using Events.API.Controllers;
using MediatR;
using Moq;

namespace Events.API.Tests.Controllers;

[TestClass]
public class ApiControllerTests
{
    [TestMethod]
    public void ApiController_Constructor_ShouldAssignMediator()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();

        // Act
        var controller = new ApiController(mediatorMock.Object);

        // Assert
        Assert.IsNotNull(controller.Mediator);
        Assert.AreSame(mediatorMock.Object, controller.Mediator);
    }
}