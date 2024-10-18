using EventsWebApplication.Core.Enums;

namespace EventsWebApplication.Application.Events.UseCases.GetEventByFilter
{
    public record GetEventsByFilterRequest
    {
        public required DateTime? DateTime { get; set; }
        public required string? Place {  get; set; }
        public required EventsCategory? EventsCategory { get; set; }
        public required int PageSize { get; init; }
        public required int PageNumber { get; init; }



    }
}
