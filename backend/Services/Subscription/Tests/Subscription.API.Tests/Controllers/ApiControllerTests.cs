using MediatR;
using Moq;
using Subscription.API.Controllers;

namespace Subscription.API.Tests.Controllers;

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