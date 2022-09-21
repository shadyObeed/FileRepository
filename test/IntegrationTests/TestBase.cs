using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebAPI.Repositories;

namespace IntegrationTests;


using static Testing;

public class TestBase
{
    protected Fixture _fixture;
    protected ServiceProvider _serviceProvider;

    [SetUp]
    public virtual async Task TestSetUp()
    {
        _fixture = new Fixture();
        _serviceProvider = await CreateStateForNewTest();

        await SpecificTestSetUp();
    }

    public virtual async Task SpecificTestSetUp() { }

    [TearDown]
    public async Task TestTearDown()
    {
        await TearDownTest(_serviceProvider);
    }

    public IQueryable<TEntity> GetQueryable<TEntity>(string containerName)
        where TEntity : EntityBase
    {
        var client = _serviceProvider.GetService<CosmosClient>();
        var dbSetting = _serviceProvider.GetService<CosmosDbSettings>();

        var container = client.GetContainer(dbSetting.DatabaseName, containerName);

        return container.GetItemLinqQueryable<TEntity>(true);
    }

    public async Task AddAsync<TEntity>(string containerName, TEntity entity)
        where TEntity : EntityBase
    {
        var dbSetting = _serviceProvider.GetService<CosmosDbSettings>();

        var client = _serviceProvider.GetService<CosmosClient>();

        var container = client.GetContainer(dbSetting.DatabaseName, containerName);

        await container.CreateItemAsync(entity);
    }

    public async Task AddAsync<TEntity>(string containerName, IEnumerable<TEntity> entities)
        where TEntity : EntityBase
    {
        var client = _serviceProvider.GetService<CosmosClient>();
        var dbSetting = _serviceProvider.GetService<CosmosDbSettings>();

        var container = client.GetContainer(dbSetting.DatabaseName, containerName);

        foreach (var entity in entities)
        {
            await container.CreateItemAsync(entity);
        }

    }

    public async Task<int> CountAsync<TEntity>(string containerName) where TEntity : EntityBase
    {
        var client = _serviceProvider.GetService<CosmosClient>();
        var dbSetting = _serviceProvider.GetService<CosmosDbSettings>();

        var container = client.GetContainer(dbSetting.DatabaseName, containerName);

        return await container.GetItemLinqQueryable<TEntity>().Where(e => e.Type == typeof(TEntity).Name).CountAsync();
    }
}
