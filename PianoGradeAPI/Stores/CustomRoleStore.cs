using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Contexts;
using PianoGradeAPI.Entities;

namespace PianoGradeAPI.Stores
{
    public class CustomRoleStore : IRoleStore<AppRoleEntity>
    {

        private GradesContext gradesContext;

        public CustomRoleStore(GradesContext gradesContext)
        {
            this.gradesContext = gradesContext;
        }

        public async Task<IdentityResult> CreateAsync(AppRoleEntity role, CancellationToken cancellationToken)
        {
            role.Name = role.Name.ToUpper();
            gradesContext.Roles.Add(role);
            await gradesContext.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppRoleEntity role, CancellationToken cancellationToken)
        {
            gradesContext.Roles.Remove(role);
            await gradesContext.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<AppRoleEntity?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await gradesContext.Roles.FirstOrDefaultAsync(r => r.Id == Convert.ToInt32(roleId));
        }

        public async Task<AppRoleEntity?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await gradesContext.Roles.FirstOrDefaultAsync(r => r.Name == normalizedRoleName);
        }

        public Task<string?> GetNormalizedRoleNameAsync(AppRoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name.ToUpper());
        }

        public Task<string> GetRoleIdAsync(AppRoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string?> GetRoleNameAsync(AppRoleEntity role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(AppRoleEntity role, string? normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(AppRoleEntity role, string? roleName, CancellationToken cancellationToken)
        {
            role.NormalizedName = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(AppRoleEntity role, CancellationToken cancellationToken)
        {
            gradesContext.Roles.Update(role);
            await gradesContext.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
