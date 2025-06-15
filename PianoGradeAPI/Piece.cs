using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI {
	[Table("piece")]
	public class Piece {
		[Column("id")]
		public int Id { get; set; }
		[Column("name")]
		public string Name { get; set; }
		public List<Composer> Composers { get; set; } = [];
		public List<Arranger> Arrangers { get; set; } = [];
		public List<Grade> Grades { get; set; } = [];
	}
}
