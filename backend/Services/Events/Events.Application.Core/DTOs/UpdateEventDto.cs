using Events.Domain.Aggregates;

namespace Events.Application.Core.DTOs;

public class UpdateEventDto
{
    public string? Category { get; set; }
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public string? Place { get; set; }
    public string? Description { get; set; }
    public string? AdditionalInfo { get; set; }
    public DateTimeOffset? Date { get; set; }
    public string? Recurrency { get; set; }


    public UpdateEventDto(){}

    public UpdateEventDto(Event @event)
    {
        Category = @event.Category;
        Title = @event.Title;
        ImageUrl = @event.ImageUrl;
        Place = @event.Place;
        Description = @event.Description;
        AdditionalInfo = @event.AdditionalInfo;
        Date = @event.Date;
        Recurrency = @event.Recurrency;
    }
}