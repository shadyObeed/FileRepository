using Domain.Entities;
using WebAPI.Domain.Exceptions;

namespace WebAPI.Domain.Entities;

public class Launcher : EntityBase
{
    public IReadOnlyCollection<Command> Commands { get; set; }

    public string PartitionKey => nameof(Launcher);
    

    public static Launcher NotFound = new() { Name = "NOT_FOUND" };


    public Launcher() { }

    public Launcher(string launcherName)
    {
        Name = launcherName;
        Commands = new List<Command>();
    }

    public void AddCommand(Command command)
    {
        var cmd = Commands.FirstOrDefault(c => c.Verb == command.Verb);
        if (cmd is not null)
        {
            throw new CommandAlreadyExistsException(Name, command);
        }

        var commands = Commands.ToList();
        commands.Add(command);
        Commands = commands;
    }

    public Command GetCommand(string commandVerb)
    {
        var command = Commands.FirstOrDefault(c => c.Verb == commandVerb);
        if (command is null)
        {
            throw new CommandNotFoundException(Name, commandVerb);
        }

        return command;
    }

    public void RemoveCommand(Command command)
    {
        var cmd = Commands.FirstOrDefault(c => c.Verb == command.Verb);
        if (cmd is null)
        {
            throw new CommandNotFoundException(Name, command.Verb);
        }

        var commands = Commands.ToList();
        commands.Remove(command);
        Commands = commands;
    }

    public void UpdateCommand(Command command)
    {
        RemoveCommand(command);
        AddCommand(command);
    }
}
