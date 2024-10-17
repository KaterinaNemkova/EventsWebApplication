
using EventsWebApplication.Application.Events.UseCases.GetEventByFilter;
using FluentValidation;

namespace EventsWebApplication.Application.Events.UseCases.GetEventsByFilter
{
    public class GetEventsByFilterRequestValidator : AbstractValidator<GetEventsByFilterRequest>
    {
        public GetEventsByFilterRequestValidator()
        {
            RuleFor(x => x.DateTime).GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(x => x.Place).MaximumLength(90);
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }

    }
    
}
