using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Api.Attributes
{
    /// <summary>
    /// A value must be a valid regular expression syntax
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
    public class RegExAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "{0} must be a valid regular expression syntax";

        public RegExAttribute() : base(DefaultErrorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var str = value.ToString();
            if (string.IsNullOrWhiteSpace(str))
                return true;

            return SafeTry.IgnoreException(() =>
            {
                // ReSharper disable once AssignmentIsFullyDiscarded
                _ = Regex.Match("", str);
                return true;
            });
        }
    }
}
