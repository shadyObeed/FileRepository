using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using WebApi.Utils;

namespace WebApi.Configs
{
    public static class TelemetryConfig
    {
        public static void AddTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITelemetryInitializer, SignalRManagerTelemetryInitializer>();
            var aiOptions = new ApplicationInsightsServiceOptions
            {
                EnableAdaptiveSampling = false,
                ConnectionString = configuration.GetValue<string>("appinsights-constring-NGC", "")
            };
            services.AddApplicationInsightsTelemetry(aiOptions);
        }

    }
}
