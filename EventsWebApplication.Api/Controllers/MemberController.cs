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
    public class MemberController:ControllerBase
    {
        private readonly IMemberService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<MemberRegistrationRequest> _validator;

        public MemberController(IMemberService service,IMapper mapper,IValidator<MemberRegistrationRequest> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpPost("{eventId:guid}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> AddToEvent(Guid eventId, [FromBody] MemberRegistrationRequest request)
        {
            ValidationResult validResult = await _validator.ValidateAsync(request);

            if (!validResult.IsValid)
            {
                return BadRequest(validResult.Errors);
            }
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "userId");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);

            var result = await _service.AddToEvent(eventId, userId, request.surname, request.birthDate, DateOnly.FromDateTime(DateTime.Now));

            return result switch
            {
                AddToEventResult.Success => Ok("Member successfully added to the event."),
                AddToEventResult.AlreadyExists => Conflict("Member already added to this event."),
                AddToEventResult.EventNotFound => NotFound("Event not found."),
                AddToEventResult.UserNotFound => NotFound("User not found."),
                _ => StatusCode(500, "An unexpected error occurred.")
            };
        }


        [HttpGet("GetByEvent{eventId:guid}")]
        [Authorize(Policy = "User")]

        public async Task<ActionResult<List<MembersResponse>>> GetByEvent(Guid eventId)
        {
            try
            {
                var members = await _service.Get(eventId);
                var response = _mapper.Map<List<MembersResponse>>(members);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {

                return NotFound(ex.Message);
            } 
        }


        [HttpGet("GetById{memberId:guid}")]
        [Authorize(Policy = "User")]
        public async Task<ActionResult<MembersResponse>> GetById(Guid memberId)
        {
            try
            {
                var member = await _service.GetById(memberId);
                var response=_mapper.Map<MembersResponse>(member);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                
                return NotFound(ex.Message);
            } 
        }

        [HttpDelete("{eventId:guid}/{memberId:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteMemberFromEvent(Guid eventId, Guid memberId)
        {
            var result = await _service.DeleteMember(eventId, memberId);

            return result switch
            {
                DeleteMemberResults.Success => Ok("Member successfully deleted from event"),
                DeleteMemberResults.EventNotFound => NotFound("Event not found"),
                DeleteMemberResults.MemberNotFound => NotFound("Member not found"),
                DeleteMemberResults.MemberNotInEvent => BadRequest("Member is not part of this event"),
                _ => StatusCode(500, "An error occurred while processing your request")
            };
        }

    }
}
