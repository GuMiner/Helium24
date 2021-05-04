using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace H24.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHostApplicationLifetime lifetime;

        public IndexModel(IHostApplicationLifetime lifetime)
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
