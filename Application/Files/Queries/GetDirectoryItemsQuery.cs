using Application.Files.Models;
using MediatR;

namespace Application.Files.Queries
{
    public class GetDirectoryItemsQuery : IRequest<DirectoryInfoDto>
    {
        public GetDirectoryItemsQuery(string id)
        {
            Id = id;
        }
        
        public string Id { get; set; }
    }
}
