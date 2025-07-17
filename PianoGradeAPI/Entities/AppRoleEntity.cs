using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI.Entities
{
    [Table("app_role")]
    public class AppRoleEntity : IdentityRole<int>
    {
        [Column("id")]
        public override int Id { get; set; }
        [Column("name")]
        public override string Name { get; set; }

        [NotMapped]
        public override string? NormalizedName { get; set; }

        [NotMapped]
        public override string? ConcurrencyStamp { get; set; }
    }
}
