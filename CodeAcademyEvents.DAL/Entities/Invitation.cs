using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class Invitation : BaseEntity
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public DateTime SentAt { get; set; } = DateTime.Now;

        // 1-1 əlaqə: dəvət qəbul edilibsə, check-in zamanı Participation yaranır
        public Participation? Participation { get; set; }
    }
}
