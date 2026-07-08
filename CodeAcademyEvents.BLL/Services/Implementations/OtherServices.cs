using AutoMapper;
using CodeAcademyEvents.BLL.DTOs;
using CodeAcademyEvents.BLL.Services.Interfaces;
using CodeAcademyEvents.DAL.Entities;
using CodeAcademyEvents.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CodeAcademyEvents.BLL.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailSender _emailSender;

        public NotificationService(IUnitOfWork uow, IEmailSender emailSender)
        {
            _uow = uow;
            _emailSender = emailSender;
        }

        public async Task SendEventReminderAsync(int eventId, string message)
        {
            var notification = new Notification
            {
                EventId = eventId,
                Message = message,
                SentAt = DateTime.Now
            };
            await _uow.Notifications.AddAsync(notification);
            await _uow.SaveChangesAsync();

            // Yalnız dəvəti qəbul edənlərə bildiriş göndər
            var acceptedInvitations = await _uow.Invitations.GetAll()
                .Include(i => i.Person)
                .Where(i => i.EventId == eventId && i.Status == InvitationStatus.Accepted)
                .ToListAsync();

            var reminderBody = "<h2 style=\"margin:0 0 12px;color:#0b3d91;font-size:20px;\">Tedbir xatirlatmasi</h2>"
                     + "<p style=\"margin:0;\">" + message + "</p>";

            foreach (var inv in acceptedInvitations)
            {
                await _emailSender.SendEmailAsync(inv.Person.Email, "Tədbir Xatırlatması", reminderBody);
            }
        }
    }

    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task SubmitAsync(FeedbackDto dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentOutOfRangeException(nameof(dto.Rating), "Reytinq 1-5 aralığında olmalıdır.");

            var feedback = _mapper.Map<Feedback>(dto);
            feedback.SubmittedAt = DateTime.Now;
            await _uow.Feedbacks.AddAsync(feedback);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<FeedbackDto>> GetByEventIdAsync(int eventId)
        {
            var list = await _uow.Feedbacks.GetAll()
                .Where(f => f.EventId == eventId)
                .ToListAsync();

            return _mapper.Map<List<FeedbackDto>>(list);
        }
    }

    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _uow;

        public StatisticsService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<EventStatisticsDto> GetEventStatisticsAsync(int eventId)
        {
            var ev = await _uow.Events.GetByIdAsync(eventId);
            var invitations = await _uow.Invitations.GetAll()
                .Where(i => i.EventId == eventId)
                .ToListAsync();

            var checkedInCount = await _uow.Participations.GetAll()
                .Include(p => p.Invitation)
                .CountAsync(p => p.Invitation.EventId == eventId && p.CheckInTime != null);

            var feedbacks = await _uow.Feedbacks.GetAll()
                .Where(f => f.EventId == eventId)
                .ToListAsync();

            return new EventStatisticsDto
            {
                EventId = eventId,
                EventTitle = ev?.Title ?? "",
                TotalInvited = invitations.Count,
                TotalAccepted = invitations.Count(i => i.Status == InvitationStatus.Accepted),
                TotalRejected = invitations.Count(i => i.Status == InvitationStatus.Rejected),
                TotalCheckedIn = checkedInCount,
                FeedbackCount = feedbacks.Count,
                AverageRating = feedbacks.Any() ? Math.Round(feedbacks.Average(f => f.Rating), 2) : 0
            };
        }
    }

    // SMTP ilə email göndərmə. Bütün məktublar eyni brendli HTML şablonu ilə göndərilir.
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpSection = _config.GetSection("SmtpSettings");
            var host = smtpSection["Host"];
            var port = int.Parse(smtpSection["Port"] ?? "587");
            var username = smtpSection["Username"];
            var password = smtpSection["Password"];
            var from = smtpSection["From"];

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("SMTP parametrleri appsettings.json-da doldurulmayib.");
            }

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(from ?? username));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = BuildHtmlTemplate(body) }.ToMessageBody();

            using var client = new SmtpClient();

            // Port 465 tələb edir ki, TLS bağlantının lap əvvəlindən qurulsun (SslOnConnect).
            // Port 587 (Gmail üçün standart) əvvəlcə açıq bağlantı açır, sonra STARTTLS ilə şifrələnir.
            var socketOptions = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

            await client.ConnectAsync(host, port, socketOptions);
            await client.AuthenticateAsync(username, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private static string BuildHtmlTemplate(string innerHtml)
        {
            var year = DateTime.Now.Year;
            return "<!DOCTYPE html><html lang=\"az\"><body style=\"margin:0;padding:0;background-color:#eef2f9;font-family:'Segoe UI',Arial,sans-serif;\">"
                 + "<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color:#eef2f9;padding:32px 0;\"><tr><td align=\"center\">"
                 + "<table role=\"presentation\" width=\"480\" cellpadding=\"0\" cellspacing=\"0\" style=\"background:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 4px 18px rgba(20,50,110,0.10);\">"
                 + "<tr><td style=\"background:linear-gradient(135deg,#0b3d91,#1d6fe0);padding:26px 32px;\"><span style=\"color:#ffffff;font-size:20px;font-weight:600;letter-spacing:.3px;\">CodeAcademy Events</span></td></tr>"
                 + "<tr><td style=\"padding:32px;color:#1f2a44;font-size:15px;line-height:1.65;\">" + innerHtml + "</td></tr>"
                 + "<tr><td style=\"padding:16px 32px;background:#f4f7fc;color:#8592ad;font-size:12px;\">&copy; " + year + " CodeAcademy Events &middot; Bu, avtomatik g\u00f6ndərilən mesajdır, xahiş edirik cavablandırmayın.</td></tr>"
                 + "</table></td></tr></table></body></html>";
        }
    }

}
