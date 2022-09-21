using Domain.Common;
using Newtonsoft.Json.Converters;
using WebAPI.Domain.Enums;

namespace WebAPI.Domain.Entities;

public class ExecutableSource : ValueObject
{
    public string Location { get; set; }
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public ExecutableSourceType Type { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Location;
        yield return Type;
    }
}
