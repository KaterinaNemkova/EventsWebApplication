 using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Events.UseCases.GetAllEvents;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
namespace EventsWebApplication.Application.Events.Services.GetAllEvents
{
    public class GetAllEventsUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;

        public GetAllEventsUseCase(IMapper mapper, IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = _unitOfWork.eventRepository;
            _mapper = mapper;
            _validationService = validationService;
        }
        public async Task<PaginatedResult<EventDto>> GetAll(GetAllEventsRequest request)
        {
            await _validationService.ValidateAsync(request);
            var eventEntities = await _eventRepository.GetAllAsync();

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
