using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CodeAcademyEvents.DAL.Entities;

namespace CodeAcademyEvents.UI.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq mütləqdir")]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        public int LocationId { get; set; }
        public List<Location>? Locations { get; set; }

        [Required]
        public int EventTypeId { get; set; }
        public List<EventType>? EventTypes { get; set; }

        [Required]
        public int OrganizerId { get; set; }
        public List<Organizer>? Organizers { get; set; }
    }

    public class InvitationListItemViewModel
    {
        public int InvitationId { get; set; }
        public string EventTitle { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public InvitationStatus Status { get; set; }
        public bool CanCheckIn { get; set; }
    }

    public class CheckInViewModel
    {
        [Required(ErrorMessage = "Dəvət nömrəsi mütləqdir")]
        public int InvitationId { get; set; }
        public int? SeatNumber { get; set; }
        public string? Message { get; set; }
    }

    public class FeedbackFormViewModel
    {
        [Required]
        public int EventId { get; set; }

        [Required]
        public int PersonId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Reytinq 1 ilə 5 arasında olmalıdır")]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}
