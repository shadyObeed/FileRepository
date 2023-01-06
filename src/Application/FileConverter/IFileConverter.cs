using Microsoft.AspNetCore.Http;

namespace Application.FileConverter;

public interface IFileConverter
{
    Task<IFormFile> ToStreamAsync(Stream processedFile, string fileFileName, string fileName, IHeaderDictionary fileHeaders, string fileContentDisposition, string fileContentType);
}