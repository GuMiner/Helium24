using Microsoft.AspNetCore.Hosting;

namespace H24.Definitions
{
    class Resetter : IResetter
    {
        public Resetter()
        {
        }

        public IApplicationLifetime Lifetime { get; set; }

        public void Reset()
        {
            this.Lifetime.StopApplication();
        }
    }
}
