using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI.Dtos {
	public class GetPieceDto {
		public int Id { get; set; }
		public string Name { get; set; }
		public List<GetPieceDtoComposer> Composers { get; set; }
		public List<GetPieceDtoArranger> Arrangers { get; set; }
		public List<GetPieceDtoGrade> Grades { get; set; }
	}
}
