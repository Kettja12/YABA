using Account;
using Account.Services;
using ServicesShared;

public static class AccountApi
{
    public static void AddAccountApi(this WebApplication app)
    {
        app.MapPost("Login", async (AccountServices services, AuthTokenRequest request) =>
        {
            return Results.Ok(await services.LoginAsync(request));
        });

        app.MapPost("Logout", (AccountServices services, AuthTokenRequest request) =>
        {
            return Results.Ok(services.Logout(request));
        });
        app.MapPost("SaveUser", async (AccountServices services, AccountServices.SaveUserRequest request, CancellationToken ct) =>
        {
            return Results.Ok(await services.SaveUserAsync(request, ct));
        });

        app.MapPost("LoadUsergroups", async (AccountServices services, AuthTokenRequest request, CancellationToken ct) =>
        {
            return Results.Ok(await services.LoadUsergroupsAsync(request, ct));
        });
        app.MapPost("LoadFunctions", async (AccountServices services, AuthTokenRequest request, CancellationToken ct) =>
        {
            return Results.Ok(await services.LoadFunctionsAsync(request, ct));
        });
        app.MapPost("LoadUsergroupFunctions", async (AccountServices services, SimpleRequest request, CancellationToken ct) =>
        {
            return Results.Ok(await services.LoadUsergroupFunctionsAsync(request, ct));
        });

        app.MapPost("LoadUsergroupUsers", async (AccountServices services, SimpleRequest request, CancellationToken ct) =>
        {
            return Results.Ok(await services.LoadUsergroupUsersAsync(request, ct));
        });
        app.MapPost("LoadUsersNotInUsergroup", async (AccountServices services, SimpleRequest request, CancellationToken ct) =>
        {
            return Results.Ok(await services.LoadUsersNotInUsergroupAsync(request, ct));
        });

        
    }
}
