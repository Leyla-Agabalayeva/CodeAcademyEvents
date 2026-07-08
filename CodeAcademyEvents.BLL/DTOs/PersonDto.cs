using CodeAcademyEvents.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.DTOs
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public PersonRole Role { get; set; }
    }
}
