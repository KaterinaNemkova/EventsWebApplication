using EventsWebApplication.Core.Entities;
using AutoMapper;
using EventsWebApplication.Application.DTOs;


namespace EventsWebApplication.Core.Mappers
{
    public class MemberEntityToMemberDto : Profile
    {
        public MemberEntityToMemberDto() 
        {
            CreateMap<MemberEntity, MemberDto>().ReverseMap();

        }
    }
}
