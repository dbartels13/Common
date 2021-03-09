// ReSharper disable UnusedMember.Global
namespace Sphyrnidae.Common.Application
{
    /// <inheritdoc />
    public class ApplicationSettingsMock : IApplicationSettings
    {
        public string Name => "Not Set";
        public string Description => "No Description Available";
        public string ContactName => "Developer";
        public string ContactEmail => "foo@foo.com";
        public string Environment => "localhost";
    }
}