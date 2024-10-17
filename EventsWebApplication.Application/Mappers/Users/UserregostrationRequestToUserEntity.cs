using AutoMapper;
using EventsWebApplication.Application.Users.Registration;
using EventsWebApplication.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.Mappers.Users
{
    public class UserRegistrationRequestToUserEntity:Profile
    {
        public UserRegistrationRequestToUserEntity()
        {
            CreateMap<UserRegistrationRequest, UserEntity>().ReverseMap();
        }
    }
}
