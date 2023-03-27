using Master.Model;
using Master.Services;
using YABA.Shared;
using YABA.Shared.Master;

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
            
            app.MapGet("GetLoginUser", (MasterServices services, HttpRequest request) =>
            {
                string? authToken = request.Headers["X-Api-Key"];
                return Results.Ok(services.GetLoginUser(authToken));
            })
            .AddEndpointFilter<AuthorizationFilter>();
            app.MapPost("SetLoginUserDatabase", (MasterServices services, SeUserDatabaseRequest request, HttpRequest httpRequest) =>
            {
                string? authToken = httpRequest.Headers["X-Api-Key"];
                return Results.Ok(services.SetLoginUserDatabase(authToken,request));
            })
            .AddEndpointFilter<AuthorizationFilter>();
        }
    }
}
