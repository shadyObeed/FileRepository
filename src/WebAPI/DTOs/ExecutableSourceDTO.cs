using WebAPI.Domain.Entities;
using Domain.Common.Mappings;

namespace WebAPI.DTOs;

public class ExecutableSourceDTO : IMapFrom<ExecutableSource>
{
    public string Location { get; set; }
    public ExecutableSourceTypeDTO Type { get; set; }
}
