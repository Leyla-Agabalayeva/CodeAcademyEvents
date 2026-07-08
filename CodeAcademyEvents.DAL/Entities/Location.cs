using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Capacity { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
