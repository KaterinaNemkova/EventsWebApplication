using EventsWebApplication.Core.Enums;

namespace EventsWebApplication.Application.Events.UseCases.UpdateEvent
{
    public record UpdateEventRequest
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime DateTime { get; set; }
        public required string Place { get; set; }
        public required EventsCategory EventCategory { get; set; }
        public required int MaxCountPeople { get; set; }
    };
}
