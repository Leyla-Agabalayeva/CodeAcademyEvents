using CodeAcademyEvents.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.BLL.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<EventDto>> GetAllAsync();
        Task<EventDto?> GetByIdAsync(int id);
        Task CreateAsync(EventDto dto);
        Task UpdateAsync(EventDto dto);
        Task DeleteAsync(int id);
    }

    public interface IInvitationService
    {
        Task<List<InvitationDto>> GetByPersonIdAsync(int personId);
        Task<List<InvitationDto>> GetByEventIdAsync(int eventId);
        Task SendInvitationAsync(int eventId, int personId);
        Task RespondAsync(int invitationId, bool accept);
    }

    public interface IParticipationService
    {
        Task<ParticipationDto> CheckInAsync(int invitationId);
        Task<List<ParticipationDto>> GetByEventIdAsync(int eventId);
    }

    public interface INotificationService
    {
        Task SendEventReminderAsync(int eventId, string message);
    }

    public interface IFeedbackService
    {
        Task SubmitAsync(FeedbackDto dto);
        Task<List<FeedbackDto>> GetByEventIdAsync(int eventId);
    }

    public interface IStatisticsService
    {
        Task<EventStatisticsDto> GetEventStatisticsAsync(int eventId);
    }

    // SMTP ilə email göndərmək üçün
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
