
namespace EventsWebApplication.Application.Members.UseCases.GetMemberByEvent
{
    public class GetMemberByEventRequest
    {
        public required Guid EventId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
