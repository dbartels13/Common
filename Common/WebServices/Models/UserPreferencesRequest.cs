using System.ComponentModel.DataAnnotations;

namespace Sphyrnidae.Common.WebServices.Models
{
    /// <summary>
    /// A user preference being sent to the API
    /// </summary>
    public class UserPreferencesRequest
    {
        /// <summary>
        /// The application to which the user preference belongs
        /// </summary>
        [Required]
        public string Application { get; set; }

        /// <summary>
        /// The user/owner of this preference
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// They name/key/identifier for the user preference
        /// </summary>
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// The value of the user preference
        /// </summary>
        [Required]
        public string Value { get; set; }
    }
}