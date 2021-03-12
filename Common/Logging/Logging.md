# Logging {#LoggingMd}

## Overview {#LoggingOverviewMd}
Logging has always been an afterthought for too many organizations.
The [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger) interface brings a robust set of logging to your application.
What makes this special isn't the mere ability to log, but rather the content of what gets logged, the configurability, and the performance.
Most loggers are synchronous, meaning that your application has to wait while the logging occurs.
In this implementation, logging is asynchronous (the retrieval of some data may be synchronous).

Here are the basic steps for how something gets logged:
1. Logging statement appears in your code (see [Statements](@ref LoggingStatementsMd))
2. Precheck occurs to determine if logging will actually occur for this call (see [Precheck](@ref LoggingPrecheckMd))
3. The logger will initiate an object containing all the information to be logged (see [Information](@ref LoggingInformationMd))
4. The logger will kick off an asynchronous thread to do the actual logging (see [Asynchronous](@ref LoggingAsyncMd))
5. Additional properties on the logging object are set (see [Additional Properties](@ref LoggingAdditionalPropertiesMd))
6. Logging object is sent to all enabled loggers (see [Loggers](@ref LoggingLoggersMd))

If your logging call is for something that will run a timer (eg. API, Web Service, Database, etc),
the logging object will be returned to the caller.
When the action has completed, this object is passed to a completion logging statement where the following occurs:
1. Ensure a valid logging object was given (if this object is null, nothing will be logged)
2. Logger will capture any additional information to be logged (see [Update Information](@ref LoggingUpdateInfoMd))
3. Stops the timer that was started in the initial logging statement
4. The logger will kick off an asynchronous thread to the the rest of the work (see [Asynchronous](@ref LoggingAsyncMd))
5. New thread will wait for the initial logging statement to complete (see [Previous Completion](@ref LoggingPreviousCompletionMd))
6. Additional properties on the logging object are set (see [Additional Properties](@ref LoggingAdditionalPropertiesMd))
7. [Alerts](@ref AlertsMd) will be checked and possibly triggered
8. Logging object is sent to all enabled loggers (see [Loggers](@ref LoggingLoggersMd))

Lastly:
1. Any exceptions that occurred during logging will be captured via Email (see [Exception Handling](@ref LoggingExceptionsMd))
2. You can customize what gets logged (see [Customizations](@ref LoggingCustomizationsMd))
3. There are many configurations via this implementation using [ILoggers](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggers),
[ILoggerInformation](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerInformation),
and [ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration)

Interface: [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger)

Mock: [NonLogger](@ref Sphyrnidae.Common.Logging.NonLogger)

Implementation: [Logger](@ref Sphyrnidae.Common.Logging.Logger)

## Statements {#LoggingStatementsMd}
The [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger) lists all the possible calls that you can make to the logger.
These are broken into 2 categories:
1. Insert-only statements
2. Updateable statements

<h2>Insert-Only</h2>
All of these logging methods will log what needs to be logged (no subsequent logging statements can be made with the logging objects).
<table>
    <tr>
        <th>Method
        <th>Information Class
        <th>Where Used
        <th>Description
    <tr>
        <td>Log
        <td>[MessageInformation](@ref Sphyrnidae.Common.Logging.Information.MessageInformation)
        <td>[HttpDataMiddleware](@ref Sphyrnidae.Common.Api.Middleware.HttpDataMiddleware): (Warning) If a non-secure request is received when HTTPS is required
        <br />[SignalR](@ref Sphyrnidae.Common.SignalR.SignalR): (Suspend) On SignalR communication issues
        <td>Logs a simple user-defined message
    <tr>
        <td>Unauthorized
        <td>[UnauthorizedInformation](@ref Sphyrnidae.Common.Logging.Information.UnauthorizedInformation)
        <td>[AuthenticationMiddleware](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware): Unauthenticated or Unauthorized request
        <td>Logs that an unauthorized call was made in the system (eg. HTTP 401/403)
    <tr>
        <td>Exception
        <td>[ExceptionInformation](@ref Sphyrnidae.Common.Logging.Information.ExceptionInformation)
        <td>[ExceptionMiddleware](@ref Sphyrnidae.Common.Api.Middleware.ExceptionMiddleware)
        <td>Records that an unhandled (fatal) exception occurred in the system
    <tr>
        <td>HiddenException
        <td>[ExceptionInformation](@ref Sphyrnidae.Common.Logging.Information.ExceptionInformation)
        <td>[SafeTry.LogException](@ref Sphyrnidae.Common.Utilities.SafeTry)
        <td>Records that an exception occurred in the system that was non-fatal
    <tr>
        <td>Custom1
        <td>[CustomInformation1](@ref Sphyrnidae.Common.Logging.Information.CustomInformation1)
        <td>
        <td>Allows you to create your own design-time class (see [Customizations](@ref LoggingCustomizationsMd))
    <tr>
        <td>Custom2
        <td>[CustomInformation2](@ref Sphyrnidae.Common.Logging.Information.CustomInformation2)
        <td>
        <td>Allows you to create your own design-time class (see [Customizations](@ref LoggingCustomizationsMd))
    <tr>
        <td>Custom3
        <td>[CustomInformation3](@ref Sphyrnidae.Common.Logging.Information.CustomInformation3)
        <td>
        <td>Allows you to create your own design-time class (see [Customizations](@ref LoggingCustomizationsMd))
    <tr>
        <td>Generic
        <td>[BaseLogInformation](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation)
        <td>
        <td>Allows you to create your own generic design-time class (see [Customizations](@ref LoggingCustomizationsMd))
