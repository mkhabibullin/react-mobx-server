using ApiGetway.Dto;
using MediatR;

namespace ApiGetway.Queries
{
    public class GetStatusesQuery : IRequest<ServiceStatusDto[]>
    {
    }
}
