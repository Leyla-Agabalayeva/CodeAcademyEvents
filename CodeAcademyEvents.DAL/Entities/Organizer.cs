using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class Organizer : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }

}
