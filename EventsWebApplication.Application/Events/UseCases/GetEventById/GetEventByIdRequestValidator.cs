using EventsWebApplication.Application.Events.Services.GetAllEvents;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.Events.UseCases.GetEventById
{
    public class GetEventByIdRequestValidator : AbstractValidator<GetEventByIdRequest>
    {
        public GetEventByIdRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            
        }

    }
}
