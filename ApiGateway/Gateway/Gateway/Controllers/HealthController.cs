using Gateway.Core.Interfaces.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHealthCore _healthCore;

        public HealthController(IHealthCore healthCore)
        {
            _healthCore = healthCore;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_healthCore.GetHealth());
        }

        [HttpGet("ready")]
        public IActionResult GetReady()
        {
            return Ok(_healthCore.GetReady());
        }

        [HttpGet("live")]
        public IActionResult GetLive()
        {
            return Ok(_healthCore.GetLive());
        }
    }
}
