using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PianoGradeAPI.Controllers {
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase {

		private readonly ILogger<ComposersController> logger;
		private SignInManager<AppUser> signInManager;
		private UserManager<AppUser> userManager;
		private IPasswordHasher<AppUser> passwordHasher;

		public AuthController(ILogger<ComposersController> logger, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher) {
			this.logger = logger;
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.passwordHasher = passwordHasher;
		}

		[HttpGet]
		public async Task<string> Get([FromQuery] string username, [FromQuery] string password) {
			AppUser user = await userManager.FindByNameAsync(username);
			if(user == null) {
				return "Username does not exist";
			}
			
			Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.CheckPasswordSignInAsync(user, password, false);
			if (result.Succeeded) {
				await signInManager.SignInAsync(user, true);
				return "User logged successfully!";
			}
			else {
				return "Could not login user";
			}
		}

		[HttpGet]
		[Route("register")]
		public async Task<string> Register([FromQuery] string username, [FromQuery] string password) {
			if((await userManager.FindByNameAsync(username)) != null) {
				return $"User with username {username} already exists";
			}

			AppUser user = new AppUser() { UserName = username };
			string hashedPassword = passwordHasher.HashPassword(user, password);
			user.PasswordHash = hashedPassword;

			await userManager.CreateAsync(user);
			await userManager.AddToRoleAsync(user, "CONTRIBUTOR");

			return "User registered successfully!";
		}

		[HttpGet]
		[Route("logout")]
		public async Task<string> Logout() {
			await signInManager.SignOutAsync();
			return "User logged out";
		}
	}
}
