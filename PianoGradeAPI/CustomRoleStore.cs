using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PianoGradeAPI {
	public class CustomRoleStore : IRoleStore<AppRole> {

		private GradesContext gradesContext;

		public CustomRoleStore(GradesContext gradesContext) {
			this.gradesContext = gradesContext;
		}

		public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken) {
			role.Name = role.Name.ToUpper();
			gradesContext.Roles.Add(role);
			await gradesContext.SaveChangesAsync();

			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken) {
			gradesContext.Roles.Remove(role);
			await gradesContext.SaveChangesAsync();

			return IdentityResult.Success;
		}

		public void Dispose() {
		}

		public async Task<AppRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken) {
			return await gradesContext.Roles.FirstOrDefaultAsync(r => r.Id == Convert.ToInt32(roleId));
		}

		public async Task<AppRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) {
			return await gradesContext.Roles.FirstOrDefaultAsync(r => r.Name == normalizedRoleName);
		}

		public Task<string?> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken) {
			return Task.FromResult(role.Name.ToUpper());
		}

		public Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken) {
			return Task.FromResult(role.Id.ToString());
		}

		public Task<string?> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken) {
			return Task.FromResult(role.Name);
		}

		public Task SetNormalizedRoleNameAsync(AppRole role, string? normalizedName, CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}

		public Task SetRoleNameAsync(AppRole role, string? roleName, CancellationToken cancellationToken) {
			role.NormalizedName = roleName;
			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken) {
			gradesContext.Roles.Update(role);
			await gradesContext.SaveChangesAsync();
			return IdentityResult.Success;
		}
	}
}
