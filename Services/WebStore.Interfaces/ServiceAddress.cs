using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Interfaces
{
    public static class ServiceAddress
    {
        public const string WebApiBaseUrl = "api/";

        public const string Values = WebApiBaseUrl + "values";

        public const string Employees = WebApiBaseUrl + "employees";

        public static class Products
        {
            public const string Name = WebApiBaseUrl + "products";

            public const string Brands = "brands";

            public const string Sections = "sections";
        }

        public static class Orders 
        {
            public const string Name = WebApiBaseUrl + "orders";

            public const string Users = "users";
        }

        public static class Identity
        {
            public static class Users
            {
                public const string Name = WebApiBaseUrl + "users";

                public const string UserId = "userId";
                public const string UserName = "userName";
                public const string NormalUserName = "normalUserName";

                public const string User = "user";
                public const string DeleteUser = User + "/delete";
                public const string FindById = User + "/find";
                public const string FindByName = User + "/normal";

                public const string Roles = "roles";
                public const string AddToRole = Roles + "/add";
                public const string RemoveFromRole = Roles + "/delete";
                public const string InRole = Roles + "/inRole";
                public const string UsersInRole = Roles + "/usersInRole";

                public const string Password = "password";
                public const string SetPasswordHash = Password + "/setHash";
                public const string GetPasswordHash = Password + "/getHash";
                public const string HasPassword = Password;

                public const string Claims = "claims";
                public const string GetClaims = Claims;
                public const string AddClaims = Claims + "/add";
                public const string ReplaceClaim = Claims + "/replace";
                public const string RemoveClaims = Claims + "/remove";
                public const string UsersForClaim = Claims + "/users";

                public const string TwoFactorEnabled = "twoFactor";
                public const string GetTwoFactorEnabled = TwoFactorEnabled + "/get";
                public const string SetTwoFactorEnabled = TwoFactorEnabled + "/set";

                //По правильному должны быть Post и Put запросы, но уже задолбался править код методички :(  
                public const string Email = "email";
                public const string SetEmail = Email + "/set";
                public const string GetEmail = Email + "/get";
                public const string GetEmailConfirmed = Email + "/getConfirmed";
                public const string SetEmailConfirmed = Email + "/setConfirmed";
                public const string GetNormalizedEmail = Email + "/getNormalized";
                public const string SetNormalizedEmail = Email + "/setNormalized";

                public const string FindByEmail = User + "/findByEmail";

                public const string Phone = "phone";
                public const string SetPhoneNumber = Phone + "/set";
                public const string GetPhoneNumber = Phone + "/get";
                public const string GetPhoneNumberConfirmed = Phone + "/getConfirmed";
                public const string SetPhoneNumberConfirmed = Phone + "/setConfirmed";

                public const string Login = "login";
                public const string AddLogin = Login + "/add";
                public const string RemoveLogin = Login + "/remove";
                public const string GetLogins = Login + "/get";

                public const string FindByLogin = User + "/findByLogin";

                public const string Lockout = "lockout";
                public const string GetLockoutEndDate = Lockout + "/getEndDate";
                public const string SetLockoutEndDate = Lockout + "/setEndDate";
                public const string IncrementAccessFailedCount = Lockout + "/accessFailed/increment";
                public const string ResetAccessFailedCount = Lockout + "/accessFailed/reset";
                public const string GetAccessFailedCount = Lockout + "/accessFailed/get";
                public const string GetLockoutEnabled = Lockout + "/getEnabled";
                public const string SetLockoutEnabled = Lockout + "/setEnabled";
            }

            public static class Roles
            {
                public const string Name = WebApiBaseUrl + "roles";

                public const string Delete = "delete";
                public const string GetRoleId = "getId";

                public const string RoleName = "name";
                public const string NormalizedRoleName = "normalizedName";

                public const string FindById = "findById";
                public const string FindByName = "findByName";
            }
        }
    }
}
