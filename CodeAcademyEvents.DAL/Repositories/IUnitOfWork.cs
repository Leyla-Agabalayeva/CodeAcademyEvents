using CodeAcademyEvents.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Person> Persons { get; }
        IRepository<Event> Events { get; }
        IRepository<EventType> EventTypes { get; }
        IRepository<Location> Locations { get; }
        IRepository<Organizer> Organizers { get; }
        IRepository<Invitation> Invitations { get; }
        IRepository<Participation> Participations { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<Feedback> Feedbacks { get; }

        Task<int> SaveChangesAsync();
    }
}
