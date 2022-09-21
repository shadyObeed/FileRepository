using AutoMapper;
using Domain.Common.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WebApi.Configs;
using WebAPI.Filters;
using WebAPI.Repositories;
using WebAPI.Domain.Services;
using WebAPI.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
StartUp.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerDeployed();
}
app.UseCorsAllowAll();
app.UseAuthorization();

app.MapControllers();
app.UseHealthChecks("/health");

app.Run();


public static class StartUp
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllers(options =>
            options.Filters.Add<ExceptionHandler>());
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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