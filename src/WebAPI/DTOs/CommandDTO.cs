using WebAPI.Domain.Entities;
using Domain.Common.Mappings;

namespace WebAPI.DTOs;

public class CommandDTO : IMapFrom<Command>
{
    public string Verb { get; set; }
    public List<ArgumentDTO> Arguments { get; set; }
    public string ExecutableName { get; set; }
    public string ExecutableFileName { get; set; }
    public ExecutableSourceDTO Source { get; set; }
    public string Description { get; set; }
}

