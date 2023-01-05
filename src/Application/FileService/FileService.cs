using Application.Exceptions;
using Application.Interfaces;
using Domain.FileProcessor;
using Microsoft.AspNetCore.Http;

namespace Application.FileService;

public class FileService : IFileService
{
    private readonly IFileConverter _fileConverter;
    private readonly IEnumerable<IFileProcessor> _processors;

    public FileService(IFileConverter fileConverter, IEnumerable<IFileProcessor> processors)
    {
        _fileConverter = fileConverter;
        _processors = processors;
    }

    public async Task<IFormFile> ProcessFileAsync(IFormFile file)
    {
        var processedFile = TryProcessFile(file.OpenReadStream());
        return await _fileConverter.ToStreamAsync(processedFile, file.Name);
    }

    private Stream TryProcessFile(Stream stream)
    {
        foreach (var processor in _processors)
        {
            if (processor.SupportsFileType(stream))
            {
                return processor.Process(stream);
            }
        }
        
        throw new UnsupportedFileTypeException($"Unsupported file type");
    }
}