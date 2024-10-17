﻿using EventsWebApplication.Application.DTOs;
using MediatR;


namespace EventsWebApplication.Application.Events.UseCases.GetEventById
{
    public record GetEventByIdRequest
    {
        public required Guid Id { get; init; }
      
    }
}
