using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using RolesIdentityAddress = WebStore.Interfaces.ServiceAddress.Identity.Roles;

namespace WebStore.ServiceHosting.Controllers.Identity
{
    [Route(RolesIdentityAddress.Name)]
    [ApiController]
    public class RolesApiController : ControllerBase
    {
        private readonly RoleStore<Role> _roleStore;

        public RolesApiController(WebStoreDB db)
        {
            _roleStore = new RoleStore<Role>(db);
        }

        [HttpPost]
        public async Task<bool> CreateAsync([FromBody] Role role)
        {
            var result = await _roleStore.CreateAsync(role);
            return result.Succeeded;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync([FromBody] Role role)
        {
            var result = await _roleStore.UpdateAsync(role);
            return result.Succeeded;
        }

        [HttpPost(RolesIdentityAddress.Delete)]
        public async Task<bool> DeleteAsync([FromBody] Role role)
        {
            var result = await _roleStore.DeleteAsync(role);
            return result.Succeeded;
        }

        [HttpPost(RolesIdentityAddress.GetRoleId)]
        public async Task<string> GetRoleIdAsync([FromBody] Role role)
        {
            var result = await _roleStore.GetRoleIdAsync(role);
            return result;
        }

        [HttpPost(RolesIdentityAddress.GetRoleName)]
        public async Task<string> GetRoleNameAsync([FromBody] Role role)
        {
            var result = await _roleStore.GetRoleNameAsync(role);
            return result;
        }

        [HttpPost(RolesIdentityAddress.SetRoleName + "/{roleName}")]
        public Task SetRoleNameAsync([FromBody] Role role, string roleName)
        {
            return _roleStore.SetRoleNameAsync(role, roleName);
        }

        [HttpPost(RolesIdentityAddress.GetNormalizedRoleName)]
        public async Task<string> GetNormalizedRoleNameAsync([FromBody] Role role)
        {
            var result = await _roleStore.GetRoleNameAsync(role);
            return result;
        }

        [HttpPost(RolesIdentityAddress.SetNormalizedRoleName + "/{normalizedName}")]
        public Task SetNormalizedRoleNameAsync([FromBody] Role role, string normalizedName)
        {
            return _roleStore.SetNormalizedRoleNameAsync(role, normalizedName);
        }

        [HttpGet(RolesIdentityAddress.FindById + "/{roleId}")]
        public async Task<Role> FindByIdAsync(string roleId)
        {
            var result = await _roleStore.FindByIdAsync(roleId);
            return result;
        }

        [HttpGet(RolesIdentityAddress.FindByName + "/{normalizedRoleName}")]
        public async Task<Role> FindByNameAsync(string normalizedRoleName)
        {
            var result = await _roleStore.FindByNameAsync(normalizedRoleName);
            return result;
        }
    }
}
