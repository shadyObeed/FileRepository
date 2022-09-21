using Microsoft.Azure.Cosmos;

namespace WebAPI.Repositories;

public static class CosmosConfig
{
    public static void AddCosmosdb(this IServiceCollection services, IConfiguration configuration)
    {
        var cosmosDb = configuration.GetSection(nameof(CosmosDbSettings)).Get<CosmosDbSettings>();
        services.AddSingleton(cosmosDb);
        var containers = configuration.GetSection("Containers")
            .Get<List<ContainerDetails>>();

        var client = CreateCosmosClient(configuration);
        CreateDatabase(client, containers, cosmosDb).Wait();
        services.AddSingleton(client);
    }

    private static CosmosClient CreateCosmosClient(IConfiguration configuration)
    {
        string account = configuration.GetValue<string>("NGC-cosmosdb-documentEndpoint");
        var key = configuration.GetValue<string>("NGC-cosmosdb-accountKey");
        var options = new CosmosClientOptions() { ConnectionMode = ConnectionMode.Gateway };
        CosmosClient client;
        if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(key))
        {
            var configSec = configuration.GetSection("CosmosDbSettings");
            var endpoint = configSec.GetSection("Account").Value;
            var cred = configSec.GetSection("Key").Value;
            client = new CosmosClient(endpoint, cred);
        }
        else
        {
            client = new CosmosClient(account, key, options);
        }
        return client;
    }

    private static async Task CreateDatabase(CosmosClient client, List<ContainerDetails> containersDetails, CosmosDbSettings cosmosDbSettings)
    {
        DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(cosmosDbSettings.DatabaseName);
        foreach (var containerDetails in containersDetails)
        {
            await database.Database.CreateContainerIfNotExistsAsync(containerDetails.Name, containerDetails.PartitionKey);
        }
    }
}
