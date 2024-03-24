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

public class SubscribeUser : ISubscribeUser
{
    private readonly IRepository<Domain.Aggregates.Subscription> _subscriptionRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogsRepository _logsRepository;

    public SubscribeUser(
        IRepository<Domain.Aggregates.Subscription> subscriptionRepository,
        IRepository<User> userRepository,
        IHttpClientFactory httpClientFactory,
        ILogsRepository logsRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _userRepository = userRepository;
        _httpClientFactory = httpClientFactory;
        _logsRepository = logsRepository;
    }

    public async Task<Guid> InvokeAsync(SubscribeUserInput input, CancellationToken cancellationToken)
    {
        try
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetOrCreateUserAsync(input.SubscribedEmail, cancellationToken);
            var subscription = new Domain.Aggregates.Subscription(user, input.EventId);

            await _subscriptionRepository.AddAsync(subscription, cancellationToken);

            await CreateNotificationAsync(input.EventId, user.Email, input.NotificationTime, cancellationToken);

            await _logsRepository.CreateLogAsync(new LogRecord
            {
                AuditTime = DateTime.UtcNow,
                Action = $"User with id: {user.Id} subscribed for event with id: {input.EventId}"
            });
            await _subscriptionRepository.SaveChangesAsync(cancellationToken);
            
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
        var httpClient = _httpClientFactory.CreateClient("NotificationHttpClient");
        
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
        var user = await _userRepository.SingleOrDefaultAsync(userSpec, cancellationToken) ?? new User(subscribedEmail);
        return user;
    }
}