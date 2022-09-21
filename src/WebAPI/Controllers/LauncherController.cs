using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Domain.Entities;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Repositories;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LaunchersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILauncherService _launchersService;

    public LaunchersController(IMapper mapper, ILauncherService launchersService)
    {
        _mapper = mapper;
        _launchersService = launchersService;
    }

    [HttpPost("{launcherName}/Add", Name = nameof(AddLauncher))]
    public async Task<LauncherDTO> AddLauncher(string launcherName)
    {
        var launcher = await _launchersService.AddLauncher(launcherName);

        return _mapper.Map<LauncherDTO>(launcher);
    }

    [HttpGet("{launcherName}", Name = nameof(GetLauncher))]
    public async Task<LauncherDTO> GetLauncher(string launcherName)
    {
        var launcher = await _launchersService.GetLauncher(launcherName);

        return _mapper.Map<LauncherDTO>(launcher);
    }

    //Commands API
    [HttpPost("{launcherName}/commands/Add", Name = nameof(AddCommand))]
    public async Task<LauncherDTO> AddCommand(string launcherName, [FromBody] CommandDTO commandDTO)
    {
        var launcher = _launchersService.AddCommand(launcherName, commandDTO);

         return _mapper.Map<LauncherDTO>(launcher);
    }

    [HttpDelete("{launcherName}/commands/Remove", Name = nameof(RemoveCommand))]
    public async Task<LauncherDTO> RemoveCommand(string launcherName, [FromBody] CommandDTO commandDTO)
    {
        var launcher = await _launchersService.RemoveCommand(launcherName, commandDTO);

        return _mapper.Map<LauncherDTO>(launcher);
    }

    [HttpGet("{launcherName}/commands/{commandVerb}", Name = nameof(GetCommand))]
    public async Task<CommandDTO> GetCommand(string launcherName, string commandVerb)
    {
        var command = await _launchersService.GetCommand(launcherName, commandVerb);

        return _mapper.Map<CommandDTO>(command);
    }

    [HttpPost("{launcherName}/commands/Parse", Name = nameof(ParseCommand))]
    public async Task<CommandDTO> ParseCommand(string launcherName, [FromBody] CommandInfoDTO commandInfoDTO)
    {
        var command = await _launchersService.GetCommand(launcherName, commandInfoDTO.Verb);

        return _mapper.Map<CommandDTO>(command);
    }

    [HttpPut("{launcherName}/commands/Update", Name = nameof(UpdateCommand))]
    public async Task<LauncherDTO> UpdateCommand(string launcherName, [FromBody] CommandDTO commandDTO)
    {
        var launcher = await _launchersService.UpdateCommand(launcherName, commandDTO);

        return _mapper.Map<LauncherDTO>(launcher);
    }
}
