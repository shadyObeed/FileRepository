using Application.Interfaces;
using Infrastructure.FileProcessor;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjection
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<IFileProcessor, FileProcessor>();
    }
}