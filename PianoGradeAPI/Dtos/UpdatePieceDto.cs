
namespace PianoGradeAPI.Dtos {
	public class UpdatePieceDto {
		public int Id { get; set; }
		public string Title { get; set; }
		public List<int> ComposerIds { get; set; } = [];
		public List<int> ArrangerIds { get; set; } = [];
		public List<UpdatePieceDtoGrade> Grades { get; set; } = [];
	}
}
