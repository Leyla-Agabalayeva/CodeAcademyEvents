using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeAcademyEvents.BLL.DTOs;
using CodeAcademyEvents.BLL.Services.Interfaces;
using CodeAcademyEvents.UI.Models;

namespace CodeAcademyEvents.UI.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: /Feedback/Create?eventId=1&personId=2
        public IActionResult Create(int eventId, int personId)
        {
            return View(new FeedbackFormViewModel { EventId = eventId, PersonId = personId });
        }

        // POST: /Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FeedbackFormViewModel vm)
        {
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

        // GET: /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var events = await _eventService.GetAllAsync();
            return View(events);
        }
    }
}
