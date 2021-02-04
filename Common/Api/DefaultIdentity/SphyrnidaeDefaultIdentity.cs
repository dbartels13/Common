using System.Collections.Generic;
using Sphyrnidae.Common.Authentication;

namespace Sphyrnidae.Common.Api.DefaultIdentity
{
    public class SphyrnidaeDefaultIdentity : IDefaultIdentity
    {
        public virtual SphyrnidaeIdentity Get => new SphyrnidaeIdentity
        {
            Id = 16, // TODO: Look this up instead of hard-code
            Username = "Public",
            CustomerId = 1,
            FirstName = "Default",
            LastName = "User",
            Email = "noreply@bartelsfamily.net",
            Roles = new List<string>()
        };
    }
}