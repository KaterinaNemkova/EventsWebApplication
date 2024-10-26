using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;

namespace EventsWebApplication.Application.Members.UseCases.GetMemberByEvent
{
    public class GetMemberByEventUseCase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ValidationService _validationService;

        public GetMemberByEventUseCase(IMapper mapper,IUnitOfWork unitOfWork, ValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _memberRepository = _unitOfWork.memberRepository;
            _mapper = mapper;
            _eventRepository = unitOfWork.eventRepository;
            _validationService = validationService;
        }

        public async Task<PaginatedResult<MemberDto>> GetByEvent(GetMemberByEventRequest request)
        {
            await _validationService.ValidateAsync(request);
            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
            if (eventEntity is null)
            {
                throw new KeyNotFoundException("Event not found.");
            }
            var memberEntities = await _memberRepository.GetByEventAsync(request.EventId, request.PageNumber, request.PageSize);
            var totalCount = await _memberRepository.GetTotalCountAsync();
            var membersDto = _mapper.Map<List<MemberDto>>(memberEntities);
            
            return new PaginatedResult<MemberDto>
            {
                Items = membersDto,
                TotalCount = totalCount,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber
            };
            
        }
    }

}
