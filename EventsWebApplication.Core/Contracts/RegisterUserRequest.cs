using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Core.Contracts
{
    public record RegisterUserRequest(string userName, string email,string password);

    public class UserValidator : AbstractValidator<RegisterUserRequest>
    {
        public UserValidator()
        {
            RuleFor(x => x.userName).Length(0, 10).NotEmpty();
            RuleFor(x => x.email).EmailAddress();
            RuleFor(x => x.password).NotEmpty().MinimumLength(6);
        }
    }
}
