using WebAPI.Domain.Entities;

namespace WebAPI.Domain.Exceptions;

public class CommandNotFoundException : Exception
{
    public CommandNotFoundException()
        : base()
    {
    }

    public CommandNotFoundException(string launcherName, string commandVerb)
        : base($"Command : {commandVerb} was not found in {launcherName}")
    {
    }

    public CommandNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}