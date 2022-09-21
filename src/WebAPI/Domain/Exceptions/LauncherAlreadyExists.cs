namespace WebAPI.Domain.Exceptions;

public class LauncherAlreadyExistsException : Exception
{
    public LauncherAlreadyExistsException()
        : base()
    {
    }

    public LauncherAlreadyExistsException(string launcherName)
        : base($"Launcher : {launcherName} already exists")
    {
    }

    public LauncherAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
