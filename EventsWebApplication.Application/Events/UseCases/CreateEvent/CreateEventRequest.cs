using EventsWebApplication.Core.Enums;


namespace EventsWebApplication.Application.Events.UseCases.CreateEvent
{
    public record CreateEventRequest 
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime DateTime { get; set; }
        public required string Place { get; set; }
        public required EventsCategory EventsCategory { get; set; }
        public required int MaxCountPeople { get; set; }
    };

    
}
