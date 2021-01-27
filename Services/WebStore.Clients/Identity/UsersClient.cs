using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Clients.Base;
using WebStore.Domain.DTOs.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services.Identity;
using UsersIdentityAddress = WebStore.Interfaces.ServiceAddress.Identity.Users;

namespace WebStore.Clients.Identity
{
    public class UsersClient : BaseClient, ICustomUserIdentity
    {
        public UsersClient(HttpClient httpClient)
            : base(httpClient, UsersIdentityAddress.Name)
        {
        }

        #region IUserStore

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, string>(user, UsersIdentityAddress.UserId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, string>(user, UsersIdentityAddress.UserName, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = await PostAndReadAsync<User, string>(user, $"{UsersIdentityAddress.UserName}/{userName}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, string>(user, UsersIdentityAddress.NormalUserName, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = await PostAndReadAsync<User, string>(user, $"{UsersIdentityAddress.NormalUserName}/{normalizedName}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var success = await PostAndReadAsync<User, bool>(user, UsersIdentityAddress.User, cancellationToken).ConfigureAwait(false);
            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var response = await PutAsync(user, UsersIdentityAddress.User, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var success = await response.Content.ReadAsAsync<bool>(cancellationToken);
            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            var success = await PostAndReadAsync<User, bool>(user, UsersIdentityAddress.DeleteUser, cancellationToken).ConfigureAwait(false);
            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await GetAsync<User>($"{UsersIdentityAddress.FindById}/{userId}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await GetAsync<User>($"{UsersIdentityAddress.FindByName}/{normalizedUserName}", cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region IUserRoleStore

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, IList<string>>(user, UsersIdentityAddress.Roles, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, bool>(user, $"{UsersIdentityAddress.InRole}/{roleName}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await GetAsync<List<User>>($"{UsersIdentityAddress.UsersInRole}/{roleName}", cancellationToken);
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            await PostAndEnsureAsync(user, $"{UsersIdentityAddress.AddToRole}/{roleName}", cancellationToken);
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            await PostAndEnsureAsync(user, $"{UsersIdentityAddress.RemoveFromRole}/{roleName}", cancellationToken);
        }

        #endregion

        #region IUserPasswordStore

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, bool>(user, UsersIdentityAddress.HasPassword, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, string>(user, UsersIdentityAddress.GetPasswordHash, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = await PostAndReadAsync<PasswordHashDto, string>(new PasswordHashDto()
            {
                User = user,
                Hash = passwordHash
            }, UsersIdentityAddress.SetPasswordHash, cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region IUserClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, IList<Claim>>(user, UsersIdentityAddress.GetClaims, cancellationToken);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<Claim, List<User>>(claim, UsersIdentityAddress.UsersForClaim, cancellationToken).ConfigureAwait(false);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            await PostAndEnsureAsync(new AddClaimsDto()
            {
                User = user,
                Claims = claims
            }, UsersIdentityAddress.AddClaims, cancellationToken);
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            await PostAndEnsureAsync(new ReplaceClaimsDto()
            {
                User = user,
                OldClaim = claim,
                NewClaim = newClaim
            }, UsersIdentityAddress.ReplaceClaim, cancellationToken);
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            await PostAndEnsureAsync(new RemoveClaimsDto()
            {
                User = user,
                Claims = claims
            }, UsersIdentityAddress.RemoveClaims, cancellationToken);
        }

        #endregion

        #region IUserTwoFactorStore

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, bool>(user, UsersIdentityAddress.GetTwoFactorEnabled, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = await PostAndReadAsync<User, bool>(user, $"{UsersIdentityAddress.SetTwoFactorEnabled}/{enabled}", cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region IUserEmailStore

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, string>(user, UsersIdentityAddress.GetEmail, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Email = await PostAndReadAsync<User, string>(user, $"{UsersIdentityAddress.SetEmail}/{email}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, bool>(user, UsersIdentityAddress.GetEmailConfirmed, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = await PostAndReadAsync<User, bool>(user, $"{UsersIdentityAddress.SetEmailConfirmed}/{confirmed}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await GetAsync<User>($"{UsersIdentityAddress.FindByEmail}/{normalizedEmail}", cancellationToken);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, string>(user, UsersIdentityAddress.GetNormalizedEmail, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = await PostAndReadAsync<User, string>(user, $"{UsersIdentityAddress.SetNormalizedEmail}/{normalizedEmail}", cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region IUserPhoneNumberStore

        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = await PostAndReadAsync<User, string>(user, $"{UsersIdentityAddress.SetPhoneNumber}/{phoneNumber}", cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, string>(user, UsersIdentityAddress.GetPhoneNumber, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, bool>(user, UsersIdentityAddress.GetPhoneNumberConfirmed, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = await PostAndReadAsync<User, bool>(user, $"{UsersIdentityAddress.SetPhoneNumberConfirmed}/{confirmed}", cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region IUserLoginStore

        public async Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            await PostAndEnsureAsync(new AddLoginDto()
            {
                User = user,
                UserLoginInfo = login
            }, UsersIdentityAddress.AddLogin, cancellationToken);
        }

        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return PostAsync(user, $"{UsersIdentityAddress.RemoveLogin}/{loginProvider}/{providerKey}", cancellationToken);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, List<UserLoginInfo>>(user, UsersIdentityAddress.GetLogins, cancellationToken).ConfigureAwait(false);
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return GetAsync<User>($"{UsersIdentityAddress.FindByLogin}/{loginProvider}/{providerKey}", cancellationToken);
        }

        #endregion

        #region IUserLockoutStore

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, DateTimeOffset?>(user, UsersIdentityAddress.GetLockoutEndDate, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = await PostAndReadAsync<SetLockoutDto, DateTimeOffset?>(new SetLockoutDto()
            {
                User = user,
                LockoutEnd = lockoutEnd
            }, UsersIdentityAddress.SetLockoutEndDate, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, int>(user, UsersIdentityAddress.IncrementAccessFailedCount, cancellationToken).ConfigureAwait(false);
        }

        public async Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            await PostAndEnsureAsync(user, UsersIdentityAddress.ResetAccessFailedCount, cancellationToken);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, int>(user, UsersIdentityAddress.GetAccessFailedCount, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            return await PostAndReadAsync<User, bool>(user, UsersIdentityAddress.GetLockoutEnabled, cancellationToken).ConfigureAwait(false);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = await PostAndReadAsync<User, bool>(user, $"{UsersIdentityAddress.SetLockoutEnabled}/{enabled}", cancellationToken).ConfigureAwait(false);
        }

        #endregion

        #region IDisposable

        void IDisposable.Dispose() 
        {
            //Do nothing
        }

        #endregion
    }
}
