using AutoMapper;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Models;
using EventsWebApplication.DataAccess.Repositories;


namespace EventsWebApplication.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        
        public MemberService(IMemberRepository memberRepository,IEventService eventService,IMapper mapper,IUserService service)
        {
            _memberRepository = memberRepository;
            _eventService = eventService;
            _mapper = mapper;
            _userService= service;

        }

        public async Task<AddToEventResult> AddToEvent(Guid eventId, Guid userId, string surname, DateOnly birthDate, DateOnly registrDate)
        {
            var eventExist = await _eventService.Exist(eventId);
            if (!eventExist)
            {
                return AddToEventResult.EventNotFound;
            }

            var userExist = await _userService.AlreadyExist(userId);
            if (userExist==false)
            {
                return AddToEventResult.UserNotFound;
            }

            var isUserAlreadyInEvent = await _memberRepository.IsMemberInEvent(eventId, userId);
            if (isUserAlreadyInEvent)
            {
                return AddToEventResult.AlreadyExists; 
            }

            var addResult = await _memberRepository.Add(eventId, userId,surname,birthDate,registrDate);
            if (!addResult)
            {
                return AddToEventResult.Failure; 
            }

            return AddToEventResult.Success;
        }
        public async Task<List<Member>> Get(Guid eventId)
        {
            var eventExist=await _eventService.Exist(eventId);
            if (!eventExist)
            {
                throw new InvalidOperationException("Event not found.");
            }
            var memberEntities = await _memberRepository.Get(eventId);

            var members=_mapper.Map<List<Member>>(memberEntities);

            return members;
        }

        public async Task<Member?> GetById(Guid id)
        { 
            var memberEntity = await _memberRepository.GetById(id);

            if (memberEntity == null)
            {
                throw new InvalidOperationException("Member not found.");
            }

            var member=_mapper.Map<Member>(memberEntity);
            return member;
        }

        public async Task<DeleteMemberResults> DeleteMember(Guid eventId, Guid memberId)
        {
            
            var eventExists = await _eventService.Exist(eventId);
            if (!eventExists)
            {
                return DeleteMemberResults.EventNotFound;
            }

            var memberExists = await _memberRepository.AlreadyExist(memberId);
            if (!memberExists)
            {
                return DeleteMemberResults.MemberNotFound;
            }

            var isDeleted = await _memberRepository.Delete(eventId, memberId);

            if (!isDeleted)
            {
                return DeleteMemberResults.MemberNotInEvent;
            }

            return DeleteMemberResults.Success;
        }

        
        
    }
}
