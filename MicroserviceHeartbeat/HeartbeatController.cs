using Microsoft.AspNetCore.Mvc;

namespace MicroserviceHeartbeat
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartbeatController : ControllerBase
    {
        [HttpGet]
        public ActionResult<bool> Ping() => Ok(true);
    }
}
