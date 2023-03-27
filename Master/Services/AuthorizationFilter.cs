
using Master.Model;
using Master.Services;
using YABA.Shared.Master;

public class AuthorizationFilter : IEndpointFilter
{
    private CacheServices services;
    public AuthorizationFilter(CacheServices services)
    {
        this.services = services;
    }
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        string? apikey = context.HttpContext.Request.Headers["X-Api-Key"];
        if (string.IsNullOrEmpty(apikey) == false)
        {
            var response = services.GetCacheItem<LoginUser>("LoginUser", apikey);
            if (response!= null)
            return await next(context);
        }
        return Results.Unauthorized();
    }
}
