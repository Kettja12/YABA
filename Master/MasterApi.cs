using Master.Services;
namespace Master;

public static class MasterApi
{
    public static void AddMasterApi(this WebApplication app)
    {
        app.MapPost("Login", async (MasterServices services, LoginRequest request) =>
        {
            return Results.Ok(await services.LoginMasterAsync(request));
        });

        app.MapPost("RefreshToken", (MasterServices services, ServicesShared.AuthTokenRequest request ) =>
        {
            return Results.Ok(services.RefreshToken(request));
        });

        app.MapPost("Logout", (MasterServices services, ServicesShared.AuthTokenRequest request) =>
        {
            return Results.Ok(services.Logout(request));
        });

        app.MapPost("GetLoginUser", (MasterServices services, ServicesShared.AuthTokenRequest request) =>
        {
            return Results.Ok(services.GetLoginUser(request));
        });

        app.MapPost("SetLoginUserDatabase", (MasterServices services, SeUserDatabaseRequest request) =>
        {
            return Results.Ok(services.SetLoginUserDatabase(request));
        });

        app.MapPost("SavePassword",async (MasterServices services, SavePasswordRequest request, CancellationToken ct) =>
        {
            return Results.Ok(await services.SavePasswordAsync(request,ct));
        });
    }
}
