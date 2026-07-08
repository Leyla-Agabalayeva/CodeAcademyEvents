using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class Notification : BaseEntity
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public string Message { get; set; } = null!;
        public DateTime SentAt { get; set; } = DateTime.Now;
    }

}
