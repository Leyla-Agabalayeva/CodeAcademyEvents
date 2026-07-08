using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class Participation : BaseEntity
    {
        public int InvitationId { get; set; }
        public Invitation Invitation { get; set; } = null!;

        public DateTime? CheckInTime { get; set; }
        public int? SeatNumber { get; set; }
    }

}
