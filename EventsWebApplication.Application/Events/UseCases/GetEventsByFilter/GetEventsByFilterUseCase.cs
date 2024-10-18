using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Events.UseCases.GetEventByFilter;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;


namespace EventsWebApplication.Application.Events.UseCases.GetEventsByFilter
{
    public class GetEventsByFilterUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;

        public GetEventsByFilterUseCase(IUnitOfWork unitOfWork, IMapper mapper, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = _unitOfWork.eventRepository;
            _mapper = mapper;
            _validationService = validationService;
        }
        public async Task<PaginatedResult<EventDto>> GetByFilter(GetEventsByFilterRequest request)
        {
            await _validationService.ValidateAsync(request);
            List<EventEntity> eventEntities = await _eventRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(request.Place))
            {
                eventEntities = eventEntities.Where(x => x.Place==request.Place).ToList();
            }
            if (request.DateTime.HasValue)
            {
                eventEntities = eventEntities.Where(x => x.DateTime.Date == request.DateTime.Value.Date).ToList();
            }
            if (request.EventsCategory.HasValue)
            {
                eventEntities = eventEntities.Where(x => x.EventCategory == request.EventsCategory.Value).ToList();
            }

            if (eventEntities == null || !eventEntities.Any())
            {
                throw new KeyNotFoundException("No events found for the given filters.");
            }
            var pagedItems = eventEntities
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var eventsDto = _mapper.Map<List<EventDto>>(pagedItems);

            return new PaginatedResult<EventDto>
            {
                Items = eventsDto,
                TotalCount = eventEntities.Count,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber
            };
        }
    }
}
