using WebAPI;
using WebApi.Configs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
Startup.ConfigureServices(builder.Services, builder.Configuration);

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