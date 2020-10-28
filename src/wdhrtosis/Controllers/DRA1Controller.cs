using System;
using System.Collections.Generic;
using EA.TaskRunner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using wdhrtosis.Options;
using Microsoft.AspNetCore.Authorization;
using CorrelationId;

namespace wdhrtosis.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ApiController]
    [Authorize(Policy = "read_only")]
    public class DRA1Controller : ControllerBase
    {
   
        private readonly ITask<DRA1> _dratask;
        private readonly ILogger<DRA1Controller> _logger;
        private readonly DRATaskOptions _draTaskOptions;
        private readonly ICorrelationContextAccessor _correlationContext;

        public DRA1Controller(ITask<DRA1> dra1, ILogger<DRA1Controller> logger, DRATaskOptions draTaskOptions, ICorrelationContextAccessor correlationContext)
        {
            _dratask = dra1;
            _logger = logger;
            _draTaskOptions = draTaskOptions;
            _correlationContext = correlationContext;
        }

        // GET: api/DR1
        [HttpGet("", Name = "Help")]
        public IEnumerable<string> Help()
        {
            var ts = _dratask.TaskStatus;

            if (_dratask.TaskStatus.IsRunningRightNow)
            {
                TimeSpan minrun = DateTime.UtcNow - _dratask.TaskStatus.LastRunTime;
                return new[] { _draTaskOptions.TaskDescription +
                                      "(" + ts.LastRunId + ") Process Running - Started:" +
                                      _dratask.TaskStatus.LastRunTime.ToString("g") +
                                      " Current Date Time :" + DateTime.UtcNow.ToString("g") +
                                      " Minutes Running :" + minrun.TotalMinutes };
            }
            return new[] { JsonConvert.SerializeObject(ts) };
        }
        
        [HttpGet("status", Name = "Status")]
        public EATaskStatus Status()
        {
            return _dratask.TaskStatus;
        }

        // POST: api/DR1/[RunID]
        [HttpPost("runtask", Name = "RunTask")]
        [Authorize(Policy = "full_access")]
        public ActionResult<EATaskStatus> RunTask()
        {
            if (_dratask.TaskStatus.IsRunningRightNow)
            {
                return _dratask.TaskStatus;
            }

            try
            {
                _dratask.Start(_correlationContext.CorrelationContext.CorrelationId, "DRA1 Task: WDHRTOSIS");
                return _dratask.TaskStatus;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Task failed to start.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
