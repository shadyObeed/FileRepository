using WebAPI.Domain.Entities;

namespace WebAPI.Domain.Exceptions;

public class CommandAlreadyExistsException : Exception
{
    public CommandAlreadyExistsException()
        : base()
    {
    }

    public CommandAlreadyExistsException(string launcherName, Command command)
        : base($"Command : {command.Verb} already exists in {launcherName}")
    {
    }

    public CommandAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
