using Domain.Common;
using WebAPI.DTOs;

namespace WebAPI.Domain.Entities;

public class Command : ValueObject
{
    public string Verb { get; set; }
    public List<Argument> Arguments { get; set; }
    public string ExecutableName { get; set; }
    public string ExecutableFileName { get; set; }
    public ExecutableSource Source { get; set; }
    public string Description { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Verb;
        foreach (Argument argument in Arguments)
        {
            yield return argument;
        }
    }

    public static Command CreateFromDTO(CommandDTO commandDTO)
    {
        List<Argument> arguments = new List<Argument>();

        foreach (ArgumentDTO argumentDTO in commandDTO.Arguments)
        {
            Argument argument = new Argument
            {
                Name = argumentDTO.Name,
                Alias = argumentDTO.Alias,
                Description = argumentDTO.Description,
                Type = (Enums.ArgumentType)argumentDTO.Type
            };

            arguments.Add(argument);
        }

        Command command = new Command
        {
            Verb = commandDTO.Verb,
            Arguments = arguments,
            ExecutableName = commandDTO.ExecutableName,
            ExecutableFileName = commandDTO.ExecutableFileName,
            Source = new ExecutableSource
            {
                Location = commandDTO.Source.Location,
                Type = (Enums.ExecutableSourceType)commandDTO.Source.Type
            },
            Description = commandDTO.Description
        };

        return command;
    }
}

