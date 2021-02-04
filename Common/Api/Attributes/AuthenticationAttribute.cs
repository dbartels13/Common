using System;
using Sphyrnidae.Common.Api.Models;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Attributes
{
    /// <summary>
    /// Represents how authentication should be done on the request (middleware will authenticate based on this attribute)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationAttribute : Attribute
    {
        /// <summary>
        /// The type of authentication to occur
        /// </summary>
        public AuthenticationType Type { get; }

        /// <summary>
        /// The roles a user must have (AuthenticationType = Jwt)
        /// </summary>
        public string Role { get; }

        /// <summary>
        /// Allows you to specify the AuthenticationAttribute
        /// </summary>
        /// <param name="type">Required: the AuthenticationType</param>
        public AuthenticationAttribute(AuthenticationType type) => Type = type;

        /// <summary>
        /// Allows you to specify the AuthenticationAttribute
        /// </summary>
        /// <param name="role">Specify this if the authenticated user must have the provided role (AuthenticationType = Jwt)</param>
        public AuthenticationAttribute(string role)
        {
            Role = role;
            Type = AuthenticationType.Jwt;
        }
    }
}