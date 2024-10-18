using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;


namespace EventsWebApplication.Application.Events.UseCases.DeleteEvent
{
    public class DeleteEventUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;


        public DeleteEventUseCase(IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = unitOfWork.eventRepository;
            _validationService = validationService;
            
            
        }
        public async Task Delete(DeleteEventRequest request)
        {
            await _validationService.ValidateAsync(request);
            EventEntity? eventObject = await _eventRepository.GetByIdAsync(request.Id);

            if (eventObject is null)
                throw new KeyNotFoundException($"Event doesn't exist.");

            await _eventRepository.Delete(eventObject);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
