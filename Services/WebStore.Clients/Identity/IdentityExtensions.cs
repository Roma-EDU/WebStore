using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services.Identity;

namespace WebStore.Clients.Identity
{
    public static class IdentityExtensions
    {
        public static IdentityBuilder AddCustomWebStoreIdentity(this IdentityBuilder builder)
        {
            builder.Services.AddCustomWebStoreIdentity();
            return builder;
        }

        public static IServiceCollection AddCustomWebStoreIdentity(this IServiceCollection services)
        {
            //К сожалению, вместо регистрации одного ICustomUserIdentity 
            //придётся прописать все элементы нашего Identity по отдельности
            services
                .AddTransient<IUserStore<User>, UsersClient>()
                .AddTransient<IUserRoleStore<User>, UsersClient>()
                .AddTransient<IUserClaimStore<User>, UsersClient>()
                .AddTransient<IUserPasswordStore<User>, UsersClient>()
                .AddTransient<IUserTwoFactorStore<User>, UsersClient>()
                .AddTransient<IUserEmailStore<User>, UsersClient>()
                .AddTransient<IUserPhoneNumberStore<User>, UsersClient>()
                .AddTransient<IUserLoginStore<User>, UsersClient>()
                .AddTransient<IUserLockoutStore<User>, UsersClient>();

            //Аналогичная история с ICustomRoleIdentity
            services.AddTransient<IRoleStore<Role>, RolesClient>();

            return services;
        }
    }
}
