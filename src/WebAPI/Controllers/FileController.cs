//using Application.Commands;

using System.Text;
using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
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
            return "shady";
        }

    }
}