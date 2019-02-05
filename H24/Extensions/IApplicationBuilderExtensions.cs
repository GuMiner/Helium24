using H24.Middleware;
using Microsoft.AspNetCore.Builder;

namespace H24.Extensions
{
    /// <summary>
    /// Simplify adding the extension to the pipeline
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogger(this IApplicationBuilder builder)
            => builder.UseMiddleware<RequestResponseLogger>();
    }
}
