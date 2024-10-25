using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EventsWebApplication.Application.Members.UseCases.AddUserToEvent;
using EventsWebApplication.Application.Members.UseCases.GetMemberByEvent;
using EventsWebApplication.Application.Members.UseCases.DeleteMemberFromEvent;
using EventsWebApplication.Application.Members.UseCases.GetMemberById;


namespace EventsWebApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController:ControllerBase
    {
        private readonly AddUserToEventUseCase _addUserToEventUseCase;
        private readonly GetMemberByEventUseCase _getMemberByEventUseCase;
        private readonly GetMemberByIdUseCase _getMemberByIdUseCase;
        private readonly DeleteMemberFromEventUseCase _deleteMemberFromEventUseCase;
        public MemberController(AddUserToEventUseCase addUserToEventUseCase, 
            GetMemberByEventUseCase getMemberByEventUseCase,
            GetMemberByIdUseCase getEventByIdUseCase,
            DeleteMemberFromEventUseCase deleteMemberFromEventUseCase)
        {
            _addUserToEventUseCase = addUserToEventUseCase;
            _getMemberByEventUseCase= getMemberByEventUseCase;
            _getMemberByIdUseCase= getEventByIdUseCase;
            _deleteMemberFromEventUseCase= deleteMemberFromEventUseCase;
        }

        [HttpPost("{eventId:guid}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> AddToEvent([FromRoute] Guid eventId)
        {
           
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "userId");

            var userId = Guid.Parse(userIdClaim.Value);                 

            await _addUserToEventUseCase.AddToEvent(new AddUserToEventRequest { EventId=eventId, UserId= userId, DateRegistration=DateOnly.FromDateTime(DateTime.Now)});
            return Ok();

        }


        [HttpGet("GetByEvent{eventId:guid}")]
        [Authorize(Policy = "User")]

        public async Task<IActionResult> GetMembersByEvent(Guid eventId, [FromQuery] int pageNumber=1, [FromQuery] int pageSize=2 )
        {
            var members=await _getMemberByEventUseCase.GetByEvent(new GetMemberByEventRequest { EventId=eventId, PageNumber=pageNumber, PageSize=pageSize });
            return Ok(members);
        }


        [HttpGet("GetById{memberId:guid}")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> GetMemberById(Guid memberId)
        {
           var member=await _getMemberByIdUseCase.GetById(new GetMemberByIdRequest { Id=memberId });
            return Ok(member);
        }

        [HttpDelete("{eventId:guid}/{memberId:guid}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteMemberFromEvent(Guid eventId, Guid memberId)
        {
            await _deleteMemberFromEventUseCase.Delete(new DeleteMemberFromEventRequest { EventId=eventId, UserId= memberId });
            return Ok();
        }

    }
}
