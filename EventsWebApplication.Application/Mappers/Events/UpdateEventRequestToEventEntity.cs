using AutoMapper;
using EventsWebApplication.Application.Events.UseCases.UpdateEvent;
using EventsWebApplication.Core.Entities;

namespace EventsWebApplication.Application.Mappers.Events
{
    public class UpdateEventRequestToEventEntity : Profile
    {
        public UpdateEventRequestToEventEntity()
        {
            CreateMap<UpdateEventRequest, EventEntity>().ReverseMap();
        }
    }
}
