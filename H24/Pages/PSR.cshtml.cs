using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class PSRModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Redirect("https://helium24.net:8642");
        }
    }
}
