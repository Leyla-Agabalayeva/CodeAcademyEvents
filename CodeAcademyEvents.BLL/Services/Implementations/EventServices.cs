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
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public EventService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<EventDto>> GetAllAsync()
        {
            var events = await _uow.Events.GetAll()
                .Include(e => e.Location)
                .Include(e => e.EventType)
                .Include(e => e.Organizer)
                .ToListAsync();

            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<EventDto?> GetByIdAsync(int id)
        {
            var ev = await _uow.Events.GetAll()
                .Include(e => e.Location)
                .Include(e => e.EventType)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == id);

            return ev == null ? null : _mapper.Map<EventDto>(ev);
        }

        public async Task CreateAsync(EventDto dto)
        {
            var entity = _mapper.Map<Event>(dto);
            await _uow.Events.AddAsync(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(EventDto dto)
        {
            var entity = await _uow.Events.GetByIdAsync(dto.Id);
            if (entity == null) return;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Date = dto.Date;
            entity.LocationId = dto.LocationId;
            entity.EventTypeId = dto.EventTypeId;
            entity.OrganizerId = dto.OrganizerId;

            _uow.Events.Update(entity);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.Events.GetByIdAsync(id);
            if (entity == null) return;

            _uow.Events.Remove(entity);
            await _uow.SaveChangesAsync();
        }
    }

}
