using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
	}
}
