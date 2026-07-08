
using AutoMapper;
using CodeAcademyEvents.BLL.DTOs;
using CodeAcademyEvents.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonDto>().ReverseMap();

            CreateMap<Event, EventDto>()
                .ForMember(d => d.LocationName, o => o.MapFrom(s => s.Location != null ? s.Location.Name : null))
                .ForMember(d => d.EventTypeName, o => o.MapFrom(s => s.EventType != null ? s.EventType.Name : null))
                .ForMember(d => d.OrganizerName, o => o.MapFrom(s => s.Organizer != null ? s.Organizer.FullName : null))
                .ReverseMap()
                .ForMember(d => d.Location, o => o.Ignore())
                .ForMember(d => d.EventType, o => o.Ignore())
                .ForMember(d => d.Organizer, o => o.Ignore());

            CreateMap<Invitation, InvitationDto>()
                .ForMember(d => d.EventTitle, o => o.MapFrom(s => s.Event != null ? s.Event.Title : null))
                .ForMember(d => d.EventDate, o => o.MapFrom(s => s.Event != null ? s.Event.Date : default))
                .ForMember(d => d.PersonFullName, o => o.MapFrom(s => s.Person != null ? s.Person.Name + " " + s.Person.Surname : null))
                .ReverseMap()
                .ForMember(d => d.Event, o => o.Ignore())
                .ForMember(d => d.Person, o => o.Ignore());

            CreateMap<Participation, ParticipationDto>().ReverseMap();
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Feedback, FeedbackDto>().ReverseMap();
        }
    }
}
