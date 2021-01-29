using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.DTOs.Identity
{
    public abstract class UserDto
    {
        public User User { get; set; }
    }

    public class PasswordHashDto : UserDto
    {
        public string Hash { get; set; }
    }

    public class AddLoginDto : UserDto
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class SetLockoutDto : UserDto
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
