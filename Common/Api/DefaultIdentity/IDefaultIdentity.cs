using Sphyrnidae.Common.Authentication;

namespace Sphyrnidae.Common.Api.DefaultIdentity
{
    /// <summary>
    /// Retrieves a default identity to be used if the person is accessing a publicly available endpoint
    /// </summary>
    public interface IDefaultIdentity
    {
        /// <summary>
        /// The default identity to use if no other identity has been provided
        /// </summary>
        SphyrnidaeIdentity Get { get; }
    }
}