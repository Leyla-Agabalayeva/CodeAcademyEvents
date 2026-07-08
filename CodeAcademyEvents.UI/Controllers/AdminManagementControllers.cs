using CodeAcademyEvents.DAL.Entities;
using CodeAcademyEvents.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademyEvents.UI.Controllers
{
    // Bu controller-lər YALNIZ Admin roluna açıqdır.
    // Əvvəllər DbContext-də HasData ilə "hazır" gələn Location/EventType/Organizer/Person
    // indi buradan, sayt üzərindən, admin tərəfindən əlavə olunur.

    [Authorize(Roles = "Admin")]
    public class LocationsController : Controller
    {
        private readonly IUnitOfWork _uow;
        public LocationsController(IUnitOfWork uow) => _uow = uow;

        public IActionResult Index() => View(_uow.Locations.GetAll().ToList());

        public IActionResult Create() => View("Edit", new Location());

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return View(new Location());
            var loc = await _uow.Locations.GetByIdAsync(id);
            if (loc == null) return NotFound();
            return View(loc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Location model)
        {
            if (!ModelState.IsValid) return View("Edit", model);

            if (model.Id == 0)
            {
                await _uow.Locations.AddAsync(model);
            }
            else
            {
                var existing = await _uow.Locations.GetByIdAsync(model.Id);
                if (existing == null) return NotFound();
                existing.Name = model.Name;
                existing.Address = model.Address;
                existing.Capacity = model.Capacity;
                _uow.Locations.Update(existing);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var loc = await _uow.Locations.GetByIdAsync(id);
            if (loc != null)
            {
                _uow.Locations.Remove(loc);
                await _uow.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Admin")]
    public class EventTypesController : Controller
    {
        private readonly IUnitOfWork _uow;
        public EventTypesController(IUnitOfWork uow) => _uow = uow;

        public IActionResult Index() => View(_uow.EventTypes.GetAll().ToList());

        public IActionResult Create() => View("Edit", new EventType());

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return View(new EventType());
            var et = await _uow.EventTypes.GetByIdAsync(id);
            if (et == null) return NotFound();
            return View(et);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(EventType model)
        {
            if (!ModelState.IsValid) return View("Edit", model);

            if (model.Id == 0)
            {
                await _uow.EventTypes.AddAsync(model);
            }
            else
            {
                var existing = await _uow.EventTypes.GetByIdAsync(model.Id);
                if (existing == null) return NotFound();
                existing.Name = model.Name;
                _uow.EventTypes.Update(existing);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var et = await _uow.EventTypes.GetByIdAsync(id);
            if (et != null)
            {
                _uow.EventTypes.Remove(et);
                await _uow.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Admin")]
    public class OrganizersController : Controller
    {
        private readonly IUnitOfWork _uow;
        public OrganizersController(IUnitOfWork uow) => _uow = uow;

        public IActionResult Index() => View(_uow.Organizers.GetAll().ToList());

        public IActionResult Create() => View("Edit", new Organizer());

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return View(new Organizer());
            var org = await _uow.Organizers.GetByIdAsync(id);
            if (org == null) return NotFound();
            return View(org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Organizer model)
        {
            if (!ModelState.IsValid) return View("Edit", model);

            if (model.Id == 0)
            {
                await _uow.Organizers.AddAsync(model);
            }
            else
            {
                var existing = await _uow.Organizers.GetByIdAsync(model.Id);
                if (existing == null) return NotFound();
                existing.FullName = model.FullName;
                existing.Email = model.Email;
                _uow.Organizers.Update(existing);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var org = await _uow.Organizers.GetByIdAsync(id);
            if (org != null)
            {
                _uow.Organizers.Remove(org);
                await _uow.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Admin")]
    public class PersonsController : Controller
    {
        private readonly IUnitOfWork _uow;
        public PersonsController(IUnitOfWork uow) => _uow = uow;

        public IActionResult Index() => View(_uow.Persons.GetAll().ToList());

        public IActionResult Create() => View("Edit", new Person());

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return View(new Person());
            var p = await _uow.Persons.GetByIdAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Person model)
        {
            if (!ModelState.IsValid) return View("Edit", model);

            if (model.Id == 0)
            {
                await _uow.Persons.AddAsync(model);
            }
            else
            {
                var existing = await _uow.Persons.GetByIdAsync(model.Id);
                if (existing == null) return NotFound();
                existing.Name = model.Name;
                existing.Surname = model.Surname;
                existing.Email = model.Email;
                existing.Phone = model.Phone;
                existing.Role = model.Role;
                _uow.Persons.Update(existing);
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _uow.Persons.GetByIdAsync(id);
            if (p != null)
            {
                _uow.Persons.Remove(p);
                await _uow.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
