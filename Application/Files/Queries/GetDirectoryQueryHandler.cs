using Application.Files.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Files.Queries
{
    public class GetDirectoryQueryHandler : IRequestHandler<GetDirectoryQuery, DirectoryDto[]>
    {
        public async Task<DirectoryDto[]> Handle(GetDirectoryQuery request, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(FilesConsts.FilesDir)) return null;
            var folders = Directory.GetDirectories(FilesConsts.FilesDir);

            var result = new List<DirectoryDto>();

            foreach (var f in folders)
            {
                var date = Directory.GetCreationTime(f);

                result.Add(new DirectoryDto(f.Replace(FilesConsts.FilesDir + "\\", "", StringComparison.InvariantCultureIgnoreCase), date));
            }

            return result
                .OrderByDescending(r => r.CreatedAt)
                .ToArray();
        }
    }
}
