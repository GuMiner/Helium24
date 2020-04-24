using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class ShitlerModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("http://helium24.net:8143");
        }
    }
}
