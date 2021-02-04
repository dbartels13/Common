using System;
using System.ComponentModel.DataAnnotations;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Attributes
{
    /// <summary>
    /// Allows a value type property (eg. structs (Guid)) to require non-default values
    /// </summary>
    /// <remarks>For value types, this is NOT the same as required. You should use "Required" attribute for non-null checks</remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NotDefaultAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "{0} must not have the default value";

        public NotDefaultAttribute() : base(DefaultErrorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            // NotDefault doesn't necessarily mean required
            if (value is null)
                return true;

            var type = value.GetType();

            // non-null ref type
            if (!type.IsValueType)
                return true;

            // Value type checking
            var defaultValue = Activator.CreateInstance(type);
            return !value.Equals(defaultValue);
        }
    }
}