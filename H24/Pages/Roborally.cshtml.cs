using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class RoborallyModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("http://helium24.net:3000");
        }
    }
}
