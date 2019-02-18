using MediatR;

namespace Application.Files.Queries
{
    public class GetFileQuery: IRequest<string>
    {
        public string Id { get; set; }

        public string ContentRootPath { get; set; }

        public GetFileQuery(string id, string contentRootPath)
        {
            Id = id;
            ContentRootPath = contentRootPath;
        }
    }
}
