using System.Reflection;
using Application.FileConverter;
using Application.FileService;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<IFileService,FileService.FileService>();
            services.AddSingleton<IFileConverter,FileConverter.FileConverter>();
        }
    }
}
