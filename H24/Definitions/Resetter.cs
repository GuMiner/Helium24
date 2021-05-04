using Microsoft.Extensions.Hosting;

namespace H24.Definitions
{
    class Resetter : IResetter
    {
        public Resetter()
        {
        }

        public IHostApplicationLifetime Lifetime { get; set; }

        public void Reset()
        {
            this.Lifetime.StopApplication();
        }
    }
}
