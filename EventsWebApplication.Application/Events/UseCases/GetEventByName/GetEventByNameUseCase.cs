using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;

namespace EventsWebApplication.Application.Events.UseCases.GetEventByName
{
    public class GetEventByNameUseCase 
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;

        public GetEventByNameUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = _unitOfWork.eventRepository;
            _mapper = mapper;
            _validationService = validationService;
        }
        public async Task<EventDto> GetByName(GetEventByNameRequest request)
        {
            await _validationService.ValidateAsync(request);
            var eventEntity = await _eventRepository.GetByNameAsync(request.Name);

            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }
            var eventDto = _mapper.Map<EventDto>(eventEntity);
            return eventDto;
        }
    }
}
