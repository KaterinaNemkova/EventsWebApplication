using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;


namespace EventsWebApplication.Application.Members.UseCases.AddUserToEvent
{
    public class AddUserToEventUseCase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationService _validationService;



        public AddUserToEventUseCase(IUnitOfWork unitOfWork, ValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _memberRepository = _unitOfWork.memberRepository;
            _userRepository = _unitOfWork.userRepository;
            _eventRepository = _unitOfWork.eventRepository;
            _validationService = validationService;
        }

        public async Task AddToEvent(AddUserToEventRequest request)
        {
            await _validationService.ValidateAsync(request);
            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
            if (eventEntity is null)
            {
                throw new KeyNotFoundException("Event not found");
            }

            var userExist = await _userRepository.AlreadyExist(request.UserId);
            if (userExist == false)
            {
                throw new KeyNotFoundException("User not found");
            }

            var isUserAlreadyInEvent = await _memberRepository.IsMemberInEvent(request.EventId, request.UserId);
            if (isUserAlreadyInEvent)
            {
                throw new InvalidOperationException("User already added to this event");
            }

            var addResult = await _memberRepository.AddAsync(request.EventId,request.UserId,request.DateRegistration);
            if (!addResult)
            {
                throw new InvalidOperationException("Error!");
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }

}
