using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.DTOs
{
    public class EventStatisticsDto
    {
        public int EventId { get; set; }
        public string EventTitle { get; set; } = null!;
        public int TotalInvited { get; set; }
        public int TotalAccepted { get; set; }
        public int TotalRejected { get; set; }
        public int TotalCheckedIn { get; set; }
        public double AverageRating { get; set; }
        public int FeedbackCount { get; set; }
    }
}
