using Master;
using Master.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServicesShared;

public static class DependencyInjection
{
    public static IServiceCollection AddInterface(this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        services.AddMemoryCache();
        services.AddScoped<CacheServices>();
        services.AddScoped<MasterServices>();
        string? connectionString = configuration.GetConnectionString("StartMaster");
        if (connectionString == null) throw new Exception("ConnectionString missing.");
        services.AddDbContext<MasterContext>
        (options =>options.UseSqlServer(connectionString,builder =>
        {
            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        }
        ));
        return services;

    }
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        return services;
    }
}