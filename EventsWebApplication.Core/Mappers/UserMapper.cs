using AutoMapper;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Models;


namespace EventsWebApplication.Core.Mappers
{
    public class UserMapper:Profile
    {
        public UserMapper() 
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
