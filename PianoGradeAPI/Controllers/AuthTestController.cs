using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PianoGradeAPI.Entities;

namespace PianoGradeAPI.Controllers
{
    [Route("[controller]")]
	[ApiController]
	public class AuthTestController : ControllerBase {

		private UserManager<AppUserEntity> userManager;

		public AuthTestController(UserManager<AppUserEntity> userManager) {
			this.userManager = userManager;
		}

		[Authorize]
		[HttpGet]
		public async Task<string> Get() {
			string username = (await userManager.GetUserAsync(HttpContext.User)).UserName;
			return $"You're logged in, {username}!";
		}

		[Authorize(Roles = "CONTRIBUTOR")]
		[HttpGet("contributor")]
		public string GetManager() {
			return "You're a contributor, so logged in!";
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("admin")]
		public string GetAdmin() {
			return "You're an admin, so logged in!";
		}

		[HttpGet("claims")]
		public IActionResult GetClaims() {
			return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
		}
	}
}
