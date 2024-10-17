using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.Events.UseCases.GetEventByName
{
    public class GetEventByNameRequestValidator : AbstractValidator<GetEventByNameRequest>
    {
        public GetEventByNameRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }

    }
}
