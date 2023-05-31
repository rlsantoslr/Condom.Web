using Condom.Domain.Models;
using Condom.Domain.Models.Identity;
using Condom.Infra.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Repositories.Identity
{
    public interface IIdentityUserStore<TEntity> : IRepository<TEntity>, IUserLoginStore<TEntity>, IUserPasswordStore<TEntity>, IUserEmailStore<TEntity>, IUserRoleStore<TEntity>, IUserStore<TEntity> where TEntity : Users
    {
        Task<Users> FindByEmail(string email, CancellationToken cancellationToken = default(CancellationToken));
        Task<UserProfiles> GetProfileByUserId(Guid userId);
    }

    public class IdentityUserStore : Repository<Users>, IIdentityUserStore<Users>
    {
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        public IdentityUserStore(CondomContext context) : base(context)
        {
        }

        public async Task<UserProfiles> GetProfileByUserId(Guid userId)
        {
            var rt = new UserProfiles();

            return await Context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Users> FindByEmail(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(email)) return null;

            email = email.ToUpperInvariant().Trim();

            var user = await OwnDbSet.FirstOrDefaultAsync(x => x.NormalizedEmail == email);
            return user;
        }

        public async Task<IdentityResult> CreateAsync(Users user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            Context.Add(user);
            await SaveChanges(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(Users user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            Context.Attach(user);
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            Context.Update(user);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public async Task<Users> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = ConvertIdFromString(userId);
            return await OwnDbSet.FindAsync(new object[] { id }, cancellationToken).AsTask();
        }
        public async Task<Users> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await OwnDbSet.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
        }

        public  Guid ConvertIdFromString(string id)
        {
            if (id == null)
            {
                return default(Guid);
            }
            return (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(id);
        }

        public async Task<IdentityResult> DeleteAsync(Users user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            Context.Remove(user);
            try
            {
                await SaveChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        public async Task<Guid> GetUserIdAsync(Users user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return user.Id;
        }

        public async Task<string> GetUserNameAsync(Users user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return user.UserName;
        }

        public async Task SetUserNameAsync(Users user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.UserName = userName;
            return;
        }

        public async Task<string> GetNormalizedUserNameAsync(Users user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return user.NormalizedUserName;
        }

        public async Task SetNormalizedUserNameAsync(Users user, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            user.NormalizedUserName = normalizedName;
            return;
        }

        async Task<string> IUserStore<Users>.GetUserIdAsync(Users user, CancellationToken cancellationToken)
        {
            return user?.Id.ToString() ?? "";
        }

        public Task SetSecurityStampAsync(Users user, string stamp, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(Users user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetEmailAsync(Users user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
        }

        public async Task<string> GetEmailAsync(Users user, CancellationToken cancellationToken)
        {
            return user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(Users user, CancellationToken cancellationToken)
        {
            return user.EmailConfirmed;
        }

        public async Task SetEmailConfirmedAsync(Users user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
        }

        public Task<Users> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetNormalizedEmailAsync(Users user, CancellationToken cancellationToken)
        {
            return user.Email.ToUpperInvariant().Trim();
        }

        public async Task SetNormalizedEmailAsync(Users user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
        }

        public Task AddLoginAsync(Users user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(Users user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(Users user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Users> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetPasswordHashAsync(Users user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
        }

        public async Task<string> GetPasswordHashAsync(Users user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(Users user, CancellationToken cancellationToken)
        {
            return !string.IsNullOrEmpty(user.PasswordHash);
        }

        public Task AddToRoleAsync(Users user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(Users user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(Users user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(Users user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Users>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
