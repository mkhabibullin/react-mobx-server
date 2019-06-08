using ApiGetway.Dto;
using ApiGetway.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using CodeSuperior.PipelineStyle;

namespace ApiGetway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartbeatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HeartbeatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("status")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ServiceStatusDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ServiceStatusDto>> GetStatus() => (await _mediator.Send(new GetStatusesQuery())).To(Ok);
    }
}