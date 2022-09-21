using Microsoft.Azure.Cosmos;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Interfaces;
using WebAPI.Domain.Services;

namespace WebAPI.Repositories;

public class LaunchersRepository : ILaunchersRepository
{
    private readonly Container _launchersContainer;
    private readonly string _containerName = "Launchers";
    
    public LaunchersRepository(CosmosClient client, CosmosDbSettings cosmosDbSettings)
    {
        _launchersContainer = client.GetContainer(cosmosDbSettings.DatabaseName, _containerName);
    }

    public async Task<Launcher> Add(Launcher launcher)
    {
        try
        {
            return await _launchersContainer.CreateItemAsync(launcher);
        }
        catch
        {
            throw new LauncherAlreadyExistsException(launcher.Name);
        }

    }

    public async Task<Launcher> Get(string launcherName)
    {
        try
        {
            return await _launchersContainer.ReadItemAsync<Launcher>(launcherName, new PartitionKey(nameof(Launcher)));
        }
        catch
        {
            return Launcher.NotFound;
        }
    }

    
    public async Task<Launcher> Update(Launcher launcher)
    {
        try
        {
            var options = new ItemRequestOptions { IfMatchEtag = launcher.Etag };
            return await _launchersContainer.ReplaceItemAsync(launcher, launcher.Name, new PartitionKey(launcher.PartitionKey), options);
        }
        catch
        {
            return Launcher.NotFound;
        }
    }
}
