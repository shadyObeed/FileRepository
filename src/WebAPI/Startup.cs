using System.Reflection;
using Application;
using Domain;
using Infrastructure;
using WebAPI.Configs;
using WebAPI.Filters;
namespace WebAPI;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        //add Infrastructure
        services.AddInfrastructure();
        //add Domain
        services.AddDomain();
        //add Application
        services.AddApplication();
        
        // Add services to the container.
        services.AddControllers(options => options.Filters.Add<ExceptionHandler>());
        services.AddCorsAllowAll();
        services.AddHealthChecks();
        services.AddSwagger();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}