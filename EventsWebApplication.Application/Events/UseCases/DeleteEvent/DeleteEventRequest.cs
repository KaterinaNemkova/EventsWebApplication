
namespace EventsWebApplication.Application.Events.UseCases.DeleteEvent
{
    public record DeleteEventRequest 
    {
        public required Guid Id { get; set; }
    }
}
