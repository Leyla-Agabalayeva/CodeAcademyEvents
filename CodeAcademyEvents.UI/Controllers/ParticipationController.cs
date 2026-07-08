using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeAcademyEvents.BLL.Services.Interfaces;
using CodeAcademyEvents.UI.Models;

namespace CodeAcademyEvents.UI.Controllers
{
    [Authorize]
    public class ParticipationController : Controller
    {
        private readonly IParticipationService _participationService;

        public ParticipationController(IParticipationService participationService)
        {
            _participationService = participationService;
        }

        // GET: /Participation/CheckIn
        public IActionResult CheckIn(int? invitationId)
        {
            return View(new CheckInViewModel { InvitationId = invitationId ?? 0 });
        }

        // POST: /Participation/CheckIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(CheckInViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var participation = await _participationService.CheckInAsync(vm.InvitationId);
                vm.SeatNumber = participation.SeatNumber;
                vm.Message = $"Check-in uğurludur! Sizin oturacaq nömrəniz: {participation.SeatNumber}";
            }
            catch (InvalidOperationException ex)
            {
                vm.Message = ex.Message;
            }

            return View(vm);
        }
    }
}
