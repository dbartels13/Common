// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.Environment
{
    /// <inheritdoc />
    public class EnvironmentalSettingsMock : IEnvironmentSettings
    {
        public virtual string Get(string name) => string.Empty;
    }
}