using ApiHealth.Core.Interfaces.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiHealth.Controllers
{
    public class HealthServiceController : Controller
    {
        private readonly IHealthServiceCore _healthCore;

        public HealthServiceController(IHealthServiceCore healthCore)
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
