using Domain.Common.Mappings;
using WebAPI.Domain.Entities;

namespace WebAPI.DTOs;

public class ArgumentDTO : IMapFrom<Argument>
{
    public string Name { get; set; }
    public string Alias { get; set; }
    public ArgumentTypeDTO Type { get; set; }
    public string Description { get; set; }
}
