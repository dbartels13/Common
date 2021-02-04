using System;
using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Attributes
{
    /// <summary>
    /// A value for an enum must be present in the enum
    /// </summary>
    /// <remarks>The default value for an enum is 0, which might not be valid</remarks>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class EnumValidatorAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "{0} must have a valid enumeration value";

        public EnumValidatorAttribute() : base(DefaultErrorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            // This should never happen since enum is a value type
            if (value == null)
                return false;

            // Ensure the type is proper, and that the value is part of the enum
            var type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value);
        }
    }
}