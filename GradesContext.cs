using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace PianoGradeAPI {
	public class GradesContext(DbContextOptions<GradesContext> options) : DbContext(options) {
		public DbSet<Composer> Composers { get; set; }
		public DbSet<Piece> Pieces { get; set; }

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
		}
	}
}
