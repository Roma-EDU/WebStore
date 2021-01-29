using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebStore.DAL.Context;
using WebStore.Domain.DTOs.Identity;
using WebStore.Domain.Entities.Identity;
using UsersIdentityAddress = WebStore.Interfaces.ServiceAddress.Identity.Users;

namespace WebStore.ServiceHosting.Controllers.Identity
{
    [Route(UsersIdentityAddress.Name)]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly UserStore<User, Role, WebStoreDB> _userStore;

        public UsersApiController(WebStoreDB db)
        {
            _userStore = new UserStore<User, Role, WebStoreDB>(db);
        }

        #region IUserStore

        [HttpPost(UsersIdentityAddress.UserId)]
        public async Task<string> GetUserIdAsync([FromBody] User user)
        {
            return await _userStore.GetUserIdAsync(user);
        }

        [HttpPost(UsersIdentityAddress.UserName)]
        public async Task<string> GetUserNameAsync([FromBody] User user)
        {
            return await _userStore.GetUserNameAsync(user);
        }

        [HttpPost(UsersIdentityAddress.UserName + "/{userName}")]
        public async Task<string> SetUserNameAsync([FromBody] User user, string userName)
        {
            await _userStore.SetUserNameAsync(user, userName);
            await _userStore.UpdateAsync(user);
            return user.UserName;
        }

        [HttpPost(UsersIdentityAddress.NormalUserName)]
        public async Task<string> GetNormalizedUserNameAsync([FromBody]User user)
        {
            return await _userStore.GetNormalizedUserNameAsync(user);
        }

        [HttpPost(UsersIdentityAddress.NormalUserName + "/{normalizedName}")]
        public async Task<string> SetNormalizedUserNameAsync([FromBody] User user, string normalizedName)
        {
            await _userStore.SetNormalizedUserNameAsync(user, normalizedName);
            await _userStore.UpdateAsync(user);
            return user.NormalizedUserName;
        }

        [HttpPost(UsersIdentityAddress.User)]
        public async Task<bool> CreateAsync([FromBody] User user)
        {
            var result = await _userStore.CreateAsync(user);
            return result.Succeeded;
        }

        [HttpPut(UsersIdentityAddress.User)]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var result = await _userStore.UpdateAsync(user);
            return result.Succeeded;
        }

        [HttpPost(UsersIdentityAddress.DeleteUser)]
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
            var result = await _userStore.DeleteAsync(user);
            return result.Succeeded;
        }

        [HttpGet(UsersIdentityAddress.FindById + "/{userId}")]
        public async Task<User> FindByIdAsync(string userId)
        {
            var result = await _userStore.FindByIdAsync(userId);
            return result;
        }

        [HttpGet(UsersIdentityAddress.FindByName + "/{normalizedUserName}")]
        public async Task<User> FindByNameAsync(string normalizedUserName)
        {
            var result = await _userStore.FindByNameAsync(normalizedUserName);
            return result;
        }

        #endregion

        #region IUserRoleStore

        [HttpPost(UsersIdentityAddress.AddToRole + "/{roleName}")]
        public async Task AddToRoleAsync([FromBody] User user, string roleName, [FromServices] WebStoreDB db)
        {
            await _userStore.AddToRoleAsync(user, roleName);
            await db.SaveChangesAsync();
        }

        [HttpPost(UsersIdentityAddress.RemoveFromRole + "/{roleName}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string roleName, [FromServices] WebStoreDB db)
        {
            await _userStore.RemoveFromRoleAsync(user, roleName);
            await db.SaveChangesAsync();
        }

        [HttpPost(UsersIdentityAddress.Roles)]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user)
        {
            return await _userStore.GetRolesAsync(user);
        }

        [HttpPost(UsersIdentityAddress.InRole + "/{roleName}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string roleName)
        {
            return await _userStore.IsInRoleAsync(user, roleName);
        }

        [HttpGet(UsersIdentityAddress.UsersInRole + "/{roleName}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string roleName)
        {
            return await _userStore.GetUsersInRoleAsync(roleName);
        }

        #endregion

        #region IUserPasswordStore

        [HttpPost(UsersIdentityAddress.SetPasswordHash)]
        public async Task<string> SetPasswordHashAsync([FromBody]PasswordHashDto hashDto)
        {
            var user = hashDto.User;
            await _userStore.SetPasswordHashAsync(user, hashDto.Hash);
            await _userStore.UpdateAsync(user);
            return user.PasswordHash;
        }

        [HttpPost(UsersIdentityAddress.GetPasswordHash)]
        public async Task<string> GetPasswordHashAsync([FromBody] User user)
        {
            var result = await _userStore.GetPasswordHashAsync(user);
            return result;
        }

        [HttpPost(UsersIdentityAddress.HasPassword)]
        public async Task<bool> HasPasswordAsync([FromBody] User user)
        {
            return await _userStore.HasPasswordAsync(user);
        }

        #endregion

        #region IUserClaimStore

        [HttpPost(UsersIdentityAddress.GetClaims)]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user)
        {
            return await _userStore.GetClaimsAsync(user);
        }

        [HttpPost(UsersIdentityAddress.AddClaims)]
        public async Task AddClaimsAsync([FromBody] AddClaimsDto claimsDto, [FromServices] WebStoreDB db)
        {
            await _userStore.AddClaimsAsync(claimsDto.User, claimsDto.Claims);
            await db.SaveChangesAsync();
        }

        [HttpPost(UsersIdentityAddress.ReplaceClaim)]
        public async Task ReplaceClaimAsync([FromBody]ReplaceClaimsDto claimsDto, [FromServices] WebStoreDB db)
        {
            await _userStore.ReplaceClaimAsync(claimsDto.User, claimsDto.OldClaim, claimsDto.NewClaim);
            await db.SaveChangesAsync();
        }

        [HttpPost(UsersIdentityAddress.RemoveClaims)]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimsDto claimsDto, [FromServices] WebStoreDB db)
        {
            await _userStore.RemoveClaimsAsync(claimsDto.User, claimsDto.Claims);
            await db.SaveChangesAsync();
        }

        [HttpPost(UsersIdentityAddress.UsersForClaim)]
        public async Task<IList<User>> GetUsersForClaimAsync([FromBody]Claim claim)
        {
            return await _userStore.GetUsersForClaimAsync(claim);
        }

        #endregion

        #region IUserTwoFactorStore

        [HttpPost(UsersIdentityAddress.SetTwoFactorEnabled + "/{enabled}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enabled)
        {
            await _userStore.SetTwoFactorEnabledAsync(user, enabled);
            await _userStore.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        [HttpPost(UsersIdentityAddress.GetTwoFactorEnabled)]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user)
        {
            return await _userStore.GetTwoFactorEnabledAsync(user);
        }

        #endregion

        #region IUserEmailStore

        [HttpPost(UsersIdentityAddress.SetEmail + "/{email}")]
        public async Task<string> SetEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
            return user.Email;
        }

        [HttpPost(UsersIdentityAddress.GetEmail)]
        public async Task<string> GetEmailAsync([FromBody] User user)
        {
            return await _userStore.GetEmailAsync(user);
        }

        [HttpPost(UsersIdentityAddress.GetEmailConfirmed)]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetEmailConfirmedAsync(user);
        }

        [HttpPost(UsersIdentityAddress.SetEmailConfirmed + "/{confirmed}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _userStore.SetEmailConfirmedAsync(user, confirmed);
            await _userStore.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        [HttpGet(UsersIdentityAddress.FindByEmail + "/{normalizedEmail}")]
        public async Task<User> FindByEmailAsync(string normalizedEmail)
        {
            return await _userStore.FindByEmailAsync(normalizedEmail);
        }

        [HttpPost(UsersIdentityAddress.GetNormalizedEmail)]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user)
        {
            return await _userStore.GetNormalizedEmailAsync(user);
        }

        [HttpPost(UsersIdentityAddress.SetNormalizedEmail + "/{normalizedEmail?}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string normalizedEmail)
        {
            await _userStore.SetNormalizedEmailAsync(user, normalizedEmail);
            await _userStore.UpdateAsync(user);
            return user.NormalizedEmail;
        }

        #endregion

        #region IUserPhoneNumberStore

        [HttpPost(UsersIdentityAddress.SetPhoneNumber + "/{phoneNumber}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phoneNumber)
        {
            await _userStore.SetPhoneNumberAsync(user, phoneNumber);
            await _userStore.UpdateAsync(user);
            return user.PhoneNumber;
        }

        [HttpPost(UsersIdentityAddress.GetPhoneNumber)]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberAsync(user);
        }

        [HttpPost(UsersIdentityAddress.GetPhoneNumberConfirmed)]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody]User user)
        {
            return await _userStore.GetPhoneNumberConfirmedAsync(user);
        }

        [HttpPost(UsersIdentityAddress.SetPhoneNumberConfirmed + "/{confirmed}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _userStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _userStore.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }

        #endregion

        #region IUserLoginStore

        [HttpPost(UsersIdentityAddress.AddLogin)]
        public async Task AddLoginAsync([FromBody] AddLoginDto loginDto, [FromServices] WebStoreDB db)
        {
            await _userStore.AddLoginAsync(loginDto.User, loginDto.UserLoginInfo);
            await db.SaveChangesAsync();
        }

        [HttpPost(UsersIdentityAddress.RemoveLogin + "/{loginProvider}/{providerKey}")]
        public async Task RemoveLoginAsync([FromBody] User user, string loginProvider, string providerKey, [FromServices] WebStoreDB db)
        {
            await _userStore.RemoveLoginAsync(user, loginProvider, providerKey);
            await db.SaveChangesAsync();
        }

        [HttpPost(UsersIdentityAddress.GetLogins)]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user)
        {
            return await _userStore.GetLoginsAsync(user);
        }

        [HttpGet(UsersIdentityAddress.FindByLogin + "/{loginProvider}/{providerKey}")]
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return await _userStore.FindByLoginAsync(loginProvider, providerKey);
        }

        #endregion

        #region IUserLockoutStore

        [HttpPost(UsersIdentityAddress.GetLockoutEndDate)]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] User user)
        {
            return await _userStore.GetLockoutEndDateAsync(user);
        }

        [HttpPost(UsersIdentityAddress.SetLockoutEndDate)]
        public async Task<DateTimeOffset?> SetLockoutEndDateAsync([FromBody] SetLockoutDto setLockoutDto)
        {
            var user = setLockoutDto.User;
            await _userStore.SetLockoutEndDateAsync(user, setLockoutDto.LockoutEnd);
            await _userStore.UpdateAsync(user);
            return user.LockoutEnd;
        }

        [HttpPost(UsersIdentityAddress.IncrementAccessFailedCount)]
        public async Task<int> IncrementAccessFailedCountAsync([FromBody] User user)
        {
            await _userStore.IncrementAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            return user.AccessFailedCount;
        }

        [HttpPost(UsersIdentityAddress.ResetAccessFailedCount)]
        public async Task<int> ResetAccessFailedCountAsync([FromBody] User user)
        {
            await _userStore.ResetAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            return user.AccessFailedCount;
        }

        [HttpPost(UsersIdentityAddress.GetAccessFailedCount)]
        public async Task<int> GetAccessFailedCountAsync([FromBody] User user)
        {
            return await _userStore.GetAccessFailedCountAsync(user);
        }

        [HttpPost(UsersIdentityAddress.GetLockoutEnabled)]
        public async Task<bool> GetLockoutEnabledAsync([FromBody] User user)
        {
            return await _userStore.GetLockoutEnabledAsync(user);
        }

        [HttpPost(UsersIdentityAddress.SetLockoutEnabled + "/{enabled}")]
        public async Task<bool> SetLockoutEnabledAsync([FromBody] User user, bool enabled)
        {
            await _userStore.SetLockoutEnabledAsync(user, enabled);
            await _userStore.UpdateAsync(user);
            return user.LockoutEnabled;
        }

        #endregion

    }
}
