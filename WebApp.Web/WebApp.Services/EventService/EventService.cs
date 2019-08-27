﻿namespace WebApp.Services.EventService
{
    using Data.Repo.EventRepo;
    using Data.Repo.UnitOfWork;
    using Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventService : BaseService, IEventService
    {
        private readonly IEventRepository eventR;

        public EventService(IEventRepository eventR, IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this.eventR = eventR;
        }

        public void CreateEvent(Event createEvent)
        {
            this.eventR.CreateEvent(createEvent);
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return this.eventR.GetAllEvents().Where(e => e.IsDeleted == false);
        }

        public Event GetEvent(int id)
        {
            Event foundEvent = this.eventR.GetEvent(id);

            if (foundEvent == null)
            {
                throw new ArgumentException($"No Event with ID: {id} exists!");
            }

            return foundEvent;
        }

        public IEnumerable<Event> GetAllEventsByUser(string id)
        {
            return this.eventR.GetAllEvents()
                 .Where(x => x.AdminId == id && x.IsDeleted == false)
                 .ToList();
        }

        public async Task DeleteEvent(int id)
        {
            var eventToBeDeleted = this.GetEvent(id);

            eventToBeDeleted.IsDeleted = true;

            await SaveChangesAsync();
        }

        public void EditEvent(Event editEvent)
        {
            var newEvent = this.GetEvent(editEvent.Id);

            newEvent.Name = editEvent.Name;
            newEvent.Description = editEvent.Description;
            newEvent.Location = editEvent.Location;
            newEvent.SportId = editEvent.SportId;
            newEvent.Time = editEvent.Time;

            if (editEvent.Image != null)
            {
                newEvent.Image = editEvent.Image;
            }

            SaveChangesAsync().Wait();
        }
    }
}
