using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net;

namespace PianoGradeAPI {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			builder.Services.AddDbContextPool<GradesContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("GradesDatabase")));

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
			builder.Services.AddAuthorization();

			builder.Services.AddIdentity<AppUser, Role>().AddEntityFrameworkStores<GradesContext>().AddDefaultTokenProviders();

			builder.Services.AddTransient<IUserStore<AppUser>, CustomUserStore>();
			builder.Services.AddTransient<IRoleStore<Role>, CustomRoleStore>();

			builder.Services.ConfigureApplicationCookie(o => {
				o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				o.Cookie.MaxAge = TimeSpan.FromMinutes(30);
				o.SlidingExpiration = true;
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
