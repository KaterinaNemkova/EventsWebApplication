using EventsWebApplication.Core.Enums;
using FluentValidation;

namespace EventsWebApplication.Core.Contracts
{
    public record EventsRequest(string title, string description, DateTime dateTime, string place, EventsCategory eventsCategory, int maxCountPeople);

    public class EventValidator : AbstractValidator<EventsRequest>
    {
        public EventValidator()
        {
            RuleFor(x => x.title).Length(0, 10).NotEmpty();
            RuleFor(x => x.description).NotEmpty().Length(5,30);
            RuleFor(x => x.maxCountPeople).NotEmpty();
            RuleFor(x => x.eventsCategory).IsInEnum();
            


        }
    }
}
