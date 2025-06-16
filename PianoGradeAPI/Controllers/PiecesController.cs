using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Dtos;
using System.Collections.Generic;

namespace PianoGradeAPI.Controllers {
	[Route("[controller]")]
	[ApiController]
	public class PiecesController : ControllerBase {
		private GradesContext gradesContext;

		public PiecesController(GradesContext gradesContext) {
			this.gradesContext = gradesContext;
		}

		[HttpGet]
		public List<GetPieceDto> GetPieces([FromQuery] string? title, [FromQuery] string? composerName, [FromQuery] string? arrangerName, [FromQuery] string? gradingSystem, string? grade, [FromQuery] int? sinceId, [FromQuery] int? limit) {
			IQueryable<Piece> query = gradesContext.Pieces;
			if (sinceId != null) {
				query = query.OrderBy(p => p.Id).Where(p => p.Id > sinceId);
			}

			if (limit != null) {
				query = query.Take((int)limit);
			}

			if (title != null)
            {
				query = query.Where(p => p.Name.Contains(title));
            }

			if(composerName != null) {
				query = query.Where(p => p.Composers.Any(c => c.Name.Contains(composerName)));
			}

			if(arrangerName != null) {
				query = query.Where(p => p.Arrangers.Any(a => a.Name.Contains(arrangerName)));
			}

			if(gradingSystem != null) {
				query = query.Where(p => p.Grades.Any(g => g.GradingSystem == gradingSystem));
			}

			if(grade != null) {
				query = query.Where(p => p.Grades.Any(g => g.GradeScore == grade));
			}

			List<GetPieceDto> pieces = query.Select(p => new GetPieceDto() {
				Id = p.Id,
				Name = p.Name,
				Composers = p.Composers.Select(c => new GetPieceDtoComposer(){
					Id = c.Id,
					Name = c.Name
				}).ToList(),
				Arrangers = p.Arrangers.Select(p => new GetPieceDtoArranger() {
					Id = p.Id,
					Name = p.Name,
				}).ToList(),
				Grades = p.Grades.Select(g => new GetPieceDtoGrade() {
					GradingSystem = g.GradingSystem,
					Grade = g.GradeScore
				}).ToList()
			}).ToList();

			return pieces;
		}

		[HttpPost]
		public async Task<ActionResult> InsertPiece([FromBody] InsertPieceDto insertPieceDto) {

			// composer and arrangers should already exist before registering a piece
			List<Composer> composers = gradesContext.Composers
				.Where(c => insertPieceDto.ComposerIds.Contains(c.Id))
				.ToList();

			List<Arranger> arrangers = gradesContext.Arrangers
				.Where(a => insertPieceDto.ArrangerIds.Contains(a.Id))
				.ToList();

			if (composers.Count == 0 || arrangers.Count == 0) {
				return UnprocessableEntity();
			}

			List<Grade> grades = insertPieceDto.Grades.Select(g => {
					Grade gradeToAdd = new Grade() { GradingSystem = g.GradingSystem, GradeScore = g.Grade};
					return gradeToAdd;
				}).ToList();

			Piece pieceToAdd = new Piece() {
				Name = insertPieceDto.Title,
				Composers = composers,
				Arrangers = arrangers,
				Grades = grades
			};

			gradesContext.Pieces.Add(pieceToAdd);
			await gradesContext.SaveChangesAsync();
			return Created();
		}

		[HttpPut]
		public async Task<ActionResult> UpdatePiece([FromBody] UpdatePieceDto updatePieceDto) {
			Piece? pieceToUpdate = await gradesContext.Pieces.Include(p => p.Composers).Include(p => p.Arrangers).Include(p => p.Grades).FirstOrDefaultAsync(a => a.Id == updatePieceDto.Id);
			if (pieceToUpdate == null) {
				return UnprocessableEntity();
			}

			List<Grade> grades = updatePieceDto.Grades.Select(g => {
				Grade gradeToAdd = new Grade() { GradingSystem = g.GradingSystem, GradeScore = g.Grade};
				return gradeToAdd;
			}).ToList();

			// composer and arrangers should already exist before registering a piece
			List<Composer> composers = gradesContext.Composers
				.Where(c => updatePieceDto.ComposerIds.Contains(c.Id))
				.ToList();

			List<Arranger> arrangers = gradesContext.Arrangers
				.Where(a => updatePieceDto.ArrangerIds.Contains(a.Id))
				.ToList();

			pieceToUpdate.Name = updatePieceDto.Title;
			pieceToUpdate.Grades = grades;
			pieceToUpdate.Composers = composers;
			pieceToUpdate.Arrangers = arrangers;

			gradesContext.Pieces.Update(pieceToUpdate);
			await gradesContext.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete]
		public async Task<ActionResult> DeletePiece([FromQuery] int id) {
			Piece? pieceToDelete = await gradesContext.Pieces.Include(p => p.Composers).Include(p => p.Arrangers).Include(p => p.Grades).FirstOrDefaultAsync(p => p.Id == id);
			if (pieceToDelete == null) {
				return UnprocessableEntity();
			}

			gradesContext.Pieces.Remove(pieceToDelete);
			await gradesContext.SaveChangesAsync();

			return NoContent();
		}
	}
}
