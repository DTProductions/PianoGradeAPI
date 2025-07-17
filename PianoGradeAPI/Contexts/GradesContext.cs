using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Entities;

namespace PianoGradeAPI.Contexts
{
    public class GradesContext(DbContextOptions<GradesContext> options) : DbContext(options)
    {

        public DbSet<ComposerEntity> Composers { get; set; }
        public DbSet<PieceEntity> Pieces { get; set; }
        public DbSet<ArrangerEntity> Arrangers { get; set; }
        public DbSet<GradeEntity> Grades { get; set; }
        public DbSet<AppUserEntity> Users { get; set; }
        public DbSet<AppRoleEntity> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PieceEntity>()
                .HasMany(p => p.Composers)
                .WithMany(c => c.Pieces)
                .UsingEntity("piece_composer",
                r => r.HasOne(typeof(ComposerEntity)).WithMany().HasForeignKey("composer_id").HasPrincipalKey(nameof(ComposerEntity.Id)),
                l => l.HasOne(typeof(PieceEntity)).WithMany().HasForeignKey("piece_id").HasPrincipalKey(nameof(PieceEntity.Id)),
                j => j.HasKey("piece_id", "composer_id"));

            modelBuilder.Entity<PieceEntity>()
                .HasMany(p => p.Arrangers)
                .WithMany(a => a.Pieces)
                .UsingEntity("piece_arranger",
                    r => r.HasOne(typeof(ArrangerEntity)).WithMany().HasForeignKey("arranger_id"),
                    l => l.HasOne(typeof(PieceEntity)).WithMany().HasForeignKey("piece_id"),
                    j => j.HasKey("piece_id", "arranger_id"));

            modelBuilder.Entity<PieceEntity>()
                .HasMany(p => p.Grades)
                .WithOne(g => g.Piece)
                .HasForeignKey(g => g.PieceId);

            modelBuilder.Entity<AppUserEntity>()
                .HasMany(u => u.Roles)
                .WithMany()
                .UsingEntity("app_user_app_role",
                    r => r.HasOne(typeof(AppRoleEntity)).WithMany().HasForeignKey("app_role_id"),
                    l => l.HasOne(typeof(AppUserEntity)).WithMany().HasForeignKey("app_user_id"),
                    j => j.HasKey("app_role_id", "app_user_id"));
        }
    }
}
