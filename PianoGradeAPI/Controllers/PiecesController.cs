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
				query = query.Where(p => p.Name.ToUpper().Contains(title.ToUpper()));
            }

			if(composerName != null) {
				query = query.Where(p => p.Composers.Any(c => c.Name.ToUpper().Contains(composerName.ToUpper())));
			}

			if(arrangerName != null) {
				query = query.Where(p => p.Arrangers.Any(a => a.Name.ToUpper().Contains(arrangerName.ToUpper())));
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

		[HttpGet]
		[Route("{id?}")]
		public async Task<ActionResult<GetPieceDto>> GetPieceById(int id) {
			GetPieceDto? piece = await gradesContext.Pieces.Where(p => p.Id == id).Select(p => new GetPieceDto() {
				Id = p.Id,
				Name = p.Name,
				Composers = p.Composers.Select(c => new GetPieceDtoComposer() {
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
			}).FirstOrDefaultAsync();

			if(piece == null) {
				return NotFound();
			}

			return Ok(piece);
		}

		[HttpPost]
		public async Task<ActionResult> InsertPiece([FromBody] InsertPieceDto insertPieceDto) {
			// composers and arrangers should already exist before registering a piece
			List<Composer> composers = [];
			if (insertPieceDto.ComposerIds.Count > 0) {
				composers = gradesContext.Composers
				.Where(c => insertPieceDto.ComposerIds.Contains(c.Id))
				.ToList();

				if(composers.Count != insertPieceDto.ComposerIds.Count) {
					return UnprocessableEntity("Invalid composer id");
				}
			}

			List<Arranger> arrangers = [];
			if(insertPieceDto.ArrangerIds.Count > 0) {
				arrangers = gradesContext.Arrangers
				.Where(a => insertPieceDto.ArrangerIds.Contains(a.Id))
				.ToList();

				if(arrangers.Count != insertPieceDto.ArrangerIds.Count) {
					return UnprocessableEntity("Invalid arranger id");
				}
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
			List<Composer> composers = [];
			if (updatePieceDto.ComposerIds.Count > 0) {
				composers = gradesContext.Composers
				.Where(c => updatePieceDto.ComposerIds.Contains(c.Id))
				.ToList();

				if (composers.Count != updatePieceDto.ComposerIds.Count) {
					return UnprocessableEntity("Invalid composer id");
				}
			}

			List<Arranger> arrangers = [];
			if (updatePieceDto.ArrangerIds.Count > 0) {
				arrangers = gradesContext.Arrangers
				.Where(a => updatePieceDto.ArrangerIds.Contains(a.Id))
				.ToList();

				if (arrangers.Count != updatePieceDto.ArrangerIds.Count) {
					return UnprocessableEntity("Invalid arranger id");
				}
			}

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
