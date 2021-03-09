# Application Settings {#ApplicationMd}

## Overview {#ApplicationOverviewMd}
Application Settings are those settings that are application-specific.
Because this is application-specific, there is no default implementation.
It will be up to you to implement the interface, and setup Dependency Injection (DI) for this implementation.
Best practice is to place your implementation in a "Settings" folder off the root of your project.

The [IApplicationSettings](@ref Sphyrnidae.Common.Application.IApplicationSettings) interface has the following properties to implement:
1. Name (The name of your application)
2. Description (Description for your application)
3. ContactName (Name of the contact person - eg. yourself)
4. ContactEmail (Email address of the contact person)
5. Environment (Name of the hosting environment - should be pulled from IWebHostEnvironment.EnvironmentName)

Interface: [IApplicationSettings](@ref Sphyrnidae.Common.Application.IApplicationSettings)

Mock: [ApplicationSettingsMock](@ref Sphyrnidae.Common.Application.ApplicationSettingsMock)

Implementation: None (You must implement)

## Where Used {#ApplicationWhereUsedMd}
1. [AuthenticationMiddleware](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware) and [IApiAuthenticationWebService](@ref Sphyrnidae.Common.WebServices.IApiAuthenticationWebService): API to API authentication
2. Swagger Documentation
3. [IEmail](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmail): Application name appears in some email subject emails
4. [IFeatureToggleServices](@ref Sphyrnidae.Common.FeatureToggle.Interfaces.IFeatureToggleServices): Features can be application-specific
5. [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger): Application is logged on every statement
6. [IUserPreferenceServices](@ref Sphyrnidae.Common.UserPreference.Interfaces. IUserPreferenceServices): User preferences can be application-specific
7. [IVariableServices](@ref Sphyrnidae.Common.Variable.Interfaces.IVariableServices): Variables/Configurations can be application-specific

## Examples {#ApplicationExampleMd}
<pre>
    public class MyApplicationSettings : IApplicationSettings
    {
        protected IWebHostEnvironment WebHost { get; }
        public MyApplicationSettings(IWebHostEnvironment webHost) => WebHost = webHost;
        public string Name => "My Application";
        public string Description => "Description of my application";
        public string ContactName => "Me";
        public string ContactEmail => "foo@foo.com";
        public string Environment => WebHost.EnvironmentName;
    }
</pre>