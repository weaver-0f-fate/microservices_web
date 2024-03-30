using Newtonsoft.Json;
using Notification.Application.Core.Contracts;
using Notification.Domain.Aggregates.Base;
using System.Transactions;
using Serilog;

namespace Notification.Application.Core.UseCases;

public interface ICreateEventNotification : IUseCaseAsync<CreateEventNotificationInput> { }

public struct CreateEventNotificationInput
{
    public string DestinationEmail { get; set; }
    public Guid EventUuid { get; set; }
    public DateTime NotificationTime { get; set; }
}

public class CreateEventNotification(
        IRepository<Domain.Aggregates.Notification> notificationRepository,
        IHttpClientFactory httpClientFactory)
    : ICreateEventNotification
{
    public async Task InvokeAsync(CreateEventNotificationInput input, CancellationToken cancellationToken)
    {
        try
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var eventInfo = await GetEventInfo(input.EventUuid, cancellationToken);

            var notificationMessage = 
                $"Event {eventInfo.Title} is going to start at {eventInfo.Date}";

            var notification = new Domain.Aggregates.Notification(
                input.NotificationTime,
                notificationMessage,
                input.DestinationEmail);

            await notificationRepository.AddAsync(notification, cancellationToken);
            transactionScope.Complete();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while creating notification for event with id: {EventUuid}", input.EventUuid);
            throw;
        }
    }

    private async Task<EventInfoResponse> GetEventInfo(Guid eventUuid, CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient("EventsHttpClient");

        var response = await httpClient.GetAsync($"api/events/{eventUuid}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get event info.");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseObject = JsonConvert.DeserializeObject<EventInfoResponse>(jsonResponse);

        return new EventInfoResponse
        {
            Title = responseObject.Title,
            Date = responseObject.Date
        };
    }
}

public struct EventInfoResponse
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
}