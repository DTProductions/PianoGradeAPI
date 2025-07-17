
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI.Entities
{
    [Table("arranger")]
    public class ArrangerEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        public List<PieceEntity> Pieces { get; set; } = [];
    }
}
