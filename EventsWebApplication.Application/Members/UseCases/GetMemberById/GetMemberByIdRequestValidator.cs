using FluentValidation;


namespace EventsWebApplication.Application.Members.UseCases.GetMemberById
{
    public class GetMemberByIdRequestValidator : AbstractValidator<GetMemberByIdRequest>
    {
        public GetMemberByIdRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

        }

    }
}
