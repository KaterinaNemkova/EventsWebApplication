

using FluentValidation;

namespace EventsWebApplication.Application.Members.UseCases.DeleteMemberFromEvent
{
    public class DeleteMemberFromEventRequestValidator : AbstractValidator<DeleteMemberFromEventRequest>
    {
        public DeleteMemberFromEventRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.EventId).NotEmpty();
        }
    }
}
