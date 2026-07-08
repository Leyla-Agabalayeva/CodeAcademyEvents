using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public int EventTypeId { get; set; }
        public EventType EventType { get; set; } = null!;

        public int OrganizerId { get; set; }
        public Organizer Organizer { get; set; } = null!;

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }

}
