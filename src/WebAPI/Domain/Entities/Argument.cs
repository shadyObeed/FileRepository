using Domain.Common;
using Newtonsoft.Json.Converters;
using WebAPI.Domain.Enums;

namespace WebAPI.Domain.Entities;

public class Argument : ValueObject
{
    public string Name { get; set; }
    public string Alias { get; set; }
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public ArgumentType Type { get; set; }
    public string Description { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Type;
    }
}
