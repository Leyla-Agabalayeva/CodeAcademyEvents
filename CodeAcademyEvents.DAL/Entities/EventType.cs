using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class EventType : BaseEntity
    {
        public string Name { get; set; } = null!; // Konfrans, Seminar, Bootcamp və s.

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
