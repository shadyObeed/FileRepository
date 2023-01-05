using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.FileService;

public interface IFileService
{
    Task<IFormFile> ProcessFileAsync(IFormFile file);
}