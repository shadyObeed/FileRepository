using WebApi.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCorsAllowAll();
builder.Services.AddHealthChecks();
builder.Services.AddSwagger();
builder.Services.AddTelemetry(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
