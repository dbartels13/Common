namespace Sphyrnidae.Common.ActiveDirectoryUtilities
{
    /// <summary>
    /// The type of request that the given parameter is
    /// </summary>
    public enum ActiveDirectoryIdType
    {
        /// <summary>
        /// The given request is an Sid
        /// </summary>
        Sid,

        /// <summary>
        /// The given request is an Active Directory Distinguished Name
        /// </summary>
        DistinguishedName,

        /// <summary>
        /// The given request is unknown (will check all methods)
        /// </summary>
        Any
    }
}