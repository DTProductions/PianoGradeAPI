using Microsoft.EntityFrameworkCore;

namespace PianoGradeAPI {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			builder.Services.AddDbContextPool<GradesContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("GradesDatabase")));

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
