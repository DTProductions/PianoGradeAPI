using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace PianoGradeAPI {
	public class GradesContext(DbContextOptions<GradesContext> options) : DbContext(options) {
		public DbSet<Composer> Composers { get; set; }
	}
}
