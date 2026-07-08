using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CodeAcademyEvents.BLL.Services.Interfaces;
using CodeAcademyEvents.DAL.Entities;
using CodeAcademyEvents.DAL.Repositories;
using CodeAcademyEvents.UI.Models;

namespace CodeAcademyEvents.UI.Controllers
{
    [Authorize]
    public class InvitationsController : Controller
    {
        private readonly IInvitationService _invitationService;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<IdentityUser> _userManager;

        public InvitationsController(IInvitationService invitationService, IUnitOfWork uow, UserManager<IdentityUser> userManager)
        {
            _invitationService = invitationService;
            _uow = uow;
            _userManager = userManager;
        }

        // Cari login olmuş istifadəçiyə uyğun Person tapır, yoxdursa avtomatik yaradır.
        // (FeedbackController-dəki eyni məntiq)
        private async Task<int> ResolveCurrentPersonIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var person = await _uow.Persons.FindAsync(p => p.ApplicationUserId == user!.Id);

            if (person == null)
            {
                person = new Person
                {
                    Name = user!.UserName ?? "İstifadəçi",
                    Surname = "-",
                    Email = user.Email ?? "-",
                    Phone = "-",
                    Role = PersonRole.Qonaq,
                    ApplicationUserId = user.Id
                };
                await _uow.Persons.AddAsync(person);
                await _uow.SaveChangesAsync();
            }

            return person.Id;
        }

        // GET: /Invitations/My — cari istifadəçinin bütün dəvətləri
        public async Task<IActionResult> My()
        {
            var personId = await ResolveCurrentPersonIdAsync();
            var invitations = await _invitationService.GetByPersonIdAsync(personId);

            var vm = invitations.Select(i => new InvitationListItemViewModel
            {
                InvitationId = i.Id,
                EventTitle = i.EventTitle ?? "",
                EventDate = i.EventDate,
                Status = i.Status,
                CanCheckIn = i.Status == InvitationStatus.Accepted
            }).ToList();

            return View(vm);
        }

        // POST: /Invitations/Respond
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(int invitationId, bool accept)
        {
            await _invitationService.RespondAsync(invitationId, accept);
            return RedirectToAction(nameof(My));
        }

        // POST: /Invitations/Send  (Admin tərəfindən dəvət göndərmə)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Send(int eventId, int personId)
        {
            await _invitationService.SendInvitationAsync(eventId, personId);
            return RedirectToAction("Details", "Events", new { id = eventId });
        }
    }
}
