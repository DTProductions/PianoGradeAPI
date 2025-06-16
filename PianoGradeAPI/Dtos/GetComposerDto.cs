using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PianoGradeAPI.Dtos {
	public class GetComposerDto {
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Era { get; set; }
		public string? Nationality { get; set; }
	}
}
