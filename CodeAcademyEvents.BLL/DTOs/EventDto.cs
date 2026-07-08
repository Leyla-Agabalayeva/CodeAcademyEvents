using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }

        public int LocationId { get; set; }
        public string? LocationName { get; set; }

        public int EventTypeId { get; set; }
        public string? EventTypeName { get; set; }

        public int OrganizerId { get; set; }
        public string? OrganizerName { get; set; }
    }
}
