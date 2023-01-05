using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.FileService;

public class FileService : IFileService
{
    private readonly IFileConverter _fileConverter;
    private readonly IFileProcessor _fileProcessor;

    public FileService(IFileConverter fileConverter, IFileProcessor fileProcessor)
    {
        _fileConverter = fileConverter;
        _fileProcessor = fileProcessor;
    }

    public async Task<IFormFile> ProcessFileAsync(IFormFile file)
    {
        // Convert the file to a string
        string fileContent = await _fileConverter.FromStreamAsync(file.OpenReadStream());

        // Process the file
        string processedContent = _fileProcessor.Process(fileContent);

        // Convert the processed string back to a stream
        return await _fileConverter.ToStreamAsync(processedContent, file.Name);
    }

}