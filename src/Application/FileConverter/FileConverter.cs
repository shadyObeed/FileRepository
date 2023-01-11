using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace Application.FileConverter;

public class FileConverter : IFileConverter
{
    public Task<IFormFile> ToStreamAsync(Stream streamInput, string fileName, string name, IHeaderDictionary fileHeaders,
        string fileContentDisposition, string fileContentType)
    {
        return Task.FromResult<IFormFile>(new FormFile(streamInput, 0, streamInput.Length, name, fileName){
            Headers = fileHeaders,
            ContentType = fileContentType,
            ContentDisposition = fileContentDisposition
        });
    }
}