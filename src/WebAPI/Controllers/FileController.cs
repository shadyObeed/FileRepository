//using Application.Commands;

using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/Ingredients")]
    [ApiController]
    public class FileController : Controller
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Sanitize",Name = nameof(Sanitize))]
        public async Task<IFormFile> Sanitize(IFormFile file)
        {
            return await _mediator.Send(new ProcessFileCommand(file){});
        }
        
        [HttpGet("SanitizeGet",Name = nameof(SanitizeGet))]
        public async Task<string> SanitizeGet()
        {
            // Get the path to the desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Create the full path to the file
            string filePath = Path.Combine(desktopPath, "test.txt");

            // Create a new binary file
            using (BinaryWriter writer = new BinaryWriter(System.IO.File.Open(filePath, FileMode.Create)))
            {
                // Write the first 3 bytes (123)
                writer.Write((byte)1);
                writer.Write((byte)2);
                writer.Write((byte)3);

                // Write the last 3 bytes (789)
                writer.Seek(5, SeekOrigin.Begin);
                writer.Write((byte)7);
                writer.Write((byte)8);
                writer.Write((byte)9);
            }

            return "finished";
        }

    }
}