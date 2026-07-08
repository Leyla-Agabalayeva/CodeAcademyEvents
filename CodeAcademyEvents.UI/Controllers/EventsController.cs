using CodeAcademyEvents.BLL.DTOs;
using CodeAcademyEvents.BLL.Services.Interfaces;
using CodeAcademyEvents.DAL.Repositories;
using CodeAcademyEvents.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademyEvents.UI.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IUnitOfWork _uow;

        public EventsController(IEventService eventService, IUnitOfWork uow)
        {
            _eventService = eventService;
            _uow = uow;
        }

        // GET: /Events
        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllAsync();
            return View(events);
        }

        // GET: /Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev == null) return NotFound();

            if (User.IsInRole("Admin"))
            {
                ViewBag.Persons = _uow.Persons.GetAll().ToList();
            }

            return View(ev);
        }

        // GET: /Events/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var vm = new EventViewModel
            {
                Locations = _uow.Locations.GetAll().ToList(),
                EventTypes = _uow.EventTypes.GetAll().ToList(),
                Organizers = _uow.Organizers.GetAll().ToList()
            };
            return View(vm);
        }

        // POST: /Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(EventViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Locations = _uow.Locations.GetAll().ToList();
                vm.EventTypes = _uow.EventTypes.GetAll().ToList();
                vm.Organizers = _uow.Organizers.GetAll().ToList();
                return View(vm);
            }

            var dto = new EventDto
            {
                Title = vm.Title,
                Description = vm.Description,
                Date = vm.Date,
                LocationId = vm.LocationId,
                EventTypeId = vm.EventTypeId,
                OrganizerId = vm.OrganizerId
            };

            await _eventService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Events/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev == null) return NotFound();

            var vm = new EventViewModel
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                Date = ev.Date,
                LocationId = ev.LocationId,
                EventTypeId = ev.EventTypeId,
                OrganizerId = ev.OrganizerId,
                Locations = _uow.Locations.GetAll().ToList(),
                EventTypes = _uow.EventTypes.GetAll().ToList(),
                Organizers = _uow.Organizers.GetAll().ToList()
            };
            return View("Create", vm);
        }

        // POST: /Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EventViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Locations = _uow.Locations.GetAll().ToList();
                vm.EventTypes = _uow.EventTypes.GetAll().ToList();
                vm.Organizers = _uow.Organizers.GetAll().ToList();
                return View("Create", vm);
            }

            var dto = new EventDto
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                Date = vm.Date,
                LocationId = vm.LocationId,
                EventTypeId = vm.EventTypeId,
                OrganizerId = vm.OrganizerId
            };

            await _eventService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Events/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _eventService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
