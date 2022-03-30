using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Configs
{
    public static class CorsConfig
    {
        public static void AddCorsAllowAll(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }

        public static void UseCorsAllowAll(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
        }
    }
}
