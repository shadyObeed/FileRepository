using WebAPI.Domain.Entities;
using WebAPI.DTOs;

namespace WebAPI.Interfaces;

public interface ILauncherService
{
    public Task<Launcher> AddLauncher(string launcherName);
    public Task<Launcher> GetLauncher(string launcherName);
    public Task<Launcher> AddCommand(string launcherName, CommandDTO commandToAdd);
    public Task<Command> GetCommand(string launcherName, string commandVerb);
    public Task<Launcher> UpdateCommand(string launcherName, CommandDTO commandToAdd);
    public Task<Launcher> RemoveCommand(string launcherName, CommandDTO commandToRemove);
}
