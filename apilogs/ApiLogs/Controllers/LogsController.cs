using ApiLogs.Core.Interfaces.Core;
using ApiLogs.Model;
using ApiLogs.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLogs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsCore _logsCore;

        public LogsController(ILogsCore logsCore)
        {
            _logsCore = logsCore;
        }

        [HttpGet]
        public IActionResult Get(int pageNumber, int limit)
        {
            return Ok(_logsCore.GetLogs(pageNumber, limit));
        }

        [HttpGet]
        [Route("/api/Logs/{id}")]
        public IActionResult GetLogById(int id)
        {
            return Ok(_logsCore.GetLogById(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] LogsDTO log)
        {
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            return Ok(_logsCore.RegisterLog(log));
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] LogsDTO log)
        {
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            return Ok(_logsCore.UpdateLog(id, log));
        }

        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            return Ok(_logsCore.DeleteLog(id));
        }
    }
}
