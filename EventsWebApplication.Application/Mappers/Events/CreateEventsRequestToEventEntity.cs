using AutoMapper;
using EventsWebApplication.Application.Events.UseCases.CreateEvent;
using EventsWebApplication.Core.Entities;


namespace EventsWebApplication.Application.Mappers.Events
{
    public class CreateEventRequestToEventEntity : Profile
    {
        public CreateEventRequestToEventEntity()
        {
            CreateMap<CreateEventRequest, EventEntity>().ReverseMap();
        }
    }
}
