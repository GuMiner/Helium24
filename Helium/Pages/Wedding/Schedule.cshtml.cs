using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Helium.Pages
{
    public class ScheduleModel : PageModel
    {
        private readonly ILogger<ScheduleModel> _logger;

        public ScheduleModel(ILogger<ScheduleModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}