using System.IO;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.FileConverter;

public class FileConverter : IFileConverter
{
    public async Task<IFormFile> ToStreamAsync(string input, string fileName)
    {
        // Convert the input string to a byte array
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);

        // Convert the byte array to a stream
        var stream = new MemoryStream(inputBytes);
        return new FormFile(stream, 0, stream.Length, null, fileName);
    }
    
    public async Task<string> FromStreamAsync(Stream input)
    {
        // Convert the stream to a byte array
        var streamReader = new StreamReader(input);
        return await streamReader.ReadToEndAsync();
    }

}