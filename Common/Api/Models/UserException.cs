using System;

namespace Sphyrnidae.Common.Api.Models
{
    /// <summary>
    /// Throw this exception if the message is meant to be shown to the end user in the UI/API call
    /// Note: Any other exception will have it's returned message replaced with "Sorry, an error has occurred..." or similar message
    /// This exception type also indicates if this exception needs to be logged or not (This one will NOT, all others will be logged as errors)
    /// </summary>
    public class UserException : Exception
    {
    }
}