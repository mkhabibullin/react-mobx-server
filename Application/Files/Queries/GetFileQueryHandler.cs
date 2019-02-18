using MediatR;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Files.Queries
{
    public class GetFileQueryHandler : IRequestHandler<GetFileQuery, string>
    {
        public async Task<string> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            var tempPath = $"temp";

            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

            var zipPath = $"{tempPath}/{request.Id}.zip";

            if (!File.Exists(zipPath))
            {
                ZipFile.CreateFromDirectory($"{FilesConsts.FilesDir}/{request.Id}", zipPath);
            }

            return Path.Combine(request.ContentRootPath, zipPath);
        }
    }
}
