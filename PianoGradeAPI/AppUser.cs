using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PianoGradeAPI {
	[Table("app_user")]
	public class AppUser : IdentityUser<int>{
		[Key]
		[Column("id")]
		public override int Id { get; set; }
		[Column("username")]
		public override string UserName { get; set; }
		[Column("password")]
		public override string PasswordHash { get; set; }
		public List<AppRole> Roles { get; set; } = [];

		[NotMapped]
		public override string? NormalizedUserName { get; set; }

		[NotMapped]
		public override string? Email { get; set; }

		[NotMapped]
		public override string? NormalizedEmail { get; set; }

		[NotMapped]
		public override bool EmailConfirmed { get; set; }

		[NotMapped]
		public override string? SecurityStamp { get; set; }

		[NotMapped]
		public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

		[NotMapped]
		public override string? PhoneNumber { get; set; }

		[NotMapped]
		public override bool PhoneNumberConfirmed { get; set; }

		[NotMapped]
		public override bool TwoFactorEnabled { get; set; }

		[NotMapped]
		public override DateTimeOffset? LockoutEnd { get; set; }

		[NotMapped]
		public override bool LockoutEnabled { get; set; }

		[NotMapped]
		public override int AccessFailedCount { get; set; }
	}
}
