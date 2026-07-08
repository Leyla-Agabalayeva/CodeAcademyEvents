using CodeAcademyEvents.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.DTOs
{
    public class InvitationDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string? EventTitle { get; set; }
        public System.DateTime EventDate { get; set; }
        public int PersonId { get; set; }
        public string? PersonFullName { get; set; }
        public InvitationStatus Status { get; set; }
        public DateTime SentAt { get; set; }
    }

}
