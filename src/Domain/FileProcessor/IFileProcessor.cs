namespace Domain.FileProcessor;

public interface IFileProcessor
{
    Stream Process(Stream input);
    bool SupportsFileType(Stream input);
}