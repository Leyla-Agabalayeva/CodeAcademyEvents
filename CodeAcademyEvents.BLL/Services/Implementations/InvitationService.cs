using AutoMapper;
using CodeAcademyEvents.BLL.DTOs;
using CodeAcademyEvents.BLL.Services.Interfaces;
using CodeAcademyEvents.DAL.Entities;
using CodeAcademyEvents.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.Services.Implementations
{

    public class InvitationService : IInvitationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public InvitationService(IUnitOfWork uow, IMapper mapper, IEmailSender emailSender)
        {
            _uow = uow;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<List<InvitationDto>> GetByPersonIdAsync(int personId)
        {
            var list = await _uow.Invitations.GetAll()
                .Include(i => i.Event)
                .Include(i => i.Person)
                .Where(i => i.PersonId == personId)
                .ToListAsync();

            return _mapper.Map<List<InvitationDto>>(list);
        }

        public async Task<List<InvitationDto>> GetByEventIdAsync(int eventId)
        {
            var list = await _uow.Invitations.GetAll()
                .Include(i => i.Event)
                .Include(i => i.Person)
                .Where(i => i.EventId == eventId)
                .ToListAsync();

            return _mapper.Map<List<InvitationDto>>(list);
        }

        public async Task SendInvitationAsync(int eventId, int personId)
        {
            var alreadyExists = await _uow.Invitations.GetAll()
                .AnyAsync(i => i.EventId == eventId && i.PersonId == personId);
            if (alreadyExists) return;

            var invitation = new Invitation
            {
                EventId = eventId,
                PersonId = personId,
                Status = InvitationStatus.Pending,
                SentAt = DateTime.Now
            };

            await _uow.Invitations.AddAsync(invitation);
            await _uow.SaveChangesAsync();

            var person = await _uow.Persons.GetByIdAsync(personId);
            var ev = await _uow.Events.GetByIdAsync(eventId);
            if (person != null && ev != null)
            {
                await _emailSender.SendEmailAsync(
                    person.Email,
                    $"Dəvət: {ev.Title}",
                    $"Hörmətli {person.Name}, sizi \"{ev.Title}\" tədbirinə dəvət edirik. Tarix: {ev.Date:dd.MM.yyyy HH:mm}.");
            }
        }

        public async Task RespondAsync(int invitationId, bool accept)
        {
            var invitation = await _uow.Invitations.GetByIdAsync(invitationId);
            if (invitation == null) return;

            invitation.Status = accept ? InvitationStatus.Accepted : InvitationStatus.Rejected;
            _uow.Invitations.Update(invitation);
            await _uow.SaveChangesAsync();
        }
    }

}
