using System.Collections.Generic;
using System.Security.Claims;

namespace WebStore.Domain.DTOs.Identity
{
    public abstract class ClaimDto : UserDto 
    { 
    }

    public class AddClaimsDto : ClaimDto
    {
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class RemoveClaimsDto : ClaimDto
    {
        public IEnumerable<Claim> Claims { get; set; }
    }

    public class ReplaceClaimsDto : ClaimDto
    {
        public Claim OldClaim { get; set; }

        public Claim NewClaim { get; set; }
    }
}
