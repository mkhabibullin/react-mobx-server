using Application.Files.Models;
using MediatR;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Files.Queries
{
    public class GetDirectoryItemsQueryHandler : IRequestHandler<GetDirectoryItemsQuery, DirectoryInfoDto>
    {
        public async Task<DirectoryInfoDto> Handle(GetDirectoryItemsQuery request, CancellationToken cancellationToken)
        {
            var path = $"files/{request.Id}/";

            var dto = new DirectoryInfoDto();

            if (Directory.Exists(path))
            {
                dto.Directories = Directory.GetDirectories(path).Select(d => new DirectoryFolderItemModel(d)).ToArray();
                dto.Files = Directory.GetFiles(path).Select(d => new DirectoryFileItemModel(d)).ToArray();
            }

            return dto;
        }
    }
}
