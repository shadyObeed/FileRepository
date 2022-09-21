using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.Interfaces;
using FluentAssertions;
using WebAPI.DTOs;

namespace IntegrationTests.Services;

using static Testing;

public class LauncherServiceTests : TestBase
{
    private readonly string _containerName = "Launchers";
    private ILauncherService _sut;


    public override Task SpecificTestSetUp()
    {
        _sut = _serviceProvider.GetService<ILauncherService>();

        return Task.CompletedTask;
    }

    [Test]
    public async Task AddLauncher_HappyPath()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();

        //Act
        var actual = await _sut.AddLauncher(launcherName);

        //Assert
        var actualDB = LaunchersQueryable().Where(x => x.Name == launcherName).ToList().FirstOrDefault();
        var launchersCount = await CountAsync<Launcher>(_containerName);

        actual.Should().BeEquivalentTo(actualDB);

        launchersCount.Should().Be(1);
    }

    [Test]
    public async Task AddLauncher_LauncherAlreadyExists_ShouldThrowLauncherAlreadyExistsException()
    {
        //Arrange
        var launcher = _fixture.Create<Launcher>();

        await AddAsync(_containerName, launcher);

        //Act + Assert
        await FluentActions.Invoking(() => _sut.AddLauncher(launcher.Name)).Should().ThrowAsync<LauncherAlreadyExistsException>();

        //Assert
        var launchersCount = await CountAsync<Launcher>(_containerName);
        launchersCount.Should().Be(1);
    }

    [Test]
    public async Task GetLauncher_HappyPath()
    {
        //Arrange
        var expected = _fixture.Create<Launcher>();

        await AddAsync(_containerName, expected);

        //Act
        var actual = await _sut.GetLauncher(expected.Name);

        //Assert
        var launchersCount = await CountAsync<Launcher>(_containerName);
        var actualDB = LaunchersQueryable().Where(x => x.Name == expected.Name).ToList().FirstOrDefault();

        actual.Name.Should().Be(expected.Name);
        actual.Commands.Should().BeEquivalentTo(expected.Commands);

        actual.Should().BeEquivalentTo(actualDB);

        launchersCount.Should().Be(1);
    }

    [Test]
    public async Task AddCommand_HappyPath()
    {
        //Arrange
        var launcher = _fixture.Create<Launcher>();
        var commandToAdd = _fixture.Create<CommandDTO>();

        await AddAsync(_containerName, launcher);

        //Act
        var actual = await _sut.AddCommand(launcher.Name, commandToAdd);

        //Assert
        var actualDB = LaunchersQueryable().Where(x => x.Name == launcher.Name).ToList().FirstOrDefault();
        var launchersCount = await CountAsync<Launcher>(_containerName);

        actual.Name.Should().Be(launcher.Name);
        actual.Commands.Should().Contain(launcher.Commands);
        actual.Commands.Should().Contain(Command.CreateFromDTO(commandToAdd));

        actual.Should().BeEquivalentTo(actualDB);

        launchersCount.Should().Be(1);
    }
    
    [Test]
    public async Task AddCommand_LauncherDoesNotExist_ShouldThrowLauncherNotFoundException()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();
        var command = _fixture.Create<CommandDTO>();

        //Act + Assert
        await FluentActions.Invoking(() => _sut.AddCommand(launcherName, command)).Should().ThrowAsync<LauncherNotFoundException>();

        //Assert
        var launchersCount = await CountAsync<Launcher>(_containerName);
        launchersCount.Should().Be(0);
    }

    [Test]
    public async Task GetCommand_HappyPath()
    {
        //Arrange
        var expected = _fixture.Create<Command>();
        var commandsList = _fixture.Create<List<Command>>();
        commandsList.Add(expected);

        var launcher = _fixture.Build<Launcher>()
            .With(x => x.Commands, commandsList)
            .Create();

        await AddAsync(_containerName, launcher);

        //Act
        var actual = await _sut.GetCommand(launcher.Name, expected.Verb);

        //Assert
        var actualLauncherDB = LaunchersQueryable().Where(x => x.Name == launcher.Name).ToList().FirstOrDefault();
        var launchersCount = await CountAsync<Launcher>(_containerName);

        actual.Should().Be(expected);

        actualLauncherDB.Commands.Should().Contain(actual);

        launchersCount.Should().Be(1);
    }

    [Test]
    public async Task GetCommand_LauncherDoesNotExist_ShouldThrowLauncherNotFoundException()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();
        var commandVerb = _fixture.Create<string>();

        //Act + Assert
        await FluentActions.Invoking(() => _sut.GetCommand(launcherName, commandVerb)).Should().ThrowAsync<LauncherNotFoundException>();

        //Assert
        var launchersCount = await CountAsync<Launcher>(_containerName);
        launchersCount.Should().Be(0);
    }
    
    [Test]
    public async Task UpdateCommand_HappyPath()
    {
        //Arrange
        var commandsList = _fixture.Create<List<Command>>();
        var expectedCommandsList = new List<Command>(commandsList);

        var oldCommand = _fixture.Create<CommandDTO>();
        commandsList.Add(Command.CreateFromDTO(oldCommand));

        var launcher = _fixture.Build<Launcher>()
            .With(x => x.Commands, commandsList)
            .Create();

        await AddAsync(_containerName, launcher);

        var updatedCommand = _fixture.Build<CommandDTO>()
            .With(x => x.Verb, oldCommand.Verb)
            .With(x => x.Arguments, oldCommand.Arguments)
            .Create();
        expectedCommandsList.Add(Command.CreateFromDTO(updatedCommand));

        //Act
        var actual = await _sut.UpdateCommand(launcher.Name, updatedCommand);

        //Assert
        var actualDB = LaunchersQueryable().Where(x => x.Name == launcher.Name).ToList().FirstOrDefault();
        var launchersCount = await CountAsync<Launcher>(_containerName);

        actual.Name.Should().Be(launcher.Name);
        actual.Commands.Should().Equal(expectedCommandsList);

        actual.Should().BeEquivalentTo(actualDB);

        launchersCount.Should().Be(1);
    }

    [Test]
    public async Task UpdateCommand_LauncherDoesNotExist_ShouldThrowLauncherNotFoundException()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();
        var commandToUpdate = _fixture.Create<CommandDTO>();

        //Act + Assert
        await FluentActions.Invoking(() => _sut.UpdateCommand(launcherName, commandToUpdate)).Should().ThrowAsync<LauncherNotFoundException>();

        //Assert
        var launchersCount = await CountAsync<Launcher>(_containerName);
        launchersCount.Should().Be(0);
    }
    
    [Test]
    public async Task RemoveCommand_HappyPath()
    {
        //Arrange
        var commandsList = _fixture.Create<List<Command>>();
        var expectedCommandsList = new List<Command>(commandsList);

        var commandToRemove = _fixture.Create<CommandDTO>();
        commandsList.Add(Command.CreateFromDTO(commandToRemove));

        var launcher = _fixture.Build<Launcher>()
            .With(x => x.Commands, commandsList)
            .Create();

        await AddAsync(_containerName, launcher);

        //Act
        var actual = await _sut.RemoveCommand(launcher.Name, commandToRemove);

        //Assert
        var actualDB = LaunchersQueryable().Where(x => x.Name == launcher.Name).ToList().FirstOrDefault();
        var launchersCount = await CountAsync<Launcher>(_containerName);

        actual.Name.Should().Be(launcher.Name);
        actual.Commands.Should().Equal(expectedCommandsList);

        actual.Should().BeEquivalentTo(actualDB);

        launchersCount.Should().Be(1);
    }

    [Test]
    public async Task RemoveCommand_LauncherDoesNotExist_ShouldThrowLauncherNotFoundException()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();
        var commandToRemove = _fixture.Create<CommandDTO>();

        //Act + Assert
        await FluentActions.Invoking(() => _sut.RemoveCommand(launcherName, commandToRemove)).Should().ThrowAsync<LauncherNotFoundException>();

        //Assert
        var launchersCount = await CountAsync<Launcher>(_containerName);
        launchersCount.Should().Be(0);
    }

    private IQueryable<Launcher> LaunchersQueryable()
    {
        return GetQueryable<Launcher>(_containerName);
    }
}
