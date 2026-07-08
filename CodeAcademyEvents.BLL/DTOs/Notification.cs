using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Message { get; set; } = null!;
        public DateTime SentAt { get; set; }
    }
   
}
