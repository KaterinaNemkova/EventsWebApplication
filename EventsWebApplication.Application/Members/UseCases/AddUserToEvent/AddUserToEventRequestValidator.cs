using FluentValidation;


namespace EventsWebApplication.Application.Members.UseCases.AddUserToEvent
{
    public class AddUserToEventRequestValidator : AbstractValidator<AddUserToEventRequest>
    {
        public AddUserToEventRequestValidator()
        {
            RuleFor(x => x.EventId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.DateRegistration).NotEmpty();

        }
    }
}
