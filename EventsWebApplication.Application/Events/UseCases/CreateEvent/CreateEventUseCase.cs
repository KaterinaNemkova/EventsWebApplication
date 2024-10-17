using AutoMapper;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;


namespace EventsWebApplication.Application.Events.UseCases.CreateEvent
{
    public class CreateEventUseCase 
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ValidationService _validationService;

        public CreateEventUseCase(IUnitOfWork unitOfWork, IMapper mapper,ValidationService validationService)
        {
            _unitOfWork=unitOfWork;
            _eventRepository = _unitOfWork.eventRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task Create(CreateEventRequest request)
        {
            await _validationService.ValidateAsync(request);

            EventEntity entity=_mapper.Map<EventEntity>(request);

            var eventObject = await _eventRepository.GetByIdAsync(entity.Id);

            if (eventObject is not null)
                    throw new InvalidOperationException($"Event with id {entity.Id} already exists.");

            await _eventRepository.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
