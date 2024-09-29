using AutoMapper;
using EventsWebApplication.Core.Contracts;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Core.Enums;

namespace EventsWebApplication.Core.Mappers
{

    public class EventMapper : Profile
    {

        public EventMapper()
        {
            
            CreateMap<EventEntity, Event>().ReverseMap();

            CreateMap<EventsRequest,Event>().ReverseMap();

            CreateMap<Event, EventsResponse>().ReverseMap();
                
        }




    }
}

