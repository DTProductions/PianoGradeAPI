using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PianoGradeAPI {
	[Table("composer")]
	public class Composer {
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("era")]
		public string Era { get; set; }

		[Column("nationality")]
		public string Nationality { get; set; }
		public List<Piece> Pieces { get; set; }
	}
}
