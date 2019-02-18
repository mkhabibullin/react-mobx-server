using MediatR;

namespace Application.Files.Queries
{
    public class DeleteDirectoryQuery : IRequest
    {
        public string Id { get; set; }

        public DeleteDirectoryQuery(string id)
        {
            Id = id;
        }
    }
}
