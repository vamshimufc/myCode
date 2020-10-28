using EA.Serilog.Sinks.StaticRolling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using System.Collections.Generic;
using System.Linq;

namespace wdhrtosis.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiController]
    [Authorize(Policy = "read_only")]
    public class LogController : ControllerBase
    {
        // GET api/log
        [HttpGet]
        public List<LogEvent> Get()
        {
            return EASinkSingleton.Instance.logEvents.ToList();
        }
    }
}
