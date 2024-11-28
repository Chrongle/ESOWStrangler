using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SharedLibrary.Middleware;

public class RestrictedAccessMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers["Referer"].FirstOrDefault();
        if (context.Request.Headers["Referer"] != "GatewayApi")
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Forbidden");
            return;
        }
        await next(context);
    }
}
