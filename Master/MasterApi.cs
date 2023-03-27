using Master.Model;
using Master.Services;

namespace Master
{
    public static class MasterApi
    {
        public static void AddMasterApi(this WebApplication app)
        {
            app.MapPost("Login", async (MasterServices services, LoginRequest request) =>
            {
                return Results.Ok(await services.LoginMasterAsync(request));
            });
            app.MapGet("RefreshToken", async (MasterServices services, HttpRequest request) =>
            {
                string? authToken = request.Headers["X-Api-Key"];
                return Results.Ok(services.RefreshToken(authToken));
            })
            .AddEndpointFilter<AuthorizationFilter>();
            
            app.MapGet("GetCurrentUser", async (MasterServices services, HttpRequest request) =>
            {
                string? authToken = request.Headers["X-Api-Key"];
                return Results.Ok(await services.GetCurrentUserAsync(authToken));
            })
            .AddEndpointFilter<AuthorizationFilter>();
            app.MapPost("SetUserDatabase", async (MasterServices services, SeUserDatabaseRequest request, HttpRequest httpRequest) =>
            {
                string? authToken = httpRequest.Headers["X-Api-Key"];
                return Results.Ok(await services.SetUserDatabaseAsync(authToken,request));
            })
            .AddEndpointFilter<AuthorizationFilter>();
        }
    }
}
