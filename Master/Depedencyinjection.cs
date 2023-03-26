using Master;
using Master.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public static class DependencyInjection
{
    public static IServiceCollection AddInterface(this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        services.AddTransient<InjectionMiddleware>();
        services.AddMemoryCache();
        services.AddScoped<CacheServices>();
        services.AddScoped<MasterServices>();
        string? connectionString = configuration.GetConnectionString("StartMaster");
        if (connectionString == null) throw new Exception("ConnectionString missing.");
        services.AddDbContext<MasterContext>
            (options => options.UseSqlServer(connectionString));
        return services;

    }
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("X-Api-Key", new OpenApiSecurityScheme()
            {
                Name = "X-Api-Key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "",
                Scheme = "ApiKeyScheme"
            });

            var key = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-Api-Key"
                },
                In = ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement
                {
                    { key, new List<string>() }
                };
            c.AddSecurityRequirement(requirement);
        });
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