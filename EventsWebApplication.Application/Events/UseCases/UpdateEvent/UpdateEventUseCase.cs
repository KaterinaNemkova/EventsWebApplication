using AutoMapper;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;

namespace EventsWebApplication.Application.Events.UseCases.UpdateEvent
{
    public class UpdateEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ValidationService _validationService;

        public UpdateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper, ValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = _unitOfWork.eventRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

       
        public async Task Update(UpdateEventRequest request)
        {
            await _validationService.ValidateAsync(request);
            EventEntity entity = _mapper.Map<EventEntity>(request);

            var eventObject = await _eventRepository.GetByIdAsync(entity.Id);

            if (eventObject is not null)
                throw new KeyNotFoundException($"Event with id {entity.Id} doesn't exist.");
            await _eventRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
