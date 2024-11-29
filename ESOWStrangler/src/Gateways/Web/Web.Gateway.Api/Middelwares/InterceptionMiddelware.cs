using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Web.Gateway.Api.Middelwares;

public class InterceptionMiddelware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers["Referer"] = "GatewayApi";
        await next(context);
    }
}