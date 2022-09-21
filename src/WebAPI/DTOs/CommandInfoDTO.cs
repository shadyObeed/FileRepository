using WebAPI.Domain.Entities;
using Domain.Common.Mappings;

namespace WebAPI.DTOs;

public class CommandInfoDTO : IMapFrom<CommandInfo>
{
    public string Verb { get; set; }
    public string Arguments { get; set; }
}

