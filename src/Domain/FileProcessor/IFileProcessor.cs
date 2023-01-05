namespace Domain.FileProcessor;

public interface IFileProcessor
{
    string Process(Stream input);
    bool SupportsFileType(Stream input);
}