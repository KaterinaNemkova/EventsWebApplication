
namespace EventsWebApplication.Application.Members.UseCases.AddUserToEvent
{
    public class AddUserToEventRequest
    {
        public required Guid EventId { get; set; }
        public required Guid UserId { get; set; }
        public DateOnly DateRegistration { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
