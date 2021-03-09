# API Setup {#SetupMd}

To install and successfully use this nuget package, please do the following:

<ol>
	<li>Create a .net core 3.1 application (no other versions currently supported)
	<li>Install the nuget package <a href="https://www.nuget.org/packages/Sphyrnidae.Common" target="blank">Sphyrnidae.Common</a> - eg. Install-Package Sphyrnidae.Common
	<li>Create a folder in your project called "Settings" (or "Implementations" or "Overrides" or whatever makes sense)
	<li>Project-specific implementations (to be placed inside this folder):
	<ol>
		<li>[IApplicationSettings](@ref Sphyrnidae.Common.Application.IApplicationSettings): see [Application](@ref ApplicationMd) for more information. Call this class <project>ApplicationSettings
		<li>[IFeatureToggleSettings](@ref Sphyrnidae.Common.FeatureToggle.Interfaces.IFeatureToggleSettings): see [Feature Toggle](@ref FeatureToggleMd) for more information
		<li>[IUserPreferenceSettings](@ref Sphyrnidae.Common.UserPreference.Interfaces.IUserPreferenceSettings): see [User Preferences](@ref UserPreferencesMd) for more information
		<li>[IVariableSettings](@ref Sphyrnidae.Common.Variable.Interfaces.IVariableSettings): see [Variables](@ref VariableMd) for more information
		<li>[ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration): see [Logging Configurations](@ref LoggingConfigurationsMd) for more information
	</ol>
	<li>Update the program.cs class to include environmental variables: CreateHostBuilder() => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).ConfigureAppConfiguration((hostingContext, config) => { config.AddEnvironmentVariables(); });
	<li>Update your startup.cs class to:
	<ol>
		<li>Inject IConfiguration into the constructor (save this reference off) - will call this "Config"
		<li>Instantiate and save off a new instance of [ServiceConfiguration](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration) - will call this "ServiceConfig"
		<li>Register your project-specific implementations in ConfigureServices():
		<ol>
			<li>[IApplicationSettings](@ref Sphyrnidae.Common.Application.IApplicationSettings)
			<li>[IFeatureToggleSettings](@ref Sphyrnidae.Common.FeatureToggle.Interfaces.IFeatureToggleSettings)
			<li>[IUserPreferenceSettings](@ref Sphyrnidae.Common.UserPreference.Interfaces.IUserPreferenceSettings)
			<li>[IVariableSettings](@ref Sphyrnidae.Common.Variable.Interfaces.IVariableSettings)
			<li>[ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration)
		</ol>
		<li>Last thing in ConfigureServices() should be the call to services.AddCommonServices() - note that it takes the [ServiceConfiguration](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration), as well as 3 other concrete instances:
		<ol>
			<li>var app = new &lt;project&gt;ApplicationSettings()
			<li>var env = new [EnvironmentalSettings](@ref Sphyrnidae.Common.Environment.EnvironmentalSettings)(Config)
			<li>var http = new [HttpClientSettings](@ref Sphyrnidae.Common.HttpClient.HttpClientSettings)(null, env)
		</ol>
		<li>In Configure(), replace the entire configuration/pipeline with a single call to: app.UseServices(ServiceConfig, sp);
		<ol>
			<li>app: injected IApplicationBuilder
			<li>ServiceConfig: The saved off [ServiceConfiguration](@ref Sphyrnidae.Common.Api.ServiceRegistration.Models.ServiceConfiguration)
			<li>sp: injected IServiceProvider
		</ol>
	</ol>
	<li>In launchSettings.json, ensure the "launchUrl" is configured to be "swagger"
	<li>Configure Project Settings:
		<ol>
			<li>Build => Suppress Warnings: 1701;1702;1591
			<li>Build => Treat warnings as errors => Specific warnings: ;NU1605
			<li>Build => Output => ENable XML documentations file: obj\Debug\netcoreapp3.1\&lt;IApplication.Name&gt; Api.xml
		</ol>
</ol>

That's the basic setup.
As you develop API's, be sure to thoroughly comment them with XML comments.
As you look to use other pieces of functionality in this package, there may be additional configurations you'll need to make.
