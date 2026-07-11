using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CodeAcademyEvents.BLL.DTOs;
using CodeAcademyEvents.BLL.Services.Interfaces;
using CodeAcademyEvents.DAL.Entities;
using CodeAcademyEvents.DAL.Repositories;
using CodeAcademyEvents.UI.Models;

namespace CodeAcademyEvents.UI.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<IdentityUser> _userManager;

        public FeedbackController(IFeedbackService feedbackService, IUnitOfWork uow, UserManager<IdentityUser> userManager)
        {
            _feedbackService = feedbackService;
            _uow = uow;
            _userManager = userManager;
        }

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

        public async Task<IActionResult> Create(int eventId)
        {
            var personId = await ResolveCurrentPersonIdAsync();
            return View(new FeedbackFormViewModel { EventId = eventId, PersonId = personId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackFormViewModel vm)
        {
            vm.PersonId = await ResolveCurrentPersonIdAsync();

            if (!ModelState.IsValid)
                return View(vm);

            await _feedbackService.SubmitAsync(new FeedbackDto
            {
                EventId = vm.EventId,
                PersonId = vm.PersonId,
                Rating = vm.Rating,
                Comment = vm.Comment
            });

            return RedirectToAction("Details", "Events", new { id = vm.EventId });
        }
    }

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IEventService _eventService;

        public AdminController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var events = await _eventService.GetAllAsync();
            return View(events);
        }
    }
}