using CodeAcademyEvents.DAL.Data;
using CodeAcademyEvents.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Persons = new Repository<Person>(context);
            Events = new Repository<Event>(context);
            EventTypes = new Repository<EventType>(context);
            Locations = new Repository<Location>(context);
            Organizers = new Repository<Organizer>(context);
            Invitations = new Repository<Invitation>(context);
            Participations = new Repository<Participation>(context);
            Notifications = new Repository<Notification>(context);
            Feedbacks = new Repository<Feedback>(context);
        }

        public IRepository<Person> Persons { get; }
        public IRepository<Event> Events { get; }
        public IRepository<EventType> EventTypes { get; }
        public IRepository<Location> Locations { get; }
        public IRepository<Organizer> Organizers { get; }
        public IRepository<Invitation> Invitations { get; }
        public IRepository<Participation> Participations { get; }
        public IRepository<Notification> Notifications { get; }
        public IRepository<Feedback> Feedbacks { get; }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
