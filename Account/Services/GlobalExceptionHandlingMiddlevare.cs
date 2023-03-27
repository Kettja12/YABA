using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
namespace Account.Services;
public class GlobalExceptionHandlingMiddleware:IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> logger;

    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        this.logger = logger;       
    }

    public async Task InvokeAsync(HttpContext context,RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, ex.Message);
            context.Response.StatusCode= (int)HttpStatusCode.InternalServerError;

        }
    }
}
