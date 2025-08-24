using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Contexts;
using PianoGradeAPI.Entities;

namespace Tests {
	public class DatabaseSeeder {
		public static async Task Seed(GradesContext gradesContext) {
			SeedComposers(gradesContext);
			SeedArrangers(gradesContext);
			SeedPieces(gradesContext);
			await gradesContext.SaveChangesAsync();

			SeedRelationships(gradesContext);
			await gradesContext.SaveChangesAsync();
		}

		private static void SeedComposers(GradesContext gradesContext) {
			ComposerEntity john = new ComposerEntity() {
				Name = "john",
				Era = "baroque",
				Nationality = "austrian"
			};
			gradesContext.Composers.Add(john);

			ComposerEntity mary = new ComposerEntity() {
				Name = "mary",
				Era = "romantic",
				Nationality = "german"
			};
			gradesContext.Composers.Add(mary);

			ComposerEntity alice = new ComposerEntity() {
				Name = "alice",
				Era = "modern",
				Nationality = "italian"
			};
			gradesContext.Composers.Add(alice);

			ComposerEntity michael = new ComposerEntity() {
				Name = "michael",
				Era = "romantic",
				Nationality = "french"
			};
			gradesContext.Composers.Add(michael);
		}

		private static void SeedArrangers(GradesContext gradesContext) {
			ArrangerEntity bob = new ArrangerEntity() { Name = "bob" };
			gradesContext.Arrangers.Add(bob);

			ArrangerEntity charles = new ArrangerEntity() { Name = "charles" };
			gradesContext.Arrangers.Add(charles);

			ArrangerEntity tom = new ArrangerEntity() { Name = "tom" };
			gradesContext.Arrangers.Add(tom);
		}

		private static void SeedPieces(GradesContext gradesContext) {
			PieceEntity nocturne = new PieceEntity() {
				Title = "nocturne",
				Grades = new List<GradeEntity> { CreateGrade("abrsm", "5") }
			};
			gradesContext.Pieces.Add(nocturne);

			PieceEntity sonata = new PieceEntity() {
				Title = "sonata",
				Grades = new List<GradeEntity> {
					CreateGrade("abrsm", "3"),
					CreateGrade("rcm", "5")
				}
			};
			gradesContext.Pieces.Add(sonata);

			PieceEntity etude = new PieceEntity() {
				Title = "etude",
				Grades = new List<GradeEntity> {
					CreateGrade("abrsm", "3"),
					CreateGrade("abrsm", "4"),
					CreateGrade("rcm", "6")
				}
			};
			gradesContext.Pieces.Add(etude);

			PieceEntity minuet = new PieceEntity() {
				Title = "minuet"
			};
			gradesContext.Pieces.Add(minuet);
		}

		private static void SeedRelationships(GradesContext gradesContext) {
			PieceEntity nocturne = gradesContext.Pieces.Where(p => p.Title == "nocturne").FirstOrDefault();
			PieceEntity sonata = gradesContext.Pieces.Where(p => p.Title == "sonata").FirstOrDefault();
			PieceEntity etude = gradesContext.Pieces.Where(p => p.Title == "etude").FirstOrDefault();

			ComposerEntity john = gradesContext.Composers.Where(c => c.Name == "john").FirstOrDefault();
			ComposerEntity mary = gradesContext.Composers.Where(c => c.Name == "mary").FirstOrDefault();
			ComposerEntity alice = gradesContext.Composers.Where(c => c.Name == "alice").FirstOrDefault();

			ArrangerEntity bob = gradesContext.Arrangers.Where(a => a.Name == "bob").FirstOrDefault();
			ArrangerEntity tom = gradesContext.Arrangers.Where(a => a.Name == "tom").FirstOrDefault();

			nocturne.Composers.Add(john);
			nocturne.Composers.Add(mary);

			sonata.Composers.Add(alice);
			sonata.Arrangers.Add(bob);

			etude.Composers.Add(john);
			etude.Arrangers.Add(bob);
			etude.Arrangers.Add(tom);
		}

		private static void SeedUsers(GradesContext gradesContext) {
			AppUserEntity appUser = new AppUserEntity();
			appUser.UserName = "admin";

			// Hashed password
			appUser.PasswordHash = "";
			appUser.Roles = new List<AppRoleEntity> { new AppRoleEntity() { Name = "ADMIN"} };
		}

		public async static Task Clear(GradesContext gradesContext) {
			await gradesContext.Pieces.Include(p=> p.Grades).Include(p => p.Composers).Include(p => p.Arrangers).ForEachAsync(p=> {
				p.Grades.Clear();
				p.Composers.Clear();
				p.Arrangers.Clear();
			});
			await gradesContext.SaveChangesAsync();

			await gradesContext.Pieces.ExecuteDeleteAsync();
			await gradesContext.Grades.ExecuteDeleteAsync();
			await gradesContext.Composers.ExecuteDeleteAsync();
			await gradesContext.Arrangers.ExecuteDeleteAsync();

			await gradesContext.SaveChangesAsync();
		}

		private static GradeEntity CreateGrade(string gradingSystem, string score) {
			return new GradeEntity() { GradeScore = score, GradingSystem = gradingSystem };
		}
	}
}
