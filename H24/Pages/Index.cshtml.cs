using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H24.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApplicationLifetime lifetime;

        public IndexModel(IApplicationLifetime lifetime)
        {
            this.lifetime = lifetime;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            if (this.HttpContext.User.IsInRole("Admin"))
            {
                this.lifetime.StopApplication();
            }
        }
    }
}
