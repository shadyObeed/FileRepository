using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using WebApi.Telemetry;

namespace WebApi.Configs;

public static class TelemetryConfig
{
    public static void AddTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var aiOptions = new ApplicationInsightsServiceOptions
        {
            EnableAdaptiveSampling = false,
            InstrumentationKey = configuration.GetValue<string>("appinsights-instrumentationKey-NGC", "")
        };
        services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();
        services.AddApplicationInsightsTelemetry(aiOptions);
    }
}