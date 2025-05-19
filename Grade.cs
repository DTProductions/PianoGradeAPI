using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI {
	[Table("grade")]
	[PrimaryKey(nameof(PieceId), nameof(Grade.GradingSystem), nameof(Grade.GradeScore))]
	public class Grade {
		[Column("piece_id")]
		public int PieceId { get; set; }
		[Column("grading_system")]
		public string GradingSystem { get; set; }
		[Column("grade")]
		public string GradeScore {  get; set; }
		public Piece Piece { get; set; }
	}
}
