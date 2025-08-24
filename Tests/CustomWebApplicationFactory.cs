using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Contexts;

namespace Tests {
	public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class {
		protected override void ConfigureWebHost(IWebHostBuilder builder) {
			builder.ConfigureServices(services => {
				ServiceDescriptor? descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(GradesContext));

				services.Remove(descriptor);

				services.AddDbContextPool<GradesContext>(opt => opt.UseNpgsql("Host=localhost;Database=piano_grades_test;Username=postgres;Password=123"));
			});
		}
	}
}
