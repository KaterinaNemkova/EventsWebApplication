using AutoMapper;
using EventsWebApplication.Core;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;


namespace EventsWebApplication.Application.Events.UseCases.UploadImage
{
    public class UploadImageUseCase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IValidationService _validationService;

        public UploadImageUseCase (IUnitOfWork unitOfWork,IFileService fileService, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = _unitOfWork.eventRepository;
            _fileService = fileService;
            _validationService = validationService;
        }
        public async Task UploadImage(UploadImageRequest request)
        {
            if (request.File == null)
            {
                throw new ArgumentNullException(nameof(request.File), "The uploaded file cannot be null.");
            }
            await _validationService.ValidateAsync(request);
            var eventEntity = await _eventRepository.GetByIdAsync(request.Id);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }
            if (!string.IsNullOrEmpty(eventEntity.EventImage))
            {
                var oldFilePath = Path.Combine("wwwroot", "images", eventEntity.EventImage);
                _fileService.DeleteFile(oldFilePath); 
            }
            
            

            var fileName = await _fileService.SaveFileAsync(request.File, "wwwroot/images");

            await _eventRepository.UploadImage(request.Id, fileName);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
