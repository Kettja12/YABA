using Account.Services;
using Microsoft.EntityFrameworkCore;
using ServicesShared;

public static class DependencyInjection
{
    public static IServiceCollection AddInterface(this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        services.AddMemoryCache();
        services.AddScoped<CacheServices>();
        services.AddScoped<AccountServices>();

        string? connectionString = configuration.GetConnectionString("Start");
        if (connectionString == null) throw new Exception("Star connectionString missing.");
        services.AddDbContext<AccountContext>
            (options => options.UseSqlServer(connectionString));

        string? proxy = configuration.GetConnectionString("Proxy");
        if (proxy == null) throw new Exception("Proxy connectionString missing.");
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(proxy)
        });

        services.AddScoped<ApiService>();
        return services;

    }
}