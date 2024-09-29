using EventsWebApplication.Core.Models;
using EventsWebApplication.Core.Entities;
using AutoMapper;
using EventsWebApplication.Core.Contracts;


namespace EventsWebApplication.Core.Mappers
{
    public class MemberMapper : Profile
    {
        public MemberMapper() 
        {
            CreateMap<MemberEntity, Member>().ReverseMap();

            CreateMap<Member,MembersResponse>().ReverseMap();

        }
    }
}
