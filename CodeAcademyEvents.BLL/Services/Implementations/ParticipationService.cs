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
    public class ParticipationService : IParticipationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ParticipationService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ParticipationDto> CheckInAsync(int invitationId)
        {
            var invitation = await _uow.Invitations.GetAll()
                .Include(i => i.Participation)
                .FirstOrDefaultAsync(i => i.Id == invitationId);

            if (invitation == null)
                throw new InvalidOperationException("Dəvət tapılmadı.");

            if (invitation.Status != InvitationStatus.Accepted)
                throw new InvalidOperationException("Yalnız dəvəti qəbul edənlər check-in edə bilər.");

            if (invitation.Participation != null)
                return _mapper.Map<ParticipationDto>(invitation.Participation);

            // Həmin tədbir üçün indiyədək verilmiş ən böyük oturacaq nömrəsini tap
            var lastSeat = await _uow.Participations.GetAll()
                .Include(p => p.Invitation)
                .Where(p => p.Invitation.EventId == invitation.EventId)
                .MaxAsync(p => (int?)p.SeatNumber) ?? 0;

            var participation = new Participation
            {
                InvitationId = invitationId,
                CheckInTime = DateTime.Now,
                SeatNumber = lastSeat + 1
            };

            await _uow.Participations.AddAsync(participation);
            await _uow.SaveChangesAsync();

            return _mapper.Map<ParticipationDto>(participation);
        }

        public async Task<List<ParticipationDto>> GetByEventIdAsync(int eventId)
        {
            var list = await _uow.Participations.GetAll()
                .Include(p => p.Invitation)
                .Where(p => p.Invitation.EventId == eventId)
                .ToListAsync();

            return _mapper.Map<List<ParticipationDto>>(list);
        }
    }

}
