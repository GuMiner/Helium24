internal class TrackerMiddleware
{
    private readonly RequestDelegate _next;

    public TrackerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        return Task.CompletedTask;
    }
}