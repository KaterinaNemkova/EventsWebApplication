using EventsWebApplication.Core.Enums;

namespace EventsWebApplication.Core.Entities
{
    public class EventEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; }= string.Empty;

        public DateTime DateTime { get; set; }

        public string Place {  get; set; }=string.Empty;

        public EventsCategory EventCategory { get; set; }

        public int MaxCountPeople { get; set; }

        public List<MemberEntity> Members { get; set; } = [];

        public string? EventImage { get; set; }=string.Empty;



    }
}
