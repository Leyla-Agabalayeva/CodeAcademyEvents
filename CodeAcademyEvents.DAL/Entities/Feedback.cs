using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class Feedback : BaseEntity
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}
