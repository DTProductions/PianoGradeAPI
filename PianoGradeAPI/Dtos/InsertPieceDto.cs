using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI.Dtos {
	public class InsertPieceDto {
		public string Title { get; set; }
		public List<int> ComposerIds { get; set; } = [];
		public List<int> ArrangerIds { get; set; } = [];
		public List<InsertPieceDtoGrade> Grades { get; set; } = [];

	}
}
