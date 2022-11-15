using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebAPI;
using WebAPI.Repositories;


namespace IntegrationTests;


[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration;
    private static ServiceCollection _servicesCollection;

    [OneTimeSetUp]
    public async Task RunOneTimeSetUp()
    {
        _servicesCollection = new ServiceCollection();


        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();


        var HostMock = Mock.Of<IHostingEnvironment>(w =>
            w.EnvironmentName == "Development");
        _servicesCollection.AddSingleton(HostMock);
        _servicesCollection.AddLogging();
        

        Startup.ConfigureServices(_servicesCollection, _configuration);

        var serviceProvider = _servicesCollection.BuildServiceProvider();
        await EnsureDeletedDatabase(serviceProvider);

        var cosmosDbSettings = serviceProvider.GetService<CosmosDbSettings>();
        _servicesCollection.AddScoped(x => new CosmosDbSettings
        {
            DatabaseName = cosmosDbSettings.DatabaseName + Guid.NewGuid(),
            Account = cosmosDbSettings.Account,
            Key = cosmosDbSettings.Key
        });

        serviceProvider.Dispose();
    }

    public static async Task EnsureDatabase(ServiceProvider serviceProvider)
    {
        var client = serviceProvider.GetService<CosmosClient>();
        var dbSettings = serviceProvider.GetService<CosmosDbSettings>();

        var database = await client.CreateDatabaseIfNotExistsAsync(dbSettings.DatabaseName);

        var containers = _configuration.GetSection("Containers")
            .Get<List<ContainerDetails>>();

        foreach (var container in containers)
        {
            await database.Database.CreateContainerIfNotExistsAsync(container.Name, container.PartitionKey);
        }
    }

    public static async Task EnsureDeletedDatabase(ServiceProvider serviceProvider)
    {
        var client = serviceProvider.GetService<CosmosClient>();
        var dbSettings = serviceProvider.GetService<CosmosDbSettings>();

        if (dbSettings.DatabaseName == "uRepo") throw new Exception("Trying to remove uRepo");

        await client.GetDatabase(dbSettings.DatabaseName).DeleteAsync();
    }

    public static async Task<ServiceProvider> CreateStateForNewTest()
    {
        var serviceProvider = _servicesCollection.BuildServiceProvider();

        await EnsureDatabase(serviceProvider);
        return serviceProvider;
    }

    public static async Task TearDownTest(ServiceProvider serviceProvider)
    {
        await EnsureDeletedDatabase(serviceProvider);
        serviceProvider.Dispose();
    }
}
