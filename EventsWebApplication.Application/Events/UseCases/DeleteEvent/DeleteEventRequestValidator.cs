using FluentValidation;


namespace EventsWebApplication.Application.Events.UseCases.DeleteEvent
{
    public class DeleteEventRequestValidator : AbstractValidator<DeleteEventRequest>
    {
        public DeleteEventRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }

    }
}
