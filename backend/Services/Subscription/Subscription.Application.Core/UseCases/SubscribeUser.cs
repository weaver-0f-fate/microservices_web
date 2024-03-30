using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;
using System.Transactions;
using Serilog;
using Subscription.Application.Core.Contracts;
using Subscription.Domain.Aggregates;
using Subscription.Domain.Aggregates.ApplicationLogs;
using Subscription.Domain.Aggregates.Base;
using Subscription.Domain.Specifications;

namespace Subscription.Application.Core.UseCases;

public interface ISubscribeUser : IUseCaseAsync<SubscribeUserInput, Guid> { }

public struct SubscribeUserInput
{
    public Guid EventId { get; set; }
    public string SubscribedEmail { get; set; }
    public DateTime NotificationTime { get; set; }
}

public class SubscribeUser(
        IRepository<Domain.Aggregates.Subscription> subscriptionRepository,
        IRepository<User> userRepository,
        IHttpClientFactory httpClientFactory,
        ILogsRepository logsRepository)
    : ISubscribeUser
{
    public async Task<Guid> InvokeAsync(SubscribeUserInput input, CancellationToken cancellationToken)
    {
        try
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetOrCreateUserAsync(input.SubscribedEmail, cancellationToken);
            var subscription = new Domain.Aggregates.Subscription(user, input.EventId);

            await subscriptionRepository.AddAsync(subscription, cancellationToken);

            await CreateNotificationAsync(input.EventId, user.Email, input.NotificationTime, cancellationToken);

            await logsRepository.CreateLogAsync(new LogRecord
            {
                AuditTime = DateTime.UtcNow,
                Action = $"User with id: {user.Id} subscribed for event with id: {input.EventId}"
            });
            await subscriptionRepository.SaveChangesAsync(cancellationToken);
            
            transactionScope.Complete();
            return subscription.Id;
        
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while subscribing a user for event: {EventId}", input.EventId);
            throw;
        }
    }

    private async Task CreateNotificationAsync(Guid eventUuid, string subscribedEmail, DateTime notificationTime, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient("NotificationHttpClient");
        
        var requestData = new
        {
            DestinationEmail = subscribedEmail,
            EventUuid = eventUuid,
            NotificationTime = notificationTime
        };
        var jsonRequestData = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");
        
        var result = await httpClient.PostAsync("api/notifications", content, cancellationToken);

        if (!result.IsSuccessStatusCode)
        {
            throw new Exception("Failed to create notification.");
        }
    }

    private async Task<User> GetOrCreateUserAsync(string subscribedEmail, CancellationToken cancellationToken)
    {
        if (!MailAddress.TryCreate(subscribedEmail, out _))
        {
            throw new ValidationException("The provided email address is not in a valid format.");
        }
        var userSpec = new UserByEmailSpecification(subscribedEmail);
        var user = await userRepository.SingleOrDefaultAsync(userSpec, cancellationToken) ?? new User(subscribedEmail);
        return user;
    }
}