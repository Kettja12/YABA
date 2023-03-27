
using Account.Services;
using Account.Model;
using Client.Services;
using YABA.Shared;
using YABA.Shared.Master;

public class AuthorizationFilter : IEndpointFilter
{
    private CacheServices services;
    private ApiService apiService;
    public AuthorizationFilter(CacheServices services,ApiService apiService)
    {
        this.services = services;
        this.apiService = apiService;
    }
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        string? apikey = context.HttpContext.Request.Headers["X-Api-Key"];
        if (string.IsNullOrEmpty(apikey) == false)
        {
            var response = services.GetCacheItem<LoginUser>("LoginUser", apikey);
            if (response== null)
            {
                var user=await apiService.GetAsync<LoginUser>(apikey, "Master/GetLoginUser");
                services.SetCacheItem("LoginUser", apikey, user);

            }
            return await next(context);
        }
        return Results.Unauthorized();
    }
}
