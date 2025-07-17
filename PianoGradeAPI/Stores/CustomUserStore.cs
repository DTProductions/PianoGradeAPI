using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PianoGradeAPI.Contexts;
using PianoGradeAPI.Entities;

namespace PianoGradeAPI.Stores
{
    public class CustomUserStore : IUserStore<AppUserEntity>, IUserPasswordStore<AppUserEntity>, IUserRoleStore<AppUserEntity>
    {

        private GradesContext gradesContext;
        private IRoleStore<AppRoleEntity> customRoleStore;

        public CustomUserStore(GradesContext gradesContext, IRoleStore<AppRoleEntity> customRoleStore)
        {
            this.gradesContext = gradesContext;
            this.customRoleStore = customRoleStore;
        }

        public async Task AddToRoleAsync(AppUserEntity user, string roleName, CancellationToken cancellationToken)
        {
            AppRoleEntity roleToAdd = await customRoleStore.FindByNameAsync(roleName.ToUpper(), cancellationToken);

            user.Roles.Add(roleToAdd);
            gradesContext.Users.Update(user);
            await gradesContext.SaveChangesAsync();
        }

        public async Task<IdentityResult> CreateAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            user.UserName = user.UserName.ToUpper();
            gradesContext.Users.Add(user);
            await gradesContext.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            gradesContext.Users.Remove(user);
            await gradesContext.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<AppUserEntity?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await gradesContext.Users.Include(u => u.Roles).Where(u => u.Id == Convert.ToInt32(userId)).FirstOrDefaultAsync();
        }

        public Task<AppUserEntity?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return gradesContext.Users.Include(u => u.Roles).Where(u => u.UserName == normalizedUserName).FirstOrDefaultAsync();
        }

        public Task<string?> GetNormalizedUserNameAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToUpperInvariant());
        }

        public Task<string?> GetPasswordHashAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<IList<string>> GetRolesAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<string>)user.Roles.Select(r => r.Name).ToList());
        }

        public Task<string> GetUserIdAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string?> GetUserNameAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public async Task<IList<AppUserEntity>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await gradesContext.Users
                .Where(u => u.Roles.Any(r => r.Name == roleName))
                .ToListAsync();
        }

        public Task<bool> HasPasswordAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public async Task<bool> IsInRoleAsync(AppUserEntity user, string roleName, CancellationToken cancellationToken)
        {
            return await gradesContext.Users.AnyAsync(u => u.Id == user.Id && u.Roles.Any(r => r.Name == roleName));
        }

        public async Task RemoveFromRoleAsync(AppUserEntity user, string roleName, CancellationToken cancellationToken)
        {
            roleName = roleName.ToUpper();
            AppRoleEntity? roleToRemove = user.Roles.FirstOrDefault(r => r.Name == roleName);

            if (roleToRemove == null)
            {
                return;
            }

            user.Roles.Remove(roleToRemove);
            await gradesContext.SaveChangesAsync();
        }

        public Task SetNormalizedUserNameAsync(AppUserEntity user, string? normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(AppUserEntity user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(AppUserEntity user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(AppUserEntity user, CancellationToken cancellationToken)
        {
            gradesContext.Users.Update(user);
            await gradesContext.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
