using Sphyrnidae.Common.Authentication;
using Sphyrnidae.Common.Authentication.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.BaseClasses
{
    /// <summary>
    /// Base engine class which holds common items used by most engines
    /// </summary>
    public abstract class BaseEngine
    {
        protected SphyrnidaeIdentity Identity { get; }
        protected BaseEngine(IIdentityWrapper identity) => Identity = identity.Current;
    }
}