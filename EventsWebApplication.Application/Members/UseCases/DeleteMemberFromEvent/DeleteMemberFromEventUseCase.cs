using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;


namespace EventsWebApplication.Application.Members.UseCases.DeleteMemberFromEvent
{
    public class DeleteMemberFromEventUseCase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationService _validationService;
        public DeleteMemberFromEventUseCase(IUnitOfWork unitOfWork, ValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _memberRepository = _unitOfWork.memberRepository;
            _eventRepository = _unitOfWork.eventRepository;
            _validationService = validationService;
        }

        public async Task Delete(DeleteMemberFromEventRequest request)
        {
            await _validationService.ValidateAsync(request);
            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
            if (eventEntity is null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            var memberExists = await _memberRepository.AlreadyExist(request.UserId);
            if (!memberExists)
            {
                throw new KeyNotFoundException("Member not found");
            }
            bool isMemberInEvent=await _memberRepository.IsMemberInEvent(request.EventId, request.UserId);
            if (!isMemberInEvent)
            {
                throw new InvalidOperationException("Member is not in event");
            }

            var isDeleted = await _memberRepository.Delete(request.EventId, request.UserId);

            if (!isDeleted)
            {
                throw new InvalidOperationException("Something wrong");
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
