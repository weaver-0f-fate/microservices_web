using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Notification.API.Controllers;
using Notification.Application.Commands;
using Notification.Application.Requests;

namespace Notification.API.Tests.Controllers;

[TestClass]
public class NotificationsControllerTests
{
    [TestMethod]
    public async Task GetNotificationAsync_WhenNotificationExists_ShouldReturn200OK()
    {
        // Arrange
        var notificationId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(It.IsAny<GetNotificationRequest>(), cancellationToken))
            .ReturnsAsync(new GetNotificationResponse
            {
                NotificationTime = DateTime.Now,
                NotificationContent = "Test Notification",
                DestinationEmail = "test@example.com",
                IsSent = true
            });

        var controller = new NotificationsController(mediatorMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
            },
        };

        // Act
        var result = await controller.GetNotificationAsync(notificationId, cancellationToken);

        // Assert
        mediatorMock.Verify(x => x.Send(It.IsAny<GetNotificationRequest>(), cancellationToken), Times.Once);

        Assert.IsNotNull(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        // Add more specific assertions based on your API response
    }

    [TestMethod]
    public async Task AddNotificationAsync_ShouldReturn201Created()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new CreateEventNotificationRequest();

        var mockHttpContext = new Mock<HttpContext>();
        var controllerContext = new ControllerContext
        {
            HttpContext = mockHttpContext.Object
        };
        mockHttpContext.SetupGet(hc => hc.Request.Scheme).Returns("http");
        mockHttpContext.SetupGet(hc => hc.Request.Host).Returns(new HostString("example.com"));


        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(request, cancellationToken))
            .ReturnsAsync(new CreateEventNotificationResponse
            {
                NotificationUuid = Guid.NewGuid()
            }); 

        var controller = new NotificationsController(mediatorMock.Object)
        {
            ControllerContext = controllerContext
        };

        // Act
        var result = await controller.AddNotificationAsync(request, cancellationToken);

        // Assert
        mediatorMock.Verify(x => x.Send(request, cancellationToken), Times.Once);

        Assert.IsNotNull(result);
        var createdResult = result as CreatedResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode);

        // Verify the Location header in the response for the created resource URI
        Assert.IsNotNull(createdResult.Location);
        Assert.IsTrue(createdResult.Location.Contains("/api/notifications/"));
    }
}