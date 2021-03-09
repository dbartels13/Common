using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Authentication.Identity;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.BaseClasses
{
    /// <summary>
    /// Base engine class which holds common items used by most engines
    /// </summary>
    public abstract class BaseEngine
    {
        protected BaseIdentity Identity { get; }
        protected BaseEngine(IIdentityHelper identity) => Identity = identity.Current;
    }
}