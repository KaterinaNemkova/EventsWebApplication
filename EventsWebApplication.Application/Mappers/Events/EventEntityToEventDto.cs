using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Core.Entities;


namespace EventsWebApplication.Application.Mappers.Events
{
    public class EventEntityToEventDto : Profile
    {
        public EventEntityToEventDto()
        {
            CreateMap<EventEntity, EventDto>().ReverseMap();
        }
    }
}
