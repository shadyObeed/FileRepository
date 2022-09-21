using WebAPI.Domain.Entities;
using Domain.Common.Mappings;

namespace WebAPI.DTOs;

public class LauncherDTO : IMapFrom<Launcher>
{
    public string Name { get; set; }
    public List<CommandDTO> Commands { get; set; }

    public LauncherDTO() { }

    public LauncherDTO(string launcherName)
    {
        Name = launcherName;
        Commands = new List<CommandDTO>();
    }
}
