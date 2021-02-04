namespace Sphyrnidae.Common.Authentication.Interfaces
{
    /// <summary>
    /// Settings for dealing with the Json Web Token (JWT)
    /// </summary>
    public interface ITokenSettings
    {
        /// <summary>
        /// How many minutes before a token expires
        /// </summary>
        int TokenExpirationMinutes { get; }
    }
}