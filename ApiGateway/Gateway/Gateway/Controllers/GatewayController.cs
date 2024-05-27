using Gateway.Core.Interfaces.Core;
using Gateway.Core.Services;
using Gateway.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly IApiGatewayCore _apiGatewayService;

        public GatewayController(IApiGatewayCore apiGatewayService)
        {
            _apiGatewayService = apiGatewayService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> AuthenticateUserAsync([FromBody] AuthenticationRequest request)
        { 
            var response = await _apiGatewayService.AuthenticateUserAsync(request.Email, request.Password);

            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUserAsync([FromBody] User user)
        {           

            var response = await _apiGatewayService.RegisterUserAsync(user);            

            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpGet("userprofile")]
        public async Task<ActionResult> GetUserProfileAsync(string userId)
        {           
            var response = await _apiGatewayService.GetUserProfileAsync(userId);

            return StatusCode(200, response);
        }

        [HttpPost("userprofile/update")]
        public async Task<ActionResult> UpdateUserProfileAsync([FromBody] UserProfile request)
        {
            var response = await _apiGatewayService.UpdateUserProfileAsync(request.UserId, request);

            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}
