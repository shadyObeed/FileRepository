using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Services;
using System.Threading.Tasks;
using Moq;
using WebAPI.Domain.Exceptions;
using WebAPI.DTOs;
using WebAPI.Interfaces;

namespace Domain.UnitTests.Services;

public class LauncherServiceTests : TestBase
{
    private Mock<ILaunchersRepository> _launchersRepository;

    private LauncherService _sut;

    public override void TestSetUp()
    {
        base.TestSetUp();

        _launchersRepository = new Mock<ILaunchersRepository>();
        _sut = new LauncherService(_launchersRepository.Object);
    }

    [Test]
    public async Task AddLauncher_HappyPath()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(Launcher.NotFound);
        _launchersRepository.Setup(x => x.Add(It.IsAny<Launcher>())).ReturnsAsync((Launcher l) =>
        {
            return l;
        });

        //Act
        var actual = await _sut.AddLauncher(launcherName);

        //Assert
        actual.Name.Should().Be(launcherName);
        actual.Commands.Should().BeEmpty();
    }

    [Test]
    public async Task AddLauncher_LauncherAlreadyExists_ShouldThrowLauncherAlreadyExistsException()
    {
        //Arrange
        var launcher = _fixture.Create<Launcher>();

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(launcher);

        //Act + Assert
        await FluentActions.Invoking(() => _sut.AddLauncher(launcher.Name)).Should().ThrowAsync<LauncherAlreadyExistsException>();
    }

    [Test]
    public async Task GetLauncher_HappyPath()
    {
        //Arrange
        var expected = _fixture.Create<Launcher>();

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(expected);

        //Act
        var actual = await _sut.GetLauncher(expected.Name);

        //Assert
        actual.Should().Be(expected);
    }

    [Test]
    public async Task AddCommand_HappyPath()
    {
        //Arrange
        var launcher = _fixture.Create<Launcher>();
        var commandToAddDTO = _fixture.Create<CommandDTO>();

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(launcher);
        _launchersRepository.Setup(x => x.Update(It.IsAny<Launcher>())).ReturnsAsync((Launcher l) =>
        {
            return l;
        });

        //Act
        var actual = await _sut.AddCommand(launcher.Name, commandToAddDTO);

        //Assert
        actual.Name.Should().Be(launcher.Name);
        actual.Commands.Should().Contain(launcher.Commands);
        actual.Commands.Should().Contain(Command.CreateFromDTO(commandToAddDTO));
    }

    [Test]
    public async Task AddCommand_LauncherDoesNotExist_ShouldThrowLauncherNotFoundException()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();
        var commandDTO = _fixture.Create<CommandDTO>();

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(Launcher.NotFound);

        //Act + Assert
        await FluentActions.Invoking(() => _sut.AddCommand(launcherName, commandDTO)).Should().ThrowAsync<LauncherNotFoundException>();
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

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(launcher);

        //Act
        var actual = await _sut.GetCommand(launcher.Name, expected.Verb);

        //Assert
        actual.Should().Be(expected);
    }

    [Test]
    public async Task GetCommand_LauncherDoesNotExist_ShouldThrowLauncherNotFoundException()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();
        var commandVerb = _fixture.Create<string>();

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(Launcher.NotFound);

        //Act + Assert
        await FluentActions.Invoking(() => _sut.GetCommand(launcherName, commandVerb)).Should().ThrowAsync<LauncherNotFoundException>();
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

        var updatedCommand = _fixture.Build<CommandDTO>()
            .With(x => x.Verb, oldCommand.Verb)
            .With(x => x.Arguments, oldCommand.Arguments)
            .Create();
        expectedCommandsList.Add(Command.CreateFromDTO(updatedCommand));

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(launcher);
        _launchersRepository.Setup(x => x.Update(It.IsAny<Launcher>())).ReturnsAsync((Launcher l) =>
        {
            return l;
        });

        //Act
        var actual = await _sut.UpdateCommand(launcher.Name, updatedCommand);

        //Assert
        actual.Name.Should().Be(launcher.Name);
        actual.Commands.Should().Equal(expectedCommandsList);
    }

    [Test]
    public async Task UpdateCommand_LauncherDoesNotExist_ShouldThrowLauncherNotFoundException()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();
        var commandToUpdate = _fixture.Create<CommandDTO>();

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(Launcher.NotFound);

        //Act + Assert
        await FluentActions.Invoking(() => _sut.UpdateCommand(launcherName, commandToUpdate)).Should().ThrowAsync<LauncherNotFoundException>();
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

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(launcher);
        _launchersRepository.Setup(x => x.Update(It.IsAny<Launcher>())).ReturnsAsync((Launcher l) =>
        {
            return l;
        });

        //Act
        var actual = await _sut.RemoveCommand(launcher.Name, commandToRemove);

        //Assert
        actual.Name.Should().Be(launcher.Name);
        actual.Commands.Should().Equal(expectedCommandsList);
    }

    [Test]
    public async Task RemoveCommand_LauncherDoesNotExist_ShouldThrowLauncherNotFoundException()
    {
        //Arrange
        var launcherName = _fixture.Create<string>();
        var commandToRemove = _fixture.Create<CommandDTO>();

        _launchersRepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(Launcher.NotFound);

        //Act + Assert
        await FluentActions.Invoking(() => _sut.RemoveCommand(launcherName, commandToRemove)).Should().ThrowAsync<LauncherNotFoundException>();
    }
}