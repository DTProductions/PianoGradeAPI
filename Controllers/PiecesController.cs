using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Dtos;

namespace PianoGradeAPI.Controllers {
	[Route("[controller]")]
	[ApiController]
	public class PiecesController : ControllerBase {
		private GradesContext context;

		public PiecesController(GradesContext context) {
			this.context = context;
		}

		[HttpGet]
		public List<GetPieceDto> GetPieces([FromQuery] string? title, [FromQuery] string? composerName, [FromQuery] string? arrangerName, [FromQuery] string? gradingSystem, string? grade) {
			IQueryable<Piece> query = context.Pieces;
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
				Name = p.Name,
				Composers = p.Composers.Select(c => c.Name).ToList(),
				Arrangers = p.Arrangers.Select(p => p.Name).ToList(),
				Grades = p.Grades.Select(g => new GetPieceDtoGrade() {
					GradingSystem = g.GradingSystem,
					Grade = g.GradeScore
				}).ToList()
			}).ToList();

			return pieces;
		}
	}
}
