using AutoMapper;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Exceptions;
using WebAPI.DTOs;
using WebAPI.Interfaces;

namespace WebAPI.Domain.Services;

public class LauncherService : ILauncherService
{
    private readonly ILaunchersRepository _launchersRepository;

    public LauncherService(ILaunchersRepository launchersRepository)
    {
        _launchersRepository = launchersRepository;
    }

    public async Task<Launcher> AddLauncher(string launcherName)
    {
        var launcher = await _launchersRepository.Get(launcherName);
        if (launcher != Launcher.NotFound)
        {
            throw new LauncherAlreadyExistsException(launcherName);
        }

        var newLauncher = new Launcher(launcherName);

        newLauncher = await _launchersRepository.Add(newLauncher);

        return newLauncher;
    }

    public async Task<Launcher> GetLauncher(string launcherName)
    {
        var launcher = await _launchersRepository.Get(launcherName);

        return launcher;
    }

    public async Task<Launcher> AddCommand(string launcherName, CommandDTO commandDTO)
    {
        var launcher = await _launchersRepository.Get(launcherName);
        if (launcher == Launcher.NotFound)
        {
            throw new LauncherNotFoundException(launcherName);
        }

        var command = Command.CreateFromDTO(commandDTO);
        launcher.AddCommand(command);

        launcher = await _launchersRepository.Update(launcher);

        return launcher;
    }

    public async Task<Command> GetCommand(string launcherName, string commandVerb)
    {
        var launcher = await _launchersRepository.Get(launcherName);
        if (launcher == Launcher.NotFound)
        {
            throw new LauncherNotFoundException(launcherName);
        }

        var command = launcher.GetCommand(commandVerb);

        return command;
    }

    public async Task<Launcher> UpdateCommand(string launcherName, CommandDTO commandToUpdate)
    {
        var launcher = await _launchersRepository.Get(launcherName);
        if (launcher == Launcher.NotFound)
        {
            throw new LauncherNotFoundException(launcherName);
        }

        var command = Command.CreateFromDTO(commandToUpdate);
        launcher.UpdateCommand(command);

        launcher = await _launchersRepository.Update(launcher);

        return launcher;
    }

    public async Task<Launcher> RemoveCommand(string launcherName, CommandDTO commandToRemove)
    {
        var launcher = await _launchersRepository.Get(launcherName);
        if (launcher == Launcher.NotFound)
        {
            throw new LauncherNotFoundException(launcherName);
        }

        var command = Command.CreateFromDTO(commandToRemove);
        launcher.RemoveCommand(command);
        
        launcher = await _launchersRepository.Update(launcher);

        return launcher;
    }
}
