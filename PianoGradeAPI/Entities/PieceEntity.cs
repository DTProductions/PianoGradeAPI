using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI.Entities
{
    [Table("piece")]
    public class PieceEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        public List<ComposerEntity> Composers { get; set; } = [];
        public List<ArrangerEntity> Arrangers { get; set; } = [];
        public List<GradeEntity> Grades { get; set; } = [];
    }
}
