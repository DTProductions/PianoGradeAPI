using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI.Entities
{
    [Table("grade")]
    [PrimaryKey(nameof(PieceId), nameof(GradingSystem), nameof(GradeScore))]
    public class GradeEntity
    {
        [Column("piece_id")]
        public int PieceId { get; set; }
        [Column("grading_system")]
        public string GradingSystem { get; set; }
        [Column("grade")]
        public string GradeScore { get; set; }
        public PieceEntity Piece { get; set; }
    }
}
