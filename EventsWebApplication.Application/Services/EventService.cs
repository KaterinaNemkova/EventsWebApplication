using AutoMapper;
using EventsWebApplication.Core.Contracts;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Models;
using EventsWebApplication.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace EventsWebApplication.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository repo,IMapper mapper)
        {
            _eventRepository = repo;
            _mapper = mapper;
        }

        public async Task<List<Event>> GetEvents()
        {
            var eventEntities = await _eventRepository.Get();
            var events=_mapper.Map<List<Event>>(eventEntities);
            return events;
        }

        public async Task<Event?> GetById(Guid id)
        {

            var eventEntity = await _eventRepository.GetById(id);

            if (eventEntity == null)
            {
                throw new InvalidOperationException("Event not found.");
            }
            var eventModel=_mapper.Map<Event>(eventEntity);
            return eventModel;
        }

        public async Task<Event?> GetByName(string name)
        {
            var eventEntity = await _eventRepository.GetByName(name);
            if (eventEntity == null)
            {
                throw new InvalidOperationException("Event not found.");
            }
            var eventModel = _mapper.Map<Event>(eventEntity);
            return eventModel;
        }

        public async Task<List<Event>> GetByFilter(DateTime? dateTime, string? place, EventsCategory? eventsCategory)
        {
            var eventEntities = await _eventRepository.GetByFilter(dateTime, place, eventsCategory);
            if (eventEntities == null)
            {
                throw new InvalidOperationException("No events found for the given filters.");
            }
            var events = _mapper.Map<List<Event>>(eventEntities);
            return events;
        }

        public async Task<Guid> CreateEvent(Event eventModel)
        {
            return await _eventRepository.Create(eventModel);
        }

        public async Task Update(Guid id,EventsRequest request)
        {
            var eventEntity = await _eventRepository.AlreadyExist(id);

            if (eventEntity == false)
            {
                throw new InvalidOperationException("This event doesn't exist.");
            }
            var model = _mapper.Map<Event>(request);
            model.Id =id;
            await _eventRepository.Update(model);
        }

        public async Task Delete(Guid id)
        {
            var eventEntity = await _eventRepository.AlreadyExist(id);

            if (eventEntity == false)
            {
                throw new InvalidOperationException("Event not found.");
            }
            await _eventRepository.Delete(id);
        }

        public async Task<bool> Exist(Guid id)
        {
            return await _eventRepository.AlreadyExist(id);
        }

        public async Task UploadImage(Guid id, IFormFile file)
        {
            var eventEntity = await _eventRepository.GetById(id);

            if (eventEntity == null)
            {
                throw new InvalidOperationException("Event not found.");
            }

            if (!string.IsNullOrEmpty(eventEntity.EventImage))
            {
                var oldFilePath = Path.Combine("wwwroot", "images", eventEntity.EventImage);

                if(System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine("wwwroot", "images", fileName);
            
            using (var stream=new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _eventRepository.UploadImage(id, fileName);

        }

    }
}
