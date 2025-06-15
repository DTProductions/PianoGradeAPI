using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI.Dtos {
	public class GetPieceDto {
		public string Name { get; set; }
		public List<string> Composers { get; set; }
		public List<string> Arrangers { get; set; }
		public List<GetPieceDtoGrade> Grades { get; set; }
	}
}