</table>

<h2>Updateable</h2>
The following methods will be called at the start of something, then again when something ends.
The [TimerBaseInformation](@ref Sphyrnidae.Common.Logging.Information.TimerBaseInformation) object will be returned from that "entry" call,
and this object should be passed to the "exit" method.
<table>
    <tr>
        <th>Entry Method
        <th>Exit Method
        <th>Information Class
        <th>Where Used
        <th>Description
    <tr>
        <td>AttributeEntry
        <td>AttributeExit
        <td>[AttributeInformation](@ref Sphyrnidae.Common.Logging.Information.AttributeInformation)
        <td>None - there are some [Attributes](@ref Sphyrnidae.Common.Api.Attributes), but none that are logging anything
        <td>Logs the execution of an attribute
    <tr>
        <td>ApiEntry
        <td>ApiExit
        <td>[ApiInformation](@ref Sphyrnidae.Common.Logging.Information.ApiInformation)
        <td>[ApiLoggingMiddleware](@ref Sphyrnidae.Common.Api.Middleware.ApiLoggingMiddleware)
        <td>Logs the execution of the API endpoint
    <tr>
        <td>DatabaseEntry
        <td>DatabaseExit
        <td>[DatabaseInformation](@ref Sphyrnidae.Common.Logging.Information.DatabaseInformation)
        <td>[Repository Calls](@ref DalMd)
        <br />[Transactions](@ref DalTransactionsMd)
        <td>Logs the execution of a database call
    <tr>
        <td>WebServiceEntry
        <td>WebServiceExit
        <td>[WebServiceInformation](@ref Sphyrnidae.Common.Logging.Information.WebServiceInformation)
        <td>[Web Services](@ref WebServiceMd): The actual call, not the preparation, nor parsing of result
        <td>Logs the execution of a web service call
    <tr>
        <td>MiddlewareEntry
        <td>MiddlewareExit
        <td>[MiddlewareInformation](@ref Sphyrnidae.Common.Logging.Information.MiddlewareInformation)
        <td>[ApiLoggingMiddleware](@ref Sphyrnidae.Common.Api.Middleware.ApiLoggingMiddleware)
        <br />[AuthenticationMiddleware](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware)
        <br />[ExceptionMiddleware](@ref Sphyrnidae.Common.Api.Middleware.ExceptionMiddleware)
        <br />[JwtMiddleware](@ref Sphyrnidae.Common.Api.Middleware.JwtMiddleware)
        <td>Logs the execution of a middleware call in the [API Pipeline](@ref ApiPipelineMd)
    <tr>
        <td>TimerStart
        <td>TimerEnd
        <td>[TimerBaseInformation](@ref Sphyrnidae.Common.Logging.Information.TimerBaseInformation)
        <td>
        <td>Allows you to create ad-hoc timer classes in your code for tracking how long sections of code run (see [Customizations](@ref LoggingCustomizationsMd))
    <tr>
        <td>CustomTimer1Start
        <td>CustomTimerEnd
        <td>[CustomTimerInformation1](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation1)
        <td>
        <td>Allows you to create your own design-time class which acts as a timer (see [Customizations](@ref LoggingCustomizationsMd))
    <tr>
        <td>CustomTimer2Start
        <td>CustomTimerEnd
        <td>[CustomTimerInformation2](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation2)
        <td>
        <td>Allows you to create your own design-time class which acts as a timer (see [Customizations](@ref LoggingCustomizationsMd))
    <tr>
        <td>CustomTimer3Start
        <td>CustomTimerEnd
        <td>[CustomTimerInformation3](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation3)
        <td>
        <td>Allows you to create your own design-time class which acts as a timer (see [Customizations](@ref LoggingCustomizationsMd))
    <tr>
        <td>Entry
        <td>Exit
        <td>User-supplied [TimerBaseInformation](@ref Sphyrnidae.Common.Logging.Information.TimerBaseInformation)
        <td>
        <td>If you don't want to use the pre-built classes inheriting from [TimerBaseInformation](@ref Sphyrnidae.Common.Logging.Information.TimerBaseInformation),
        you can create your own customized object (see [Customizations](@ref LoggingCustomizationsMd))
</table>

## Precheck {#LoggingPrecheckMd}
The pre-check step is a performance enhancement to quickly return if the "Type" (see [customizations](@ref LoggingConfigurationsMd) of object being logged is not enabled.
The [ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration)
(see [configurations](@ref LoggingConfigurationsMd))
has 2 properties that drive this:
1. [Enabled](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration.Enabled)
2. LoggerEnabled()

If both of these return true, then the precheck passes.
If either of these return false, the precheck fails and the logging statement will conclude without performing any data lookups or actual logging.


## Information {#LoggingInformationMd}
The information that gets logged is critical, and is a huge reason for the complexity of this logging sytem.
All things that will be logged will contain the following pieces of information (see [BaseLogInformation](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation)):
<table>
    <tr>
        <th>Property
        <th>Description
        <th>Customizeable
    <tr>
        <td>[Identifier](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Identifier)
        <td>A unique identifier (Guid) for each log [statement](@ref LoggingStatementsMd).
        This identifier will be used to link the 'insert' statement with the 'update' statement.
        <td>
    <tr>
        <td>[Timestamp](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Timestamp)
        <td>When the logging statement was called
        <td>
    <tr>
        <td>[Severity](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Severity)
        <td>Similar to logging level, this gives an indication of how critical the message is.
        See <a href="https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.traceeventtype?view=net-5.0" target="blank">TraceEventType</a> for possible values
        <td>
    <tr>
        <td>[Order](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Order)
        <td>All statements within a Request will be ordered.
        You can sort by this column to see all the statements for a given request in the correct order.
        Any calls to another web service will also be ordered as part of this.
        Eg. A, B, BA, BB, C: Three statements were logged on the main request, and the 2nd "B" request was a web service call that logged 2 statements.
        <td>[LogOrderPrefix](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerInformation.LogOrderPrefix)
        <br />[LoggingOrder](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerInformation.LoggingOrder)
    <tr>
        <td>[RequestId](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.RequestId)
        <td>A unique identifier (Guid) that is used for all logging statements within a single Http Request
        <td>[RequestId](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerInformation.RequestId)
    <tr>
        <td>[Session](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Session)
        <td>If your front end application is generating a sessionId, it can pass this along to be logged.
        It should pass this in the Http header: [SessionIdHeader](@ref Sphyrnidae.Common.HttpClient.IHttpClientSettings.SessionIdHeader).
        This will tie all requests together for a given "session" (eg. multiple Http Requests using the same window without refresh)
        <td>[SessionId](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerInformation.SessionId)
    <tr>
        <td>[UserId](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.UserId)
        <td>If the user is logged in, this is their Id
        <td>
    <tr>
        <td>[IdentityProperties](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.IdentityProperties)
        <td>If you have custom items in your identity object that will always need to be logged, you should:
        1. Create your own custom class that inherits from [BaseIdentity](@ref Sphyrnidae.Common.Authentication.Identity.BaseIdentity)
        2. Add in your custom properties to that class
        3. Override the GetCustomLoggingProperties() method and return any properties in that collection
        4. In your [logger](@ref LoggingLoggersMd), ensure [IncludeIdentity](@ref Sphyrnidae.Common.Logging.Loggers.BaseLogger.IncludeIdentity) is enabled
        5. Retrieve that property from the [dictionary](@ref Sphyrnidae.Common.Logging.Loggers.Models.LogInsert.Other)
        <td>
    <tr>
        <td>[Message](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Message)
        <td>The main message to be logged
        <td>
    <tr>
        <td>[Category](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Category)
        <td>A category to group messages.
        For Timer statements (eg. has Entry() and Exit() methods), the Category will match the [Type](@ref LoggingConfigurationsMd)
        <td>
    <tr>
        <td>[Machine](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Machine)
        <td>Name of the physical machine this code was running on
        <td>
    <tr>
        <td>[Application](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Application)
        <td>Name of the application (eg. The same application could be running on multiple machines for load balancing)
        <td>
    <tr>
        <td>[StaticProperties](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.StaticProperties)
        <td>Any additional properties you want included:
        1. Derive and implement [StaticProperties](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerInformation.StaticProperties)
        2. Update service registration (startup.cs) with your new class
        3. In your [logger](@ref LoggingLoggersMd), ensure [IncludeStatic](@ref Sphyrnidae.Common.Logging.Loggers.BaseLogger.IncludeStatic) is enabled
        4. Retrieve that property from the [dictionary](@ref Sphyrnidae.Common.Logging.Loggers.Models.LogInsert.Other)
        <td>
</table>

Most of these properties get set immediately in the context of the active application thread.
However, the following properties are set [later](@ref LoggingAdditionalPropertiesMd) since they do not depend on an active object on the main thread:
1. [Machine](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Machine)
2. [Application](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Application)

There is a slight overhead to setting all of these properties, but they are small and have been optimized.
Any other custom properties should be set as follows:
1. Constructor: Never
2. Initialize(): This is what gets called to save off any information that was passed into the logging call. You should avoid directly setting custom properties in this method unless it is not thread-safe. You should instead just save off the object(s) into a member variable(s).
3. SetProperties(): This is where you'll take the saved object(s) from Initialize() and actually set the real properties (see [Additional Properties](@ref LoggingAdditionalPropertiesMd))

Here are all of the custom properties for each logging type.
Note that the "key" is provided, and can be used to "hide" the value (see [configurations](@ref LoggingConfigurationsMd) for more information around these).
<table>
    <tr>
        <th>Type
        <th>Key
        <th>When Set
        <th>Description
    <tr>
        <td>API
        <td>Route
        <td>Initialize
        <td>Unique route (template)
    <tr>
        <td>API
        <td>Http Method
        <td>Initialize
        <td>Request Method (eg. POST/PUT/GET/ETC)
    <tr>
        <td>API
        <td>Http Headers
        <td>Initialize (Formatting done in SetProperties)
        <td>The entire collection of Http Request headers
    <tr>
        <td>API
        <td>Query String
        <td>Initialize (Formatting done in SetProperties)
        <td>The querystring in the URL
    <tr>
        <td>API
        <td>Form Data
        <td>Initialize (Formatting done in SetProperties)
        <td>Form data in the Http Request
    <tr>
        <td>API
        <td>Request Data
        <td>Initialize (Formatting done in SetProperties)
        <td>Data in the Http Request that was posted (or other method)
    <tr>
        <td>API
        <td>Browser
        <td>Initialize
        <td>Type of user browser (Eg. "User-Agent" header)
    <tr>
        <td>API
        <td>Http Response
        <td>External Call
        <td>Http Response Status Code (Eg. 200)
    <tr>
        <td>API
        <td>Http Result
        <td>External Call
        <td>The complete Http Response object
    <tr>
        <td>Attribute
        <td>User-Supplied
        <td>Initialize
        <td>Any attribute information to be saved
    <tr>
        <td>Database
        <td>Connection
        <td>Initialize
        <td>Name of the [database connection repo](@ref Sphyrnidae.Common.Dal.BaseRepo.CnnName)
    <tr>
        <td>Database
        <td>SQL Parameters
        <td>SetProperties
        <td>SQL Parameters as name/value pairs
    <tr>
        <td>Exception, Hidden Exception
        <td>Stack Trace
        <td>SetProperties
        <td>The stack trace of the exception
    <tr>
        <td>Exception, Hidden Exception
        <td>Source
        <td>SetProperties
        <td>Source of the exception
    <tr>
        <td>Exception, Hidden Exception
        <td>Title
        <td>Initialize
        <td>Type/Title for the exception
    <tr>
        <td>Web Service
        <td>Route
        <td>Initialize
        <td>Unique route for the web service call
    <tr>
        <td>Web Service
        <td>Http Method
        <td>Initialize
        <td>Request Method (eg. POST/PUT/GET/ETC)
    <tr>
        <td>Web Service
        <td>Request Data
        <td>SetProperties
        <td>Data in the Http Request that was posted (or other method)
    <tr>
        <td>Web Service
        <td>Http Response
        <td>External Call
        <td>Http Response Status Code (Eg. 200)
    <tr>
        <td>Web Service
        <td>Http Result
        <td>External Call
        <td>The complete Http Response object
</table>

## Asynchronous {#LoggingAsyncMd}
The [Logger](@ref Sphyrnidae.Common.Logging.Logger) implementation of the [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger) interface runs asynchronously.
The logging [statement](@ref LoggingStatementsMd), [precheck](@ref LoggingPrecheckMd), and setting of [information](@ref LoggingInformationMd) runs synchronously.
But the remainder of the call will be done asynchronously (in the background, no await).
Eg. [SetProperties()](@ref LoggingAdditionalPropertiesMd) and [loggers](@ref LoggingLoggersMd).
Even when you await the completion of the [statement](@ref LoggingStatementsMd), this will continue to execute asynchronously.
It is because of this asynchronous nature that you should be careful what gets set in Initialize() vs SetProperties() methods.
Eg. the HttpRequest could be gone by the time the logger is used to actually log information related to this,
so anything that relies on the HttpRequest object (or anything else that isn't thread-safe) should go in the Initialize() method.
If you can find a way to push this into the SetProperties() method, or at least do the majority of processing in that method,
then your overall logging performance will increase since those items will run asynchronously and not cause the main thread to wait during Initialize().


## Additional Properties {#LoggingAdditionalPropertiesMd}
Any custom properties on your [logging information](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation) class will need to do the following:
1. Following [setting information](@ref LoggingInformationMd), there may still be some custom properties that need to be set
2. Obfuscation of the values based on HideKeys() (see [ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration))
3. Taking the property, and assigning it to a Dictionary(string, string) - eg. key/value

For non-timer statements, and for all entry() methods, SetProperties() is where this will all be done.
The 3 dictionaries (HighProperties, MedProperties, LowProperties) are used by the [loggers](@ref LoggingLoggersMd).
Eg. some loggers might only save the 'High' properties.

For update timer statements (eg. exit() methods), UpdateProperties() is where this will all be done.
Before this method is called, the 3 dictionaries will be cleared out so that only updated items will be logged on the update.

<table>
    <tr>
        <th>Type
        <th>Key
        <th>Insert
        <th>Update
        <th>Dictionary
    <tr>
        <td>API
        <td>Route
        <td>Yes
        <td>
        <td>High
    <tr>
        <td>API
        <td>Http Method
        <td>Yes
        <td>
        <td>High
    <tr>
        <td>API
        <td>Http Headers
        <td>Yes
        <td>
        <td>Med
    <tr>
        <td>API
        <td>Query String
        <td>Yes
        <td>
        <td>Med
    <tr>
        <td>API
        <td>Form Data
        <td>Yes
        <td>
        <td>Med
    <tr>
        <td>API
        <td>Request Data
        <td>Yes
        <td>
        <td>Med
    <tr>
        <td>API
        <td>Browser
        <td>Yes
        <td>
        <td>Low
    <tr>
        <td>API
        <td>Http Response
        <td>
        <td>Yes
        <td>High
    <tr>
        <td>API
        <td>Http Result
        <td>
        <td>Yes
        <td>Med
    <tr>
        <td>Attribute
        <td>User-Supplied
        <td>Yes
        <td>
        <td>Med
    <tr>
        <td>Database
        <td>Connection
        <td>Yes
        <td>
        <td>High
    <tr>
        <td>Database
        <td>SQL Parameters
        <td>Yes
        <td>
        <td>Med
    <tr>
        <td>Exception, Hidden Exception
        <td>Stack Trace
        <td>Yes
        <td>
        <td>High
    <tr>
        <td>Exception, Hidden Exception
        <td>Source
        <td>Yes
        <td>
        <td>Med
    <tr>
        <td>Exception, Hidden Exception
        <td>Title
        <td>Yes
        <td>
        <td>Low
    <tr>
        <td>Web Service
        <td>Route
        <td>Yes
        <td>
        <td>High
    <tr>
        <td>Web Service
        <td>Http Method
        <td>Yes
        <td>
        <td>High
    <tr>
        <td>Web Service
        <td>Request Data
        <td>Yes
        <td>
        <td>Med
    <tr>
        <td>Web Service
        <td>Http Response
        <td>
        <td>Yes
        <td>High
    <tr>
        <td>Web Service
        <td>Http Result
        <td>
        <td>Yes
        <td>Med
</table>

## Loggers {#LoggingLoggersMd}
A logger is a how the [information](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation) gets transferred to a physical storage location.
Eg. File, Database, etc.
A logger has the following characteristics:
1. Class inherits from [BaseLogger](@ref Sphyrnidae.Common.Logging.Interfaces.BaseLogger)
2. Is registered in your implementation of [ILoggers](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggers)
3. Is [configured](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration) to perform logging - LoggerEnabled() method

Available loggers (you can create your own too):
<table>
    <tr>
        <th>Type
        <th>Name
        <th>Description
        <th>Recommended Types
        <th>Default Dictionaries
    <tr>
        <td>[AwsLogger](@ref Sphyrnidae.Implementations.Common.Loggers.AwsLogger)
        <td>AWS
        <td>Sends to CloudWatch (Not currently an option... You can view <a href="https://github.com/dbartels13/Common/blob/main/Common/Logging/Loggers/AwsLogger.cs" target="blank">source code</a> for example implementation)
        <td>All
        <td>All
    <tr>
        <td>[AzureLogger](@ref Sphyrnidae.Implementations.Common.Loggers.AzureLogger)
        <td>Azure
        <td>Sends to EventHub (Not currently an option... You can view <a href="https://github.com/dbartels13/Common/blob/main/Common/Logging/Loggers/AzureLogger.cs" target="blank">source code</a> for example implementation)
        <td>All
        <td>All
    <tr>
        <td>[DebugLogger](@ref Sphyrnidae.Common.Logging.Loggers.DebugLogger)
        <td>Debug
        <td>Debugging messages in visual studio utilizing Debug.WriteLine()
        <td>Exception;Hidden Exception;HTTP Response Error;Long Running;Message;Timer
        <td>High
    <tr>
        <td>[EmailLogger](@ref Sphyrnidae.Common.Logging.Loggers.EmailLogger)
        <td>Email
        <td>Sends an [Email](@ref EmailMd) to someone
        <td>Identity;Static;High;Med
        <td>Exception;Hidden Exception
    <tr>
        <td>[FileLogger](@ref Sphyrnidae.Common.Logging.Loggers.FileLogger)
        <td>File
        <td>Writes to a file.
        A single file will be created every day in the format of $"log_{dt.DayOfYear}.log".
        File location is given by the [variable](@ref VariableMd) "Logging_FilePath" - and put into subfolder of "Logs" at that path.
        If the [severity](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Severity) of the thing being logged is Warning, Error, or Critical, then a 2nd logfile will be created.
        Location of this log file will be in a subfolder of the Logs directory called "Highlights" with a naming convention of $"log_highlights_{dt.Year}_{dt:MMM}.log".
        <td>Identity;Static;High;Med
        <td>API;Exception;Hidden Exception;HTTP Response Error;Long Running;Message;Timer;Web Service
    <tr>
        <td>[Log4NetLogger](@ref Sphyrnidae.Common.Logging.Loggers.Log4NetLogger)
        <td>Log4Net
        <td>Sends to Log4Net which has it's own logging configurations
        <td>All
        <td>All
</table>

Registration of which loggers the system knows about is done via the [ILoggers](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggers) interface.
The default registration is a Transient which returns an empty list (no loggers).
Your implementation registration may wish to be Singleton (especially if there are multiple injections or any kind of business logic/lookups).


## Update Information {#LoggingUpdateInfoMd}
This only impacts 2 calls:
1. ApiExit()
2. WebServiceExit()

In both cases, a call to SaveResult() will occur which will save off the returned Status Code and Response body.
These will be the only candidates to be logged on the update statement (along with time to execute).


## Previous Completion {#LoggingPreviousCompletionMd}
Because logging is mostly [asynchronous](@ref LoggingAsyncMd), the Exit() call might happen before the Entry() call has completed.
Depending on your logger (eg. Database), you may need to capture the generated LogID (to be placed in [NotResetProperties](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.NotResetProperties)).
The Exit() log statement will depend on this completing, and it's good practice to ensure the Entry() log statement appears 1st in the logs.
Therefore, the Exit() call will wait for the Entry() log statement to complete.
It does this by setting the [SetComplete](@ref Sphyrnidae.Common.Logging.Information.TimerBaseInformation.SetComplete) flag when the Entry() statment completes.
The Exit() statement will await the setting of this flag (technically sleep for 100ms and recheck for up to 1 minute).


## Exception Handling {#LoggingExceptionsMd}
Everything that occurs within the [asynchronous](@ref LoggingAsyncMd) portion of execution will have all exceptions handled.
Because we are already in the logger, it doesn't make sense to log the exception (possible recursive loop).
Instead, the logging error will be [e-mailed](@ref EmailMd) and any exceptions during the Email process will be ignored.
Note this utilizes the Email type: [HiddenException](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType.HiddenException).


## Configurations {#LoggingConfigurationsMd}
The [ILoggerConfiguration](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration) contains a number for configurations.
The main implementation is an abstract class [LoggerConfiguration](@ref Sphyrnidae.Common.Logging.LoggerConfiguration).
The registered implementation is [SphyrnidaeLoggerConfiguration](@ref Sphyrnidae.Common.Logging.SphyrnidaeLoggerConfiguration).
You will likely want to replace this with a different implementation.
For best performance, it is recommended to register your implementation as Scoped.
You can then save off the current configuration for all log statements in the request.

<table>
    <tr>
        <th>Property/Method
        <th>Description
        <th>Default Implementation
    <tr>
        <td>[Enabled](@ref Sphyrnidae.Common.Logging.Interfaces.ILoggerConfiguration.Enabled)
        <td>Global flag that allows you to completely disable logging. If this is true, a lot more checks might still prevent statements from being logged.
        <td>True
    <tr>
        <td>TypeEnabled()
        <td>The [Type](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Type) of the thing being logged can be enabled/disabled.
        See the Types table below.
        Enabled Types are a semicolon delimited list.
        <td>This list is retrieved from the [configuration variable](@ref VariableMd) "Logging_Enabled_Types"
    <tr>
        <td>Include()
        <td>The information gathered for a class might be optional to actually "log" that information.
        Included properties are a semicolon delimited list.
        <td>This list is retrieved from the [configuration variable](@ref VariableMd) "Logging_Includes"
    <tr>
        <td>LoggerEnabled()
        <td>Determines if a [logger](@ref LoggingLoggersMd) is enabled given a type.
        Name of enabled loggers are a semicolon delimited list.
        Types that are supported for a given logger are a semicolon delimited list.
        See the Types table below.
        <td>List of enabled loggers is retrieved from the [configuration variable](@ref VariableMd) "Logging_Enabled_Loggers".
        List of types enabled for a given logger is retrieved from the [configuration variable](@ref VariableMd) $"Logging_Enabled_{name}_Types".
    <tr>
        <td>HideKeys()
        <td>Listing of any custom properties that will be obfuscated in the log. Eg. Passwords.
        The property name will be the "key" of the item inserted into one of the dictionaries that will be read by the logger.
        Property keys are a semicolon delimited list.
        <td>This list is retrieved from the [configuration variable](@ref VariableMd) "Logging_HideKeys"
    <tr>
        <td>MaxLength()
        <td>Maximum length of a custom property to be logged given a logger.
        This is useful for not logging an image, or large set of text.
        <td>1000: Also could be retrieved from the [configuration variable](@ref VariableMd) $"Logging_{loggerName}_MaxLength"
</table>

Existing [Types](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation.Type) (You can also create your own "Type" - see [Customizations](@ref LoggingCustomizationsMd)):
<table>
    <tr>
        <th>Type
        <th>Info Class
        <th>Statement
        <th>Severity
    <tr>
        <td>API
        <td>[ApiInformation](@ref Sphyrnidae.Common.Logging.Information.ApiInformation)
        <td>ApiEntry()
        <td>Information
    <tr>
        <td>Attribute
        <td>[AttributeInformation](@ref Sphyrnidae.Common.Logging.Information.AttributeInformation)
        <td>AttributeEntry()
        <td>Verbose
    <tr>
        <td>Database
        <td>[DatabaseInformation](@ref Sphyrnidae.Common.Logging.Information.DatabaseInformation)
        <td>DatabaseEntry()
        <td>Verbose
    <tr>
        <td>Exception
        <td>[ExceptionInformation](@ref Sphyrnidae.Common.Logging.Information.ExceptionInformation)
        <td>Exception()
        <td>Error
    <tr>
        <td>Hidden Exception
        <td>[ExceptionInformation](@ref Sphyrnidae.Common.Logging.Information.ExceptionInformation)
        <td>HiddenException()
        <td>Warning
    <tr>
        <td>HTTP Response Error
        <td>[HttpResponseInformation](@ref Sphyrnidae.Common.Logging.Information.HttpResponseInformation)
        <td>None - see [Alerts](@ref AlertsMd)
        <td>Warning
    <tr>
        <td>Long Running
        <td>[LongRunningInformation](@ref Sphyrnidae.Common.Logging.Information.LongRunningInformation)
        <td>None - see [Alerts](@ref AlertsMd)
        <td>Warning
    <tr>
        <td>Message
        <td>[MessageInformation](@ref Sphyrnidae.Common.Logging.Information.MessageInformation)
        <td>Log()
        <td>User-Supplied
    <tr>
        <td>Middleware
        <td>[MiddlewareInformation](@ref Sphyrnidae.Common.Logging.Information.MiddlewareInformation)
        <td>MiddlewareEntry()
        <td>Information
    <tr>
        <td>Timer
        <td>[TimerInformation](@ref Sphyrnidae.Common.Logging.Information.TimerInformation)
        <td>TimerStart()
        <td>Information
    <tr>
        <td>Unauthorized
        <td>[UnauthorizedInformation](@ref Sphyrnidae.Common.Logging.Information.UnauthorizedInformation)
        <td>Unauthorized()
        <td>Warning
    <tr>
        <td>Web Service
        <td>[WebServiceInformation](@ref Sphyrnidae.Common.Logging.Information.WebServiceInformation)
        <td>WebServiceEntry()
        <td>Verbose
    <tr>
        <td>Developer Supplied
        <td>[CustomInformation1](@ref Sphyrnidae.Common.Logging.Information.CustomInformation1)
        <br />[CustomInformation2](@ref Sphyrnidae.Common.Logging.Information.CustomInformation2)
        <br />[CustomInformation3](@ref Sphyrnidae.Common.Logging.Information.CustomInformation3)
        <br />[CustomTimerInformation1](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation1)
        <br />[CustomTimerInformation2](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation2)
        <br />[CustomTimerInformation3](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation3)
        <br />Custom
        <br />Custom
        <td>Custom1()
        <br />Custom2()
        <br />Custom3()
        <br />CustomTimer1Start()
        <br />CustomTimer2Start()
        <br />CustomTimer3Start()
        <br />Generic()
        <br />Entry()
        <td>
</table>

## Customizations {#LoggingCustomizationsMd}
Besides having most features be highly configurable, you may wish to customize the logger in any number of ways.

<h2>Adding things to be logged</h2>
You can extend any of the [BaseLogInformation](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation) classes to add properties.
If you want to update or remove properties, you can update these in the [SetProperties](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation) or [UpdateProperties](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation) methods.

Once you have added a property by extending the intended class, you can register your own class instead of the default class.

Eg.
<pre>
    // Extend a given class by adding a property
    public class MyNewExceptionInformation : ExceptionInformation
    {
        private string MyNewProperty { get; set; }
        public MyNewExceptionInformation(ILoggerInformation info, IApplicationSettings appSettings) : base(info, appSettings) { }

        public override void Initialize(Exception ex, string title, bool messageOnly)
        {
            base.Initialize(ex, title, messageOnly);
            MyNewProperty = "Some Value";
        }

        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);
            MedProperties.Add("My New Property", MyNewProperty);
        }
    }

    // Register this so that this class everywhere in place of ExceptionInformation (startup.cs)
    services.AddTransient<ExceptionInformation, MyNewExceptionInformation>();
</pre>

<h2>Design-time capabilities</h2>
If you want to keep things cleaner, you can create your own logging class.
To do this, you'll first inherit from one of the following "custom" classes:
1. [CustomInformation1](@ref Sphyrnidae.Common.Logging.Information.CustomInformation1)
2. [CustomInformation2](@ref Sphyrnidae.Common.Logging.Information.CustomInformation2)
3. [CustomInformation3](@ref Sphyrnidae.Common.Logging.Information.CustomInformation3)
4. [CustomTimerInformation1](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation1)
5. [CustomTimerInformation2](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation2)
6. [CustomTimerInformation3](@ref Sphyrnidae.Common.Logging.Information.CustomTimerInformation3)

All of these interface methods take a generic type as the argument with an optional message.
The generic type gets saved and you will, in your class, handle the logging of this generic object by assigning all of it's properties into the proper dictionary (High/Med/Low properties).

You will then add a service registration to specify that your class will be used when calling the proper method on the [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger) interface.
Note that when you wish to log something, this will use the "Custom()" methods on the interface.
This method takes a generic object, which may or may not be the same type as you are expecting, so there is a potential for run-time anomolies.

If you wish to have a better name and work around these anomolies, it's recommended that you:
1. Create your own interface with the proper method signature - this interface should inherit from [ILogger](@ref Sphyrnidae.Common.Logging.Interfaces.ILogger)
2. Implement this method in a class which inherits from [Logger](@ref Sphyrnidae.Common.Logging.Logger)
3. The implementation should simply forward on the call to the proper "Custom()" method.

Eg.
<pre>
    // Create your class which will act as the generic type
    public class Widget
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // Create your logging class which inherits from one of the CustomInformation classes
    public class WidgetInformation : CustomInformation1<Widget>
    {
        protected WidgetInformation(ILoggerInformation info, IApplicationSettings appSettings) : base(info, appSettings) { }
        public override string Type => "Widget"; // Required override
        protected override TraceEventType CustomSeverity => TraceEventType.Verbose; // Required override
        protected override string CustomMessage => "A widget was created"; // Optional override which means this will be used instead of a user-supplied message
        // Save off any properties you wish to have recorded - or you could just serialize the entire object into one of the dictionaries
        protected override void SetHighProperties(Dictionary<string, string> highProperties, Widget obj) => highProperties.Add("Name", obj.Name); 
        protected override void SetLowProperties(Dictionary<string, string> lowProperties, Widget obj) => lowProperties.Add("Id", obj.Id.ToString());
        protected override void SetMedProperties(Dictionary<string, string> highProperties, Widget obj) { }
    }

    // Service registration (startup.cs)
    services.TryAddTransient<CustomInformation1<Widget>, WidgetInformation>();

    // Test call
    ILogger logger; // Should be injected
    await logger.Custom1(new Widget { Name="Foo", Id=1 });
    await logger.Custom1("a string object"); // This will compile, but will be a run-time exception with service lookup

    // Alternative: Inherit and extend the interface
    public interface ICustomizedLogger : ILogger
    {
        Task Widget(Widget widget);
    }

    // Implementation
    public class CustomizedLogger : Logger, ICustomizedLogger
    {
        public CustomizedLogger(ILoggerConfiguration config, ILoggers loggers, IServiceProvider provider, IAlert alert, LongRunningInformation longRunningInfo, HttpResponseInformation httpResponseInfo, IApplicationSettings app, IEmail email)
            : base(config, loggers, provider, alert, longRunningInfo, httpResponseInfo, app, email)
            { }

        public void Widget(Widget widget) => Custom1(widget);
    }

    // Registration (Note that the ILogger can still be used, but if you want to use your customized calls, you should use this one)
    services.TryAddTransient<ICustomizedLogger, CustomizedLogger>();

    // Make the new call
    ICustomizedLogger logger; // Should be injected
    logger.Widget(new Widget { Name="Foo", Id=1 });
</pre>

<h2>Additional Design-time capabilities</h2>
You only create 3 Insert-Only custom classes, and 3 Updateable custom classes.
If you need more, then you'll have to take a slightly different approach.
Or, if you need to customize a class that will take more than 1 object as a parameter
(You can always make this a single object by using a container object).

The approach is similar to the custom classes,
except that you'll be directly inheritied from [BaseLogInformation](@ref Sphyrnidae.Common.Logging.Information.BaseLogInformation),
or from [TimerBaseInformation](@ref Sphyrnidae.Common.Logging.Information.TimerBaseInformation) (for timers).
This class should be (see below for example):
1. Contain all the parameters/objects you wish
2. Have all custom properties settable via a single method (eg. Initialize() which will first call InitializeBase())
3. Ensure the custom properties are sent to the loggers via SetProperties()
4. This class itself should be passed in to the proper method
    1. Non-Timer: Generic()
    2. Timer: Entry() and Exit()

Eg.
<pre>
    // Create your class inheriting from BaseLogInformation
    public class WidgetInformation : BaseLogInformation
    {
        public override string Type => "Widget";
        public string Name { get; private set; }
        public int Id { get; private set; }
        public WidgetInformation(ILoggerInformation info, IApplicationSettings appSettings) : base(info, appSettings) { }

        // Best practice to do all initializing/setting in a method
        public virtual void Initialize(string name, int id)
        {
            InitializeBase(TraceEventType.Information);
            Message = "Something happened with a widget"; // Optional to set this
            Category = "Widgets"; // Optional to set this
            Name = name;
            Id = id;
        }

        // The following method is where you'll save your properties into a dictionary which will be logged
        // Note: There is an UpdateProperties() that will be called for timers to reset this information (base method clears the dictionaries)
        public override void SetProperties(ILoggerConfiguration config)
        {
            base.SetProperties(config);
            HighProperties.Add("Name", Name);
            LowProperties.Add("Id", Id.ToString());
        }
    
    // How to use this
    ILoggerInformation info; // Should be injected
    IApplicationSettings app; // Should be injected
    var widgetInfo = new WidgetInformation(info, app);
    widgetInfo.Initialize("my name", 1);

    ILogger logger = null; // Should be injected
    await logger.Generic(widgetInfo);

    // No service registration necessary, which is why you "new" up an instance
</pre>