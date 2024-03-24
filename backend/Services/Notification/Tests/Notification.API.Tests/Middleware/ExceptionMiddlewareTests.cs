using System.Net;
using Microsoft.AspNetCore.Http;
using Notification.API.Middleware;
using Notification.Domain.Exceptions;

namespace Notification.API.Tests.Middleware;

[TestClass]
public class ExceptionMiddlewareTests
{
    [TestMethod]
    public async Task InvokeAsync_ShouldHandleNotFoundException()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var next = new RequestDelegate(_ => throw new NotFoundException("Resource not found."));

        var middleware = new ExceptionMiddleware();

        // Act
        await middleware.InvokeAsync(context, next);

        // Assert
        Assert.AreEqual((int)HttpStatusCode.NotFound, context.Response.StatusCode);
    }

    [TestMethod]
    public async Task InvokeAsync_ShouldHandleInternalServerError()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var next = new RequestDelegate(_ => throw new Exception("Internal server error."));

        var middleware = new ExceptionMiddleware();

        // Act
        await middleware.InvokeAsync(context, next);

        // Assert
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
    }

    [TestMethod]
    public async Task InvokeAsync_ShouldWriteErrorResponse()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var next = new RequestDelegate(_ => throw new NotFoundException("Resource not found."));

        var middleware = new ExceptionMiddleware();

        // Act
        await middleware.InvokeAsync(context, next);
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();

        // Assert
        Assert.AreEqual((int)HttpStatusCode.NotFound, context.Response.StatusCode);
    }
}