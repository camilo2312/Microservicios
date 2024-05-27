using ApiGateway.Core.Interfaces.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    public class HealthController : Controller
    {
        private readonly IHealthCore _healthCore;

        public HealthController(IHealthCore healthCore)
        {
            _healthCore = healthCore;
        }

        [HttpGet("health")]
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
