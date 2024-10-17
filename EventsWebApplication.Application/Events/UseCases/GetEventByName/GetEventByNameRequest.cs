
namespace EventsWebApplication.Application.Events.UseCases.GetEventByName
{
    public record GetEventByNameRequest
    {
        public required string Name { get; set; }
    }
}
