
namespace EventsWebApplication.Application.Events.UseCases.GetAllEvents
{
    public record GetAllEventsRequest
    {
        public required int PageSize { get; init; }
        public required int PageNumber { get; init; }
    }

}
