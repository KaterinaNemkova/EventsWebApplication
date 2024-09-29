using EventsWebApplication.Core.Enums;

namespace EventsWebApplication.Core.Contracts
{
    public record EventsResponse(
        string Title, 
        string Description, 
        DateTime DateTime, 
        string Place, 
        EventsCategory EventCategory, 
        int MaxCountPeople,
        string EventImage
        );
    
}
