
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI {
	[Table("arranger")]
	public class Arranger {
		[Key]
		[Column("id")]
		public int Id { get; set; }
		[Column("name")]
		public string Name { get; set; }
		public List<Piece> Pieces { get; set; }
	}
}
