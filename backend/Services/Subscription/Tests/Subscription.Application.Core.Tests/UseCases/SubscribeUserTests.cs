using System.Net;
using Moq;
using Moq.Protected;
using Subscription.Application.Core.UseCases;
using Subscription.Domain.Aggregates;
using Subscription.Domain.Aggregates.ApplicationLogs;
using Subscription.Domain.Aggregates.Base;
using Subscription.Domain.Specifications;

namespace Subscription.Application.Core.Tests.UseCases;

[TestClass]
public class SubscribeUserTests
{
    [TestMethod]
    public async Task InvokeAsync_Should_SubscribeUser()
    {
        // Arrange
        var subscriptionRepository = new Mock<IRepository<Domain.Aggregates.Subscription>>();
        var userRepository = new Mock<IRepository<User>>();

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
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created))
            .Verifiable();

        httpClientFactoryMock.Setup(x => x.CreateClient("NotificationHttpClient"))
            .Returns(httpClient);

        var logsRepositoryMock = new Mock<ILogsRepository>();
        logsRepositoryMock
            .Setup(r => r.CreateLogAsync(It.IsAny<LogRecord>()));

        var subscribeUser = new SubscribeUser(
            subscriptionRepository.Object, 
            userRepository.Object,
            httpClientFactoryMock.Object,
            logsRepositoryMock.Object);
        
        var input = new SubscribeUserInput
        {
            EventId = Guid.NewGuid(),
            SubscribedEmail = "Test@mail.com",
            NotificationTime = DateTime.UtcNow
        };

        // Act

        var result = await subscribeUser.InvokeAsync(input, CancellationToken.None);

        // Assert
        subscriptionRepository.Verify(x =>
            x.AddAsync(It.IsAny<Domain.Aggregates.Subscription>(), CancellationToken.None), Times.Once);
        subscriptionRepository.Verify(x =>
            x.SaveChangesAsync(CancellationToken.None), Times.Once);
        userRepository.Verify(x =>
            x.SingleOrDefaultAsync(It.IsAny<UserByEmailSpecification>(), CancellationToken.None), Times.Once);
        httpClientFactoryMock.Verify(x => x.CreateClient("NotificationHttpClient"), Times.Once);
        Assert.IsNotNull(result);
    }
}