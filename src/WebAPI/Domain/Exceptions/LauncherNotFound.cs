namespace WebAPI.Domain.Exceptions;

public class LauncherNotFoundException : Exception
{
    public LauncherNotFoundException()
        : base()
    {
    }

    public LauncherNotFoundException(string launcherName)
        : base($"Launcher : {launcherName} was not found")
    {

    }

    public LauncherNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
