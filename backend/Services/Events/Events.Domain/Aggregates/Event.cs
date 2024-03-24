using System.ComponentModel.DataAnnotations;
using Events.Domain.Aggregates.Base;

namespace Events.Domain.Aggregates;

public class Event : EntityBase, IAggregateRoot
{
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? Date { get; set; }

    public TimeSpan? UtcTime { get; set; }
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Additional Info")] public string AdditionalInfo { get; set; } = string.Empty;

    [Display(Name = "Image Url")] public string ImageUrl { get; set; } = string.Empty;
    public string? Recurrency { get; set; }

    public Event() { }

    public void Update(Event @event)
    {
        Title = @event.Title;
        Category = @event.Category;
        Place = @event.Place;
        Date = @event.Date;
        UtcTime = @event.UtcTime;
        Description = @event.Description;
        AdditionalInfo = @event.AdditionalInfo;
        ImageUrl = @event.ImageUrl;
        Recurrency = @event.Recurrency;
    }


    public DateTime GetDateWithTime()
    {
        if (Date is null || UtcTime is null)
            throw new ValidationException("Date or UtcTime is not assigned.");

        return Date.Value.Add(UtcTime.Value);
    }

    public void UpdateDateTime(DateTimeOffset dateTime)
    {
        Date = dateTime.Date;
        UtcTime = dateTime.UtcDateTime.TimeOfDay;
    }
}