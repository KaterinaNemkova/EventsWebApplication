
using FluentValidation;


namespace EventsWebApplication.Application.Events.UseCases.CreateEvent
{
    public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
    {
        public CreateEventRequestValidator()
        {
            RuleFor(x => x.Title).Length(0, 10).NotEmpty();
            RuleFor(x => x.Description).NotEmpty().Length(5, 30);
            RuleFor(x => x.MaxCountPeople).NotEmpty();
            RuleFor(x => x.EventsCategory).IsInEnum();

        }
    }
}
