using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TimeReport.Queries;

namespace TimeReport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : BaseController
    {
        public ReportController()
        {

        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get([FromQuery]GetTimeReportQuery query)
            => Ok(await Mediator.Send(query));
    }
}