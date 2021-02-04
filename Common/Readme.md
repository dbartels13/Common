# Overview
This Nuget package contains numerous helper methods and common functionality useful in any application.
To install, you can search Nuget for "Sphyrnidae.Common" - and contains no license (is free to all to use).
Please note that I do not currently have the source code available for use - when this is available, I'll be sure to update with a proper license.
This documentation will guide you through all of the utilities available in this package.

Additionally, I've included reference material for coding standards associated with these packages (although the guidance is generally applicable to any application regardless of this library).
- [API Setup](@ref SetupMd): How to setup your API project
- [API Standards](@ref StandardsAPIMd): Best practices for coding an API.


## Utility class types
All of the functionality in this library can be broken down into the following categories:
1. [Stand-Alone Helper Methods](@ref HelperMethods)
2. [Interface Driven Methods](@ref InterfaceMethods)
3. [Base Classes](@ref BaseClasses)

## Helper Methods {#HelperMethods}
These methods can be directly called without any interface registration.
As such, all customization in the behavior of these methods is done via method overloads.

[Active Directory](@ref Sphyrnidae.Common.ActiveDirectoryUtilities): Methods for accessing active directory (not currently supported)


## Interface Methods {#InterfaceMethods}
[Alerts](@ref AlertsMd): Alerts


## Base Classes {#BaseClasses}


Utilities:

For the most part, this project is generic.
However, the following items aren't quite generic, so you may need to alter for your use:

Models/ApiResponseObject: We are actually returning an HTTP 200 every time, with this as the structure
Extensions/HttpResponseExtensions.WriteResponse()
Models/SphyrnidaeIdentity: Will have whatever fields you need

Models/SphyrnidaeFeatureToggle: If you have additional properties/logic, this can be updated (and the implementation of IFeatureToggle should be updated too)
Models/SphyrnidaeUserPreference: If you have additional properties/logic, this can be updated (and the implementation of IUserPreferences should be updated too)
Models/SphyrnidaeVariable: If you have additional properties/logic, this can be updated (and the implementation of IVariableSettings should be updated too)

Logging:
For the most part, this project is generic.
However, the following items aren't quite generic, so you may need to alter for your use:

LogInformation/BaseInformation:
"User" property is set from the Identity
"Customer" property is set from the Identity
These may wish to be pulled from other places, or you may wish to not log them, or log other things.

Would also need to update similar things in:
Models/LogInsert
Models/LogUpdate
BaseLogger.cs


# Installation
## Internal Nuget Feed Url
To install a nuget package into your solution, you will first need to reference our internal TFS Nuget feed. Current URL is:<br />
https://use1a-tfs.visualvault.com/tfs/auersoft/_packaging/VisualVault/nuget/v3/index.json

To access current URL, go through TFS => Artifacts => Connect to feed => Package source URL

<br />

## Visual Studio Nuget Feed
Inside visual studio, select Tools => Options => Nuget Package Manager => Package Sources<br />
Name: "VisualVault" (You can call this anything you want, but "VisualVault" makes the most sense)<br />
Source: Paste in the package source URL from above.<br />
Click Update => OK

<br />

## Install in application
Inside of visual studio, you can right-click on 'Dependencies' inside your project, then select Manage Nuget Packages...<br />
Change the package source to be "VisualVault" (or whatever you named it above).<br />
Under 'Browse' tab, you should see all the packages listed.<br />
You can select any that you'd like/need, and then press the 'Install' button.

If you'd like to install all packages, then you can select the VisualVault.WebApi.Core package.

<br />

# Packages
## Overview
The nuget packages are a collection of capabilities that are common to the entire suite of applications.<br />
Each nuget package generally builds upon the previous package to provide additional capabilities.<br />
Additionally, most packages are built with .net Standard, meaning they can be used in .net Framework, or in .net Core.<br />
The packages that are .net Core specific have a ".Core" extension in the name - and are only available in a .net Core application.

If you're not sure where to start, go [here](@ref WebApiCoreMd).

## Listing
[Utilities](@ref UtilitiesMd)
This is a collection of extension methods, some share models, interfaces, and static wrappers for the interfaces.

[Utilities.Core](@ref UtilitiesCoreMd)
This is additional utility functions that are specific to .net Core.

[Logging](@ref LoggingMd)
This is a logging framework similar to other frameworks, but with a great deal of additional capability. Many components in these nuget packages utilize this logging framework.

[Logging.Core](@ref LoggingCoreMd)
This is some .net Core specific extension methods to the logging framework.

[Dal](@ref DalMd)
This is a data access layer (DAL) for connecting to databases, specifically implemented for Sql Server.

[Implementations.Common](@ref ImplementationsCommonMd)
The [Implementations](@ref VisualVault.Implementations) namespace is where all of the [services](@ref Services) are implemented. This package contains generic implementations (non-VisualVault and non-Core). Eg. These could be used by anyone.

[Implementations](@ref ImplementationsMd)
The [Implementations](@ref VisualVault.Implementations) namespace is where all of the [services](@ref Services) are implemented. This package contains VisualVault-specific implementations. Eg. Can not be used outside of the company.

[Implementations.Core](@ref ImplementationsCoreMd)
The [Implementations](@ref VisualVault.Implementations) namespace is where all of the [services](@ref Services) are implemented. This package contains generic implementations that are .net Core-specific implementations. Eg. These could be used by anyone.

[WebApi](@ref WebApiMd)
This is common methods that should be used by any Api implementation.

[WebApi.Core](@ref WebApiCoreMd)
This provides all of the capabilities of these packages in a single place for consumption by a .net Core application.

[SharedEntities](@ref SharedEntitiesMd)
This is meant to store any common business logic across multiple applications.

<i>The complete listing with versions and dependencies can be found in the development shared folder => Applications and Settings.xlsx => Nuget tab.</i>