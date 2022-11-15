using System.Reflection;
using WebApi.Configs;
using WebAPI.Domain.Services;
using WebAPI.Filters;
using WebAPI.Interfaces;
using WebAPI.Repositories;
namespace WebAPI;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllers(options => options.Filters.Add<ExceptionHandler>());
        services.AddCorsAllowAll();
        services.AddHealthChecks();
        services.AddSwagger();
        services.AddTelemetry(configuration);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddCosmosdb(configuration);
        services.AddScoped<ILaunchersRepository, LaunchersRepository>();
        services.AddScoped<ILauncherService, LauncherService>();
    }
}