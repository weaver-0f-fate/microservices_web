using AutoMapper;
using Events.Application.Core.DTOs;
using Events.Domain.Aggregates;

namespace Events.Application.Core.Profiles;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<UpdateEventDto, Event>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(
                dest => dest.Date,
                opt => opt.MapFrom(
                    src => ToDateTime(src.Date)))
            .ForMember(
                dest => dest.UtcTime,
                opt => opt.MapFrom(
                    src => ToTimeSpan(src.Date)))
            .ForMember(
                dest => dest.Recurrency,
                opt => opt.MapFrom(
                    src => string.IsNullOrEmpty(src.Recurrency) ? null : src.Recurrency))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }

    private static TimeSpan? ToTimeSpan(DateTimeOffset? dateTime)
    {
        if (dateTime is null)
            return null;

        var hours = dateTime.Value.Hour;
        var minutes = dateTime.Value.Minute;
        var seconds = dateTime.Value.Second;

        return new TimeSpan(hours, minutes, seconds);
    }

    private static DateTime? ToDateTime(DateTimeOffset? dateTimeOffset)
    {
        return dateTimeOffset?.Date;
    }
}