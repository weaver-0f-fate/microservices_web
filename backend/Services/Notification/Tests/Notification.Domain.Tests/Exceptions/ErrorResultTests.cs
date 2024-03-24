using Notification.Domain.Exceptions;

namespace Notification.Domain.Tests.Exceptions;

[TestClass]
public class ErrorResultTests
{
    [TestMethod]
    public void ErrorResult_PropertiesShouldBeSetCorrectly()
    {
        // Arrange
        var messages = new List<string> { "Message1", "Message2" };
        var source = "Source";
        var exception = "Exception message";
        var errorId = "Error123";
        var supportMessage = "Support message";
        var statusCode = 404;

        // Act
        var errorResult = new ErrorResult
        {
            Messages = messages,
            Source = source,
            Exception = exception,
            ErrorId = errorId,
            SupportMessage = supportMessage,
            StatusCode = statusCode
        };

        // Assert
        Assert.AreEqual(messages, errorResult.Messages);
        Assert.AreEqual(source, errorResult.Source);
        Assert.AreEqual(exception, errorResult.Exception);
        Assert.AreEqual(errorId, errorResult.ErrorId);
        Assert.AreEqual(supportMessage, errorResult.SupportMessage);
        Assert.AreEqual(statusCode, errorResult.StatusCode);
    }
}