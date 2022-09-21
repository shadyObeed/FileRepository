using System.Text.Json.Serialization;

namespace WebAPI.DTOs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExecutableSourceTypeDTO
{
    FileShare,
    Artifactory
}
