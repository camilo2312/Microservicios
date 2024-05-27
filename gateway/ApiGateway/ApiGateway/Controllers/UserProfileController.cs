using ApiGateway.Core.Interfaces.Core;
using ApiGateway.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileCore _userProfileService;

        public UserProfileController(IUserProfileCore userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_userProfileService.SaveProfile(userProfile));
        }

        [HttpGet("{userId}")]
        public ActionResult<UserProfile> GetUserProfile(string userId)
        {
            var userProfile = _userProfileService.GetUserProfile(userId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok(userProfile);
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUserProfile(string userId, UserProfile userProfile)
        {
            if (userId != userProfile.UserId)
            {
                return BadRequest();
            }

            _userProfileService.UpdateUserProfile(userId, userProfile);
            return NoContent();
        }
    }
}
