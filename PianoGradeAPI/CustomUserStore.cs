using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PianoGradeAPI {
	public class CustomUserStore : IUserStore<AppUser>, IUserPasswordStore<AppUser>, IUserRoleStore<AppUser> {
		
		private GradesContext gradesContext;
		private IRoleStore<Role> customRoleStore;

		public CustomUserStore(GradesContext gradesContext, IRoleStore<Role> customRoleStore) {
			this.gradesContext = gradesContext;
			this.customRoleStore = customRoleStore;
		}

		public async Task AddToRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken) {
			Role roleToAdd = await customRoleStore.FindByNameAsync(roleName.ToUpper(), cancellationToken);

			user.Roles.Add(roleToAdd);
			gradesContext.Users.Update(user);
			await gradesContext.SaveChangesAsync();
		}

		public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken) {
			user.UserName = user.UserName.ToUpper();
			gradesContext.Users.Add(user);
			await gradesContext.SaveChangesAsync();

			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken) {
			gradesContext.Users.Remove(user);
			await gradesContext.SaveChangesAsync();

			return IdentityResult.Success;
		}

		public void Dispose() {
		}

		public async Task<AppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken) {
			return await gradesContext.Users.Include(u=> u.Roles).Where(u => u.Id == Convert.ToInt32(userId)).FirstOrDefaultAsync();
		}

		public Task<AppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) {
			return gradesContext.Users.Include(u => u.Roles).Where(u => u.UserName == normalizedUserName).FirstOrDefaultAsync();
		}

		public Task<string?> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken) {
			return Task.FromResult(user.UserName.ToUpperInvariant());
		}

		public Task<string?> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken) {
			return Task.FromResult(user.PasswordHash);
		}

		public Task<IList<string>> GetRolesAsync(AppUser user, CancellationToken cancellationToken) {
			return Task.FromResult((IList<string>)user.Roles.Select(r => r.Name).ToList());
		}

		public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken) {
			return Task.FromResult(user.Id.ToString());
		}

		public Task<string?> GetUserNameAsync(AppUser user, CancellationToken cancellationToken) {
			return Task.FromResult(user.UserName);
		}

		public async Task<IList<AppUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken) {
			return await gradesContext.Users
				.Where(u => u.Roles.Any(r => r.Name == roleName))
				.ToListAsync();
		}

		public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken) {
			return Task.FromResult(!String.IsNullOrEmpty(user.PasswordHash));
		}

		public async Task<bool> IsInRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken) {
			return await gradesContext.Users.AnyAsync(u => u.Id == user.Id && u.Roles.Any(r => r.Name == roleName));
		}

		public async Task RemoveFromRoleAsync(AppUser user, string roleName, CancellationToken cancellationToken) {
			roleName = roleName.ToUpper();
			Role? roleToRemove = user.Roles.FirstOrDefault(r => r.Name == roleName);

			if (roleToRemove == null) {
				return; 
			}

			user.Roles.Remove(roleToRemove);
			await gradesContext.SaveChangesAsync();
		}

		public Task SetNormalizedUserNameAsync(AppUser user, string? normalizedName, CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}

		public Task SetPasswordHashAsync(AppUser user, string? passwordHash, CancellationToken cancellationToken) {
			user.PasswordHash = passwordHash;
			return Task.CompletedTask;
		}

		public Task SetUserNameAsync(AppUser user, string? userName, CancellationToken cancellationToken) {
			user.UserName = userName;
			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken) {
			gradesContext.Users.Update(user);
			await gradesContext.SaveChangesAsync();
			return IdentityResult.Success;
		}
	}
}
