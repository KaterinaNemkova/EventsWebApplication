using EventsWebApplication.Application.Events.Services.GetAllEvents;
using FluentValidation;


namespace EventsWebApplication.Application.Events.UseCases.GetAllEvents
{
    public class GetAllEventsRequestValidator : AbstractValidator<GetAllEventsRequest>
    {
        public GetAllEventsRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }

    }
}
