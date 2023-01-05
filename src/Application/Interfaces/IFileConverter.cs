using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IFileConverter
{
    Task<IFormFile> ToStreamAsync(Stream input, string fileName);
}