using EventsWebApplication.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EventsWebApplication.Application.Events.UseCases.CreateEvent;
using EventsWebApplication.Application.Events.Services.GetAllEvents;
using EventsWebApplication.Application.Events.UseCases.GetAllEvents;
using EventsWebApplication.Application.Events.UseCases.GetEventById;
using EventsWebApplication.Application.Events.UseCases.GetEventByName;
using EventsWebApplication.Application.Events.UseCases.GetEventsByFilter;
using EventsWebApplication.Application.Events.UseCases.GetEventByFilter;
using EventsWebApplication.Application.Events.UseCases.UpdateEvent;
using EventsWebApplication.Application.Events.UseCases.UploadImage;
using EventsWebApplication.Application.Events.UseCases.DeleteEvent;


namespace EventsWebApplication.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    
    public class EventsController:ControllerBase
    {
        private readonly CreateEventUseCase _createEventUseCase;
        private readonly GetAllEventsUseCase _getAllEventsUseCase;
        private readonly GetEventByIdUseCase _getEventByIdUseCase;
        private readonly GetEventByNameUseCase _getEventByNameUseCase;
        private readonly GetEventsByFilterUseCase _getEventsByFilterUseCase;
        private readonly UpdateEventUseCase _updateEventUseCase;
        private readonly UploadImageUseCase _uploadImageUseCase;
        private readonly DeleteEventUseCase _deleteEventUseCase;
        public EventsController(CreateEventUseCase createEventUseCase, 
            GetAllEventsUseCase getAllEventsUseCase, GetEventByIdUseCase getEventByIdUseCase, 
            GetEventByNameUseCase getEventByNameUseCase, GetEventsByFilterUseCase getEventsByFilterUseCase, 
            UpdateEventUseCase updateEventUseCase, DeleteEventUseCase deleteEventUseCase, UploadImageUseCase uploadImageUseCase)
        {
           _createEventUseCase = createEventUseCase;
            _getAllEventsUseCase= getAllEventsUseCase;
            _getEventByIdUseCase= getEventByIdUseCase;
            _getEventByNameUseCase= getEventByNameUseCase;
            _getEventsByFilterUseCase= getEventsByFilterUseCase;
            _updateEventUseCase= updateEventUseCase;
            _deleteEventUseCase= deleteEventUseCase;
            _uploadImageUseCase= uploadImageUseCase;
        }


        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllEvents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            var events = await _getAllEventsUseCase.GetAll(new GetAllEventsRequest { PageNumber = pageNumber, PageSize = pageSize });
            return Ok(events);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
           var _event=await _getEventByIdUseCase.GetById(new GetEventByIdRequest { Id=id});     
            return Ok(_event);

        }

        [HttpGet("{title}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetByName(string title)
        {
            var _event = await _getEventByNameUseCase.GetByName(new GetEventByNameRequest { Name = title });
            return Ok(_event);
        }

        [HttpGet("filter")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetByFilter(
            [FromQuery] DateTime? eventDate,
            [FromQuery] string? place,
            [FromQuery] EventsCategory? eventsCategory, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            var events = await _getEventsByFilterUseCase.GetByFilter(new GetEventsByFilterRequest { DateTime = eventDate, Place = place, 
                EventsCategory = eventsCategory,
                PageNumber = pageNumber, PageSize = pageSize });
            return Ok(events);
        }


        [HttpPost("create-event")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
        {

            await _createEventUseCase.Create(request);
            return Ok();
            
        }

        [HttpPut]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request)
        {
            await _updateEventUseCase.Update(request);
            return Ok();
        }
        [HttpPost("upload-image/{eventId}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UploadImage(Guid eventId, IFormFile file)
        {
           await _uploadImageUseCase.UploadImage(new UploadImageRequest { Id=eventId, File=file});
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid id)
        {
           await _deleteEventUseCase.Delete(new DeleteEventRequest { Id = id });
            return Ok();
        }

    }
}
