using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.DTOs
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int PersonId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
