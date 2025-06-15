using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PianoGradeAPI {
	public class CustomRoleStore : IRoleStore<Role> {

		private GradesContext gradesContext;

		public CustomRoleStore(GradesContext gradesContext) {
			this.gradesContext = gradesContext;
		}

		public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken) {
			role.Name = role.Name.ToUpper();
			gradesContext.Roles.Add(role);
			await gradesContext.SaveChangesAsync();

			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken) {
			gradesContext.Roles.Remove(role);
			await gradesContext.SaveChangesAsync();

			return IdentityResult.Success;
		}

		public void Dispose() {
		}

		public async Task<Role?> FindByIdAsync(string roleId, CancellationToken cancellationToken) {
			return await gradesContext.Roles.FirstOrDefaultAsync(r => r.Id == Convert.ToInt32(roleId));
		}

		public async Task<Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken) {
			return await gradesContext.Roles.FirstOrDefaultAsync(r => r.Name == normalizedRoleName);
		}

		public Task<string?> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken) {
			return Task.FromResult(role.Name.ToUpper());
		}

		public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken) {
			return Task.FromResult(role.Id.ToString());
		}

		public Task<string?> GetRoleNameAsync(Role role, CancellationToken cancellationToken) {
			return Task.FromResult(role.Name);
		}

		public Task SetNormalizedRoleNameAsync(Role role, string? normalizedName, CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}

		public Task SetRoleNameAsync(Role role, string? roleName, CancellationToken cancellationToken) {
			role.NormalizedName = roleName;
			return Task.CompletedTask;
		}

		public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken) {
			gradesContext.Roles.Update(role);
			await gradesContext.SaveChangesAsync();
			return IdentityResult.Success;
		}
	}
}
