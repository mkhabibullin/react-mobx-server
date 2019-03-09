using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Files.Queries
{
    public class DeleteDirectoryQueryHandler : IRequestHandler<DeleteDirectoryQuery>
    {
        public async Task<Unit> Handle(DeleteDirectoryQuery request, CancellationToken cancellationToken)
        {
            var path = $"{FilesConsts.FilesDir}/{request.Id}";

            if (Directory.Exists(path)) Directory.Delete(path, true);

            return Unit.Value;
        }
    }
}
