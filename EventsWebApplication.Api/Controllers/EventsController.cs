using EventsWebApplication.Core.Contracts;
using EventsWebApplication.Application.Services;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

namespace EventsWebApplication.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    
    public class EventsController:ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;
        private readonly IValidator<EventsRequest> _validator;

        public EventsController(IEventService eventService,IMapper mapper, IValidator<EventsRequest> validator)
        {
            _eventService = eventService;
            _mapper = mapper;
            _validator = validator;
        }


        [HttpGet]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<List<EventsResponse>>> GetEvents()
        {
            try
            {
                var events = await _eventService.GetEvents();
                var response = _mapper.Map<List<EventsResponse>>(events);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<EventsResponse>> GetById(Guid id)
        {
            try
            {
                var eventModel = await _eventService.GetById(id);
                var response = _mapper.Map<EventsResponse>(eventModel);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpGet("{title}")]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<EventsResponse>> GetByName(string title)
        {
            try
            {
                var eventModel = await _eventService.GetByName(title);
                var response = _mapper.Map<EventsResponse>(eventModel);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("filter")]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<List<EventsResponse>>> GetByFilter(
            [FromQuery] DateTime? eventDate,
            [FromQuery] string? place,
            [FromQuery] EventsCategory? eventsCategory)
        {
            try 
            {
                var events = await _eventService.GetByFilter(eventDate, place, eventsCategory);
                var response = _mapper.Map<List<EventsResponse>>(events);
                return Ok(response);
            }
            catch(InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost("create-event")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<EventsResponse>> CreateEvent([FromBody] EventsRequest request)
        {
            ValidationResult result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var eventModel = new Event {
                    Id=Guid.NewGuid(),
                    Title=request.title,
                    Description=request.description,
                    DateTime=request.dateTime,
                    Place=request.place,
                    EventCategory=request.eventsCategory,
                    MaxCountPeople=request.maxCountPeople
                    };
            await _eventService.CreateEvent(eventModel);
            var response=_mapper.Map<EventsResponse>(eventModel);
            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UpdateEvent([FromBody] EventsRequest request,Guid id)
        {
            ValidationResult result = await _validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors); 
            }
            try
            {
                await _eventService.Update(id, request);
                return Ok("Event succesfully updated");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("upload-image/{eventId}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> UploadImage(Guid eventId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Invalid file.");
                }

                await _eventService.UploadImage(eventId, file);
                return Ok("Image succesfully added");
            }

            catch(InvalidOperationException ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteEvent([FromQuery] Guid id)
        {
            try
            {
                await _eventService.Delete(id);
                return Ok($"Event with id {id} is deleted");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            

           
        }

    }
}
