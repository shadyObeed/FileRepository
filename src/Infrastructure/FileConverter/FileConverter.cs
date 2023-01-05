using System.IO;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.FileConverter;

public class FileConverter : IFileConverter
{
    public async Task<IFormFile> ToStreamAsync(Stream input, string fileName)
    {
        // Convert the stream to a string
        var fileStr = await GetStreamAsync(input);
        
        // Convert the input string to a byte array
        byte[] inputBytes = Encoding.ASCII.GetBytes(fileStr);

        // Convert the byte array to a stream
        var stream = new MemoryStream(inputBytes);
        return new FormFile(stream, 0, stream.Length, null, fileName);
    }

    private async Task<string> GetStreamAsync(Stream stream)
    {
        var streamReader = new StreamReader(stream);
        return await streamReader.ReadToEndAsync();
    }
}