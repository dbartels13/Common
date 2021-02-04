using System;
using Sphyrnidae.Common.Authentication.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Authentication
{
    /// <inheritdoc />
    public class IdentityWrapperMock : IIdentityWrapper
    {
        public SphyrnidaeIdentity Current
        {
            get => new SphyrnidaeIdentity
            {
                CustomerId = 1,
                Expires = DateTime.MaxValue,
                FirstName = "Test",
                LastName = "User",
                Id = 1
            };
            set { }
        }
    }
}