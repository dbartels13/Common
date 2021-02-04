using System;

namespace Sphyrnidae.Common.Api.Attributes
{
    /// <summary>
    /// Allows you to specify that the call should NOT be logged
    /// </summary>
    /// <remarks>
    /// It is envisioned that some calls might wish to not be logged:
    /// 1: If it is called frequently
    /// 2: If it is a background call
    /// 3: If it contains media
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SkipLogAttribute : Attribute
    {
    }
}