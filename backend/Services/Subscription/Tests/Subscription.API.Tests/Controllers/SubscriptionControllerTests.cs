using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Subscription.API.Controllers;
using Subscription.Application.Commands;

namespace Subscription.API.Tests.Controllers;


[TestClass]
public class SubscriptionControllerTests
{
    [TestMethod]
    public async Task SubscribeUserAsync_ValidRequest_Returns201Created()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var mockHttpContext = new Mock<HttpContext>();
        var controllerContext = new ControllerContext
        {
            HttpContext = mockHttpContext.Object
        };
        mockHttpContext.SetupGet(hc => hc.Request.Scheme).Returns("http");
        mockHttpContext.SetupGet(hc => hc.Request.Host).Returns(new HostString("example.com"));


        var controller = new SubscriptionController(mediatorMock.Object)
        {
            ControllerContext = controllerContext
        };

        var cancellationToken = CancellationToken.None;

        var subscribeUserRequest = new SubscribeUserRequest
        {
            EventUuid = Guid.NewGuid(),
            NotificationTime = DateTime.Now,
            SubscribedEmail = "test@email.com"
        };

        var expectedResult = new SubscribeUserResponse
        {
            // Initialize expected response properties here
            SubscriptionUuid = Guid.NewGuid()
        };

        mediatorMock.Setup(m => m.Send(It.IsAny<SubscribeUserRequest>(), cancellationToken))
                    .ReturnsAsync(expectedResult);

        // Act
        var result = await controller.SubscribeUserAsync(subscribeUserRequest, cancellationToken);

        // Assert
        Assert.IsInstanceOfType(result, typeof(CreatedResult));

        var createdResult = (CreatedResult)result;
        Assert.AreEqual(StatusCodes.Status201Created, createdResult.StatusCode);

        var responseValue = (SubscribeUserResponse)createdResult.Value!;
        Assert.IsNotNull(responseValue);
        Assert.AreEqual(expectedResult.SubscriptionUuid, responseValue.SubscriptionUuid);

        // Verify the Mediator.Send method was called
        mediatorMock.Verify(m => m.Send(It.Is<SubscribeUserRequest>(request =>
            // Add assertions for request properties if needed
            request.EventUuid == subscribeUserRequest.EventUuid &&
            request.NotificationTime == subscribeUserRequest.NotificationTime &&
            request.SubscribedEmail == subscribeUserRequest.SubscribedEmail 
        ), cancellationToken), Times.Once);

        // Verify the constructed resource URI
        var expectedUri = new Uri($"{controllerContext.HttpContext.Request.Scheme}://{controllerContext.HttpContext.Request.Host}/api/subscription/{expectedResult.SubscriptionUuid}");
        Assert.AreEqual(expectedUri.AbsoluteUri, createdResult.Location);
    }
}