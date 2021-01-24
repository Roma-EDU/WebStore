using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Clients.Base;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
using WebStore.Interfaces.Services.Identity;
using RolesIdentityAddress = WebStore.Interfaces.ServiceAddress.Identity.Roles;

namespace WebStore.Clients.Identity
{
    public class RolesClient : BaseClient, ICustomRoleIdentity
    {
        public RolesClient(HttpClient httpClient) 
            : base(httpClient, RolesIdentityAddress.Name)
        {
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            var result = await PostAsync(role, RolesIdentityAddress.Name, cancellationToken).ConfigureAwait(false);
            var success = await result.Content.ReadAsAsync<bool>(cancellationToken);
            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            var result = await PutAsync(role, RolesIdentityAddress.Name, cancellationToken).ConfigureAwait(false);
            var success = await result.Content.ReadAsAsync<bool>(cancellationToken);
            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            var result = await PostAsync(role, RolesIdentityAddress.Delete, cancellationToken).ConfigureAwait(false);
            var success = await result.Content.ReadAsAsync<bool>(cancellationToken);
            return success ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            var result = await PostAsync(role, RolesIdentityAddress.GetRoleId, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            var result = await PostAsync(role, RolesIdentityAddress.RoleName, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            var response = await PostAsync(role, $"{RolesIdentityAddress.RoleName}/{roleName}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            role.Name = roleName;
        }

        public async Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            var result = await PostAsync(role, RolesIdentityAddress.NormalizedRoleName, cancellationToken).ConfigureAwait(false);
            return await result.Content.ReadAsAsync<string>(cancellationToken);
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            var response = await PostAsync(role, $"{RolesIdentityAddress.NormalizedRoleName}/{normalizedName}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            role.NormalizedName = normalizedName;
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return await GetAsync<Role>($"{RolesIdentityAddress.FindById}/{roleId}", cancellationToken);
        }
        
        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return await GetAsync<Role>($"{RolesIdentityAddress.FindByName}/{normalizedRoleName}", cancellationToken);
        }

        #region IDisposable

        void IDisposable.Dispose()
        {
            //Do nothing
        }

        #endregion
    }
}
