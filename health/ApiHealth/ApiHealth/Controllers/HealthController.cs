using ApiHealth.Core.Interfaces.Core;
using ApiHealth.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiHealth.Controllers
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

        [HttpPost]
        public IActionResult Post([FromBody]RequestService requestService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            return Ok(_healthCore.SaveService(requestService));
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_healthCore.GetHealthServices(false));
        }

        [HttpGet]
        [Route("/api/Health/{serviceName}")]
        public IActionResult GetHealthServiceByName(string serviceName) 
        {
            return Ok(_healthCore.GetHealthServiceByName(serviceName));
        }

    }
}
