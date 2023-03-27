using Account.Model;
using Account.Services;
using YABA.Shared;

public static class AccountApi
{
    public static void AddAccountApi(this WebApplication app)
    {
        app.MapPost("Login", async (HttpContext httpContext, AccountServices services, LoginRequest request, HttpRequest httpRequest) =>
        {
            string? authToken = httpRequest.Headers["X-Api-Key"];
            return Results.Ok(await services.LoginAsync(request,authToken));
        })
        .AddEndpointFilter<AuthorizationFilter>();

        //app.MapPost("RefreshToken", async (MasterServices services, RefreshRequest request) =>
        //{
        //    return Results.Ok(services.RefreshToken(request));
        //})
        //.AddEndpointFilter<AuthorizationFilter>();
    }
}
