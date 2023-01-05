using System.Threading;
using System.Threading.Tasks;
using Application.FileService;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands;

public class ProcessFileCommand : IRequest<IFormFile>
{
    public IFormFile File { get; }

    public ProcessFileCommand(IFormFile file)
    {
        File = file;
    }
}

public class ProcessFileCommandHandler : IRequestHandler<ProcessFileCommand, IFormFile>
{
    private readonly IFileService _fileService;

    public ProcessFileCommandHandler(IFileService fileService)
    {
        _fileService = fileService;
    }

    public async Task<IFormFile> Handle(ProcessFileCommand request, CancellationToken cancellationToken)
    {
        return await _fileService.ProcessFileAsync(request.File);
    }
}

