using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Clients.Base;
using WebStore.Domain.DTOs.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
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
            var response = await PostAsync(user, UsersIdentityAddress.UserId, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, UsersIdentityAddress.UserName, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, $"{UsersIdentityAddress.UserName}/{userName}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.UserName = userName;
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, UsersIdentityAddress.NormalUserName, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<string>(cancellationToken);
        }
        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, $"{UsersIdentityAddress.NormalUserName}/{normalizedName}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.NormalizedUserName = normalizedName;
        }
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, UsersIdentityAddress.CreateUser, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var success = await response.Content.ReadAsAsync<bool>(cancellationToken);
            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var response = await PutAsync(user, UsersIdentityAddress.EditUser, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var success = await response.Content.ReadAsAsync<bool>(cancellationToken);
            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, UsersIdentityAddress.DeleteUser, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var success = await response.Content.ReadAsAsync<bool>(cancellationToken);
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

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            return PostAsync(user, $"{UsersIdentityAddress.AddToRole}/{roleName}", cancellationToken);
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            return PostAsync(user, $"{UsersIdentityAddress.RemoveFromRole}/{roleName}", cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, UsersIdentityAddress.Roles, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<IList<string>>(cancellationToken);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, $"{UsersIdentityAddress.InRole}/{roleName}", cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<bool>(cancellationToken);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return await GetAsync<List<User>>($"{UsersIdentityAddress.UsersInRole}/{roleName}", cancellationToken);
        }

        #endregion

        #region IUserPasswordStore

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            var response = await PostAsync(new PasswordHashDto()
            {
                User = user,
                Hash = passwordHash
            }, UsersIdentityAddress.SetPasswordHash, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.PasswordHash = passwordHash;
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, UsersIdentityAddress.GetPasswordHash, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.HasPassword, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<bool>(cancellationToken);
        }

        #endregion

        #region IUserClaimStore

        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetClaims, cancellationToken);
            return await result.Content.ReadAsAsync<List<Claim>>(cancellationToken);
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return PostAsync(new AddClaimsDto()
            {
                User = user,
                Claims = claims
            }, UsersIdentityAddress.AddClaims, cancellationToken);
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            return PostAsync(new ReplaceClaimsDto()
            {
                User = user,
                OldClaim = claim,
                NewClaim = newClaim
            }, UsersIdentityAddress.ReplaceClaim, cancellationToken);
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return PostAsync(new RemoveClaimsDto()
            {
                User = user,
                Claims = claims
            }, UsersIdentityAddress.RemoveClaims, cancellationToken);
        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var result = await PostAsync(claim, UsersIdentityAddress.UsersForClaim, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<List<User>>(cancellationToken);
        }

        #endregion

        #region IUserTwoFactorStore

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, $"{UsersIdentityAddress.SetTwoFactorEnabled}/{enabled}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.TwoFactorEnabled = enabled;
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetTwoFactorEnabled, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<bool>(cancellationToken);
        }

        #endregion

        #region IUserEmailStore

        public async Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, $"{UsersIdentityAddress.SetEmail}/{email}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.Email = email;
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetEmail, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetEmailConfirmed, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<bool>(cancellationToken);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, $"{UsersIdentityAddress.SetEmailConfirmed}/{confirmed}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.EmailConfirmed = confirmed;
        }

        public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return GetAsync<User>($"{UsersIdentityAddress.FindByEmail}/{normalizedEmail}", cancellationToken);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetNormalizedEmail, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, UsersIdentityAddress.SetNormalizedEmail, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.NormalizedEmail = normalizedEmail;
        }
        #endregion

        #region IUserPhoneNumberStore

        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, $"{UsersIdentityAddress.SetPhoneNumber}/{phoneNumber}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.PhoneNumber = phoneNumber;
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetPhoneNumber, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetPhoneNumberConfirmed, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<bool>(cancellationToken);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, $"{UsersIdentityAddress.SetPhoneNumberConfirmed}/{confirmed}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.PhoneNumberConfirmed = confirmed;
        }

        #endregion

        #region IUserLoginStore

        public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            return PostAsync(new AddLoginDto()
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
            var result = await PostAsync(user, UsersIdentityAddress.GetLogins, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<List<UserLoginInfo>>(cancellationToken);
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return GetAsync<User>($"{UsersIdentityAddress.FindByLogin}/{loginProvider}/{providerKey}", cancellationToken);
        }

        #endregion

        #region IUserLockoutStore

        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetLockoutEndDate, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<DateTimeOffset?>(cancellationToken);
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            var response = await PostAsync(new SetLockoutDto()
            {
                User = user,
                LockoutEnd = lockoutEnd
            }, UsersIdentityAddress.SetLockoutEndDate, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.LockoutEnd = lockoutEnd;
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.IncrementAccessFailedCount, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<int>(cancellationToken);
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            return PostAsync(user, UsersIdentityAddress.ResetAccessFailedCount, cancellationToken);
        }

        public async Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetAccessFailedCount, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<int>(cancellationToken);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            var result = await PostAsync(user, UsersIdentityAddress.GetLockoutEnabled, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<bool>(cancellationToken);
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            var response = await PostAsync(user, $"{UsersIdentityAddress.SetLockoutEnabled}/{enabled}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            user.LockoutEnabled = enabled;
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
