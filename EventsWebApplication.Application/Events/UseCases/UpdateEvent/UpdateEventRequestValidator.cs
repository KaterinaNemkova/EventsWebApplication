using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.Events.UseCases.UpdateEvent
{
    public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventRequestValidator()
        {
            RuleFor(x => x.Title).Length(0, 10).NotEmpty();
            RuleFor(x => x.Description).NotEmpty().Length(5, 30);
            RuleFor(x => x.MaxCountPeople).NotEmpty();
            RuleFor(x => x.EventCategory).IsInEnum();

        }
    }
}
