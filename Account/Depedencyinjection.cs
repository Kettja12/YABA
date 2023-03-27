using Account;
using Account.Services;
using Client.Services;
using Microsoft.EntityFrameworkCore;

public static class DependencyInjection
{
    public static IServiceCollection AddInterface(this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        services.AddTransient<InjectionMiddleware>();
        services.AddMemoryCache();
        services.AddScoped<CacheServices>();
        services.AddScoped<AccountServices>();
        string? connectionString = configuration.GetConnectionString("Start");
        if (connectionString == null) throw new Exception("ConnectionString missing.");
        services.AddDbContext<AccountContext>
            (options => options.UseSqlServer(connectionString));
        services.AddScoped(hc => new HttpClient { BaseAddress = new Uri("https://localhost:7000") });
        services.AddScoped<ApiService>();
        return services;

    }
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("https://localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
        return services;
    }
}