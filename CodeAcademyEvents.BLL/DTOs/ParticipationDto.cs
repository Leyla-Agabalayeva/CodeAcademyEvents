using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.DTOs
{
    public class ParticipationDto
    {
        public int Id { get; set; }
        public int InvitationId { get; set; }
        public DateTime? CheckInTime { get; set; }
        public int? SeatNumber { get; set; }
    }

}
