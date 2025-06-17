using Microsoft.EntityFrameworkCore;

namespace PianoGradeAPI {
	public class GradesContext(DbContextOptions<GradesContext> options) : DbContext(options) {

		public DbSet<Composer> Composers { get; set; }
		public DbSet<Piece> Pieces { get; set; }
		public DbSet<Arranger> Arrangers { get; set; }
		public DbSet<Grade> Grades { get; set; }
		public DbSet<AppUser> Users { get; set; }
		public DbSet<AppRole> Roles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Piece>()
				.HasMany(p => p.Composers)
				.WithMany(c => c.Pieces)
				.UsingEntity("piece_composer",
				r => r.HasOne(typeof(Composer)).WithMany().HasForeignKey("composer_id").HasPrincipalKey(nameof(Composer.Id)),
				l => l.HasOne(typeof(Piece)).WithMany().HasForeignKey("piece_id").HasPrincipalKey(nameof(Piece.Id)),
				j => j.HasKey("piece_id", "composer_id"));

			modelBuilder.Entity<Piece>()
				.HasMany(p => p.Arrangers)
				.WithMany(a => a.Pieces)
				.UsingEntity("piece_arranger",
					r => r.HasOne(typeof(Arranger)).WithMany().HasForeignKey("arranger_id"),
					l => l.HasOne(typeof(Piece)).WithMany().HasForeignKey("piece_id"),
					j => j.HasKey("piece_id", "arranger_id"));
			
			modelBuilder.Entity<Piece>()
				.HasMany(p => p.Grades)
				.WithOne(g => g.Piece)
				.HasForeignKey(g => g.PieceId);

			modelBuilder.Entity<AppUser>()
				.HasMany(u => u.Roles)
				.WithMany()
				.UsingEntity("app_user_app_role",
					r => r.HasOne(typeof(AppRole)).WithMany().HasForeignKey("role_id"),
					l => l.HasOne(typeof(AppUser)).WithMany().HasForeignKey("app_user_id"),
					j => j.HasKey("role_id", "app_user_id"));
		}
	}
}
