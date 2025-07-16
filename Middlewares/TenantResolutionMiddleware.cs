namespace PUMP_BACKEND.Middlewares;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var host = context.Request.Host.Host;
        var parts = host.Split('.');
        if (parts.Length > 2 || (parts.Length == 2 && host.Contains("localhost")))
        {
            context.Items["Tenant"] = parts[0];
        }

        await _next(context);
    }
}
