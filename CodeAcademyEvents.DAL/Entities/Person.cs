using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public PersonRole Role { get; set; }

        // əgər Identity istifadə edirsinizsə, ApplicationUser-i Person-a bağlamaq üçün:
        public string? ApplicationUserId { get; set; }

        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
