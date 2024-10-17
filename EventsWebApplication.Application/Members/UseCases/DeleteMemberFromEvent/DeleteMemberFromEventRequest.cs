
namespace EventsWebApplication.Application.Members.UseCases.DeleteMemberFromEvent
{
    public class DeleteMemberFromEventRequest
    {
        public required Guid EventId { get; set; }
        public required Guid UserId { get; set; }
    }
}
