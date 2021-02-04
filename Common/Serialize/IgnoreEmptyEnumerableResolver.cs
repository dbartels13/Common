using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sphyrnidae.Common.Serialize
{
    public class IgnoreEmptyEnumerableResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                property.ShouldSerialize = instance =>
                {
                    // this value could be in a public field or public property
                    var enumerable = member.MemberType switch
                    {
                        MemberTypes.Property => instance
                            .GetType()
                            .GetProperty(member.Name)?
                            .GetValue(instance, null) as IEnumerable,
                        MemberTypes.Field => instance
                            .GetType()
                            .GetField(member.Name)
                            .GetValue(instance) as IEnumerable,
                        _ => null
                    };

                    return enumerable == null || enumerable.GetEnumerator().MoveNext();
                    // if the list is null, we defer the decision to NullValueHandling
                };
            }

            return property;
        }
    }
}