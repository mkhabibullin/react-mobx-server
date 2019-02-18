using Application.Files.Models;
using MediatR;

namespace Application.Files.Queries
{
    public class GetDirectoryQuery : IRequest<DirectoryDto[]>
    {
    }
}
