using System.Net;
using System.Text;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Notification.Application.Core.UseCases;
using Notification.Domain.Aggregates.Base;

namespace Notification.Application.Core.Tests.UseCases;


[TestClass]
public class CreateEventNotificationTests
{
    [TestMethod]
    public async Task InvokeAsync_Should_Add_Notification()
    {
        // Arrange
        var repositoryMock = new Mock<IRepository<Domain.Aggregates.Notification>>();
        
        var result = new EventInfoResponse
        {
            Title = "Test",
            Date = DateTime.UtcNow
        };

        var stringResult = JsonConvert.SerializeObject(result);
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(stringResult, Encoding.UTF8, "application/json")
        };


        var handlerMock = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(handlerMock.Object);
        var baseAddress = new Uri("https://example.com");
        httpClient.BaseAddress = baseAddress;
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();


        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response)
            .Verifiable();

        httpClientFactoryMock.Setup(x => x.CreateClient("EventsHttpClient"))
            .Returns(httpClient);

        var createEventNotification = new CreateEventNotification(repositoryMock.Object, httpClientFactoryMock.Object);
        var input = new CreateEventNotificationInput
        {
            DestinationEmail = "test@mail.com",
            EventUuid = Guid.NewGuid(),
            NotificationTime = DateTime.UtcNow
        };

        // Act
        await createEventNotification.InvokeAsync(input, CancellationToken.None);

        // Assert
        repositoryMock.Verify(x => 
            x.AddAsync(It.IsAny<Domain.Aggregates.Notification>(), CancellationToken.None), Times.Once);
        httpClientFactoryMock.Verify(x => x.CreateClient("EventsHttpClient"), Times.Once);
    }
}