using Account.Model;
using Account.Services;

public static class AccountApi
{
    public static void AddAccountApi(this WebApplication app)
    {
        app.MapPost("Login", async (HttpContext httpContext, AccountServices services, LoginRequest request) =>
        {
            return Results.Ok(await services.LoginAsync(request));
        })
        .AddEndpointFilter<AuthorizationFilter>();

        //app.MapPost("RefreshToken", async (MasterServices services, RefreshRequest request) =>
        //{
        //    return Results.Ok(services.RefreshToken(request));
        //})
        //.AddEndpointFilter<AuthorizationFilter>();
    }
}
