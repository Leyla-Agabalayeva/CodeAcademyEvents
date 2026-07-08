using Microsoft.AspNetCore.Identity.UI.Services;

namespace CodeAcademyEvents.UI.Services
{
    // ASP.NET Core Identity-nin daxili Register / Login / ForgotPassword səhifələri
    // MƏHZ bu interfeysdən (Microsoft.AspNetCore.Identity.UI.Services.IEmailSender) istifadə edir.
    //
    // Proqramda bu interfeys heç vaxt DI-a qeydiyyatdan keçirilmədiyi üçün Identity avtomatik
    // olaraq öz "boş" (heç nə etməyən) email sender-indən istifadə edirdi — buna görə də
    // qeydiyyatdan keçən qonağın poçtuna heç vaxt təsdiq məktubu getmirdi.
    //
    // Bu class Identity-nin çağırışını bizim əsl SMTP göndərənimizə (BLL.EmailSender) ötürür.
    public class IdentityEmailSender : IEmailSender
    {
        private readonly CodeAcademyEvents.BLL.Services.Interfaces.IEmailSender _realEmailSender;

        public IdentityEmailSender(CodeAcademyEvents.BLL.Services.Interfaces.IEmailSender realEmailSender)
        {
            _realEmailSender = realEmailSender;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return _realEmailSender.SendEmailAsync(email, subject, htmlMessage);
        }
    }
}
