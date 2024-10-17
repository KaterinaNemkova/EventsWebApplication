using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.Members.UseCases.GetMemberByEvent
{
    public class GetMemberByEventRequestValidator : AbstractValidator<GetMemberByEventRequest>
    {
        public GetMemberByEventRequestValidator()
        {
            RuleFor(x => x.EventId).NotEmpty();

            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);

        }

    }
}
