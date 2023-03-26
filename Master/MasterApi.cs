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
            app.MapPost("RefreshToken", async (MasterServices services, RefreshRequest request) =>
            {
                return Results.Ok(services.RefreshToken(request));
            })
            .AddEndpointFilter<AuthorizationFilter>();
        }
    }
}
