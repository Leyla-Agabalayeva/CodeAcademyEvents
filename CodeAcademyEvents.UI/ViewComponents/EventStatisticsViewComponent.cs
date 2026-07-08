using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeAcademyEvents.BLL.Services.Interfaces;

namespace CodeAcademyEvents.UI.ViewComponents
{
    public class EventStatisticsViewComponent : ViewComponent
    {
        private readonly IStatisticsService _statisticsService;

        public EventStatisticsViewComponent(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int eventId)
        {
            var stats = await _statisticsService.GetEventStatisticsAsync(eventId);
            return View(stats); // Views/Shared/Components/EventStatistics/Default.cshtml
        }
    }
}
