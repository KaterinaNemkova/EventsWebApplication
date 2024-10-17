using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;


namespace EventsWebApplication.Application.Events.UseCases.GetEventById
{
    public class GetEventByIdUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidationService _validationService;


        public GetEventByIdUseCase(IUnitOfWork unitOfWork, IMapper mapper, ValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = _unitOfWork.eventRepository;
            _mapper = mapper;
            _validationService = validationService;
        }
        public async Task<EventDto> GetById(GetEventByIdRequest request)
        {
            await _validationService.ValidateAsync(request);
            var eventEntity = await _eventRepository.GetByIdAsync(request.Id);

            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }
            var eventDto = _mapper.Map<EventDto>(eventEntity);
            return eventDto;
        }
    }
}
