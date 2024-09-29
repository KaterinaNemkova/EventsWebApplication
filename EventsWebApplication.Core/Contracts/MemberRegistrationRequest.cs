using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Core.Contracts
{
    public record MemberRegistrationRequest(string surname, DateOnly birthDate);

    public class MemberValidator : AbstractValidator<MemberRegistrationRequest>
    {
        public MemberValidator()
        {
            RuleFor(x => x.surname).Length(0, 25).NotEmpty();
            RuleFor(x=>x.birthDate).LessThan(DateOnly.FromDateTime(DateTime.Now));



        }
    }
}
