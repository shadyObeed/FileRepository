using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;

namespace Domain.UnitTests.Entities;

public class LauncherServiceTests : TestBase
{
    private Launcher _sut;

    public override void TestSetUp()
    {
        base.TestSetUp();

        _sut = new Launcher("Test");
    }

    [Test]
    public void AddCommand_HappyPath()
    {
        //Arrange
        var commandToAdd = _fixture.Create<Command>();

        //Act
        _sut.AddCommand(commandToAdd);

        //Assert
        _sut.Commands.Should().Contain(commandToAdd);
    }

    [Test]
    public void AddCommand_CommandAlreadyExists_ShouldThrowCommandAlreadyExistsExceptionAsync()
    {
        //Arrange
        var commandToAdd = _fixture.Create<Command>();
        _sut.AddCommand(commandToAdd);

        //Act + Assert
        FluentActions.Invoking(() => _sut.AddCommand(commandToAdd)).Should().Throw<CommandAlreadyExistsException>();
    }

    [Test]
    public void GetCommand_HappyPath()
    {
        //Arrange
        var expected = _fixture.Create<Command>();
        _sut.AddCommand(expected);

        //Act
        var actual = _sut.GetCommand(expected.Verb);

        //Assert
        actual.Should().Be(expected);
    }

    [Test]
    public void GetCommand_CommandNotFound_ShouldThrowCommandNotFoundException()
    {
        //Arrange
        var commandVerb = _fixture.Create<string>();

        //Act + Assert
        FluentActions.Invoking(() => _sut.GetCommand(commandVerb)).Should().Throw<CommandNotFoundException>();
    }

    [Test]
    public void RemoveCommand_HappyPath()
    {
        //Arrange
        var commandToRemove = _fixture.Create<Command>();
        _sut.AddCommand(commandToRemove);

        //Act
        _sut.RemoveCommand(commandToRemove);

        //Assert
        _sut.Commands.Should().NotContain(commandToRemove);
    }

    [Test]
    public void RemoveCommand_CommandNotFound_ShouldThrowCommandNotFoundException()
    {
        //Arrange
        var commandToRemove = _fixture.Create<Command>();

        //Act + Assert
        FluentActions.Invoking(() => _sut.RemoveCommand(commandToRemove)).Should().Throw<CommandNotFoundException>();
    }
}
