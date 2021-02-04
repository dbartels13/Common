using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sphyrnidae.Common.Logging.Information;

namespace Sphyrnidae.Common.Logging.Interfaces
{
    /// <summary>
    /// Main logger for the system
    /// </summary>
    /// <remarks>All the configurations on how the logger works is given in ILoggerConfiguration</remarks>
    public interface ILogger
    {
        #region Generic
        Task Entry(BaseLogInformation info, Action errorAction = null);
        Task Exit(TimerBaseInformation info, Action errorAction = null);
        #endregion

        #region Log
        /// <summary>
        /// Logs a generic message
        /// </summary>
        /// <remarks>Call this function at any place in your code where you want to capture helpful information</remarks>
        /// <param name="severity">The severity level of the log request</param>
        /// <param name="message">The message to log</param>
        /// <param name="category">
        /// The category of the message
        /// Default: "" (nothing)
        /// </param>
        Task Log(TraceEventType severity, string message, string category = "");
        #endregion

        #region Unauthorized
        /// <summary>
        /// Logs an unauthorized access to the system
        /// </summary>
        /// <remarks>This should be called on your authentication/authorization checks if they fail</remarks>
        /// <param name="message">The message to log</param>
        Task Unauthorized(string message);
        #endregion

        #region Timers
        /// <summary>
        /// Will log how long the enclosed action took (from TimerStart to TimerEnd)
        /// </summary>
        /// <remarks>Call this function at any place in your code where you want to capture helpful timing information</remarks>
        /// <param name="name">The name you are giving this timer</param>
        /// <returns>The object which keeps the timer (to be passed into TimerEnd)</returns>
        Task<TimerInformation> TimerStart(string name);
        /// <summary>
        /// Logs the actual time an operation lasted (in milliseconds)
        /// </summary>
        /// <remarks>This should be called after TimerStart</remarks>
        /// <param name="info">The object with the timer which was returned from TimerStart</param>
        Task TimerEnd(TimerInformation info);
        #endregion

        #region Custom
        /// <summary>
        /// Logs a generic message with the type set to the category so it can be dis/enabled
        /// </summary>
        /// <param name="severity">The severity level of the log request</param>
        /// <param name="type">The category of the log message which doubles as the type (which can be configured on/off)</param>
        /// <param name="message">The message to log</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        Task Custom(TraceEventType severity, string type, string message, object o = null);
        /// <summary>
        /// Logs a generic message with the type set to the category so it can be dis/enabled
        /// </summary>
        /// <remarks>
        /// There are 3 methods to choose from - each one you can specify your own LogInformation file.
        /// This file should inherit from CustomInformation1 and override virtual properties.
        /// You should register that implementation in your own project against CustomInformation1.
        /// To make things cleaner, you should also likely create an extension method instead of directly calling Custom1()
        /// </remarks>
        /// <param name="message">The message to log</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        Task Custom1(string message, object o = null);
        /// <summary>
        /// Logs a generic message with the type set to the category so it can be dis/enabled
        /// </summary>
        /// <remarks>
        /// There are 3 methods to choose from - each one you can specify your own LogInformation file.
        /// This file should inherit from CustomInformation2 and override virtual properties.
        /// You should register that implementation in your own project against CustomInformation2.
        /// To make things cleaner, you should also likely create an extension method instead of directly calling Custom2()
        /// </remarks>
        /// <param name="message">The message to log</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        Task Custom2(string message, object o = null);
        /// <summary>
        /// Logs a generic message with the type set to the category so it can be dis/enabled
        /// </summary>
        /// <remarks>
        /// There are 3 methods to choose from - each one you can specify your own LogInformation file.
        /// This file should inherit from CustomInformation3 and override virtual properties.
        /// You should register that implementation in your own project against CustomInformation3.
        /// To make things cleaner, you should also likely create an extension method instead of directly calling Custom3()
        /// </remarks>
        /// <param name="message">The message to log</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        Task Custom3(string message, object o = null);

        /// <summary>
        /// Will log a custom timer method with the type set to the category so it can be dis/enabled
        /// </summary>
        /// <param name="severity">The severity level of the log request</param>
        /// <param name="type">The category of the timer which doubles as the type (which can be configured on/off)</param>
        /// <param name="name">The name you are giving this timer</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        /// <returns>The object which keeps the timer (to be passed into TimerEnd)</returns>
        Task<CustomTimerInformation> CustomTimerStart(TraceEventType severity, string type, string name, object o = null);
        /// <summary>
        /// Will log a custom timer method with the type set to the category so it can be dis/enabled
        /// </summary>
        /// <remarks>
        /// There are 3 methods to choose from - each one you can specify your own LogInformation file.
        /// This file should inherit from CustomTimerInformation1 and override virtual properties.
        /// You should register that implementation in your own project against CustomTimerInformation1.
        /// To make things cleaner, you should also likely create an extension method instead of directly calling CustomTimer1Start()/CustomTimerEnd()
        /// </remarks>
        /// <param name="name">The name you are giving this timer</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        /// <returns>The object which keeps the timer (to be passed into TimerEnd)</returns>
        Task<CustomTimerInformation1> CustomTimer1Start(string name, object o = null);
        /// <summary>
        /// Will log a custom timer method with the type set to the category so it can be dis/enabled
        /// </summary>
        /// <remarks>
        /// There are 3 methods to choose from - each one you can specify your own LogInformation file.
        /// This file should inherit from CustomTimerInformation2 and override virtual properties.
        /// You should register that implementation in your own project against CustomTimerInformation2.
        /// To make things cleaner, you should also likely create an extension method instead of directly calling CustomTimer2Start()/CustomTimerEnd()
        /// </remarks>
        /// <param name="name">The name you are giving this timer</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        /// <returns>The object which keeps the timer (to be passed into TimerEnd)</returns>
        Task<CustomTimerInformation2> CustomTimer2Start(string name, object o = null);
        /// <summary>
        /// Will log a custom timer method with the type set to the category so it can be dis/enabled
        /// </summary>
        /// <remarks>
        /// There are 3 methods to choose from - each one you can specify your own LogInformation file.
        /// This file should inherit from CustomTimerInformation3 and override virtual properties.
        /// You should register that implementation in your own project against CustomTimerInformation3.
        /// To make things cleaner, you should also likely create an extension method instead of directly calling CustomTimer3Start()/CustomTimerEnd()
        /// </remarks>
        /// <param name="name">The name you are giving this timer</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        /// <returns>The object which keeps the timer (to be passed into TimerEnd)</returns>
        Task<CustomTimerInformation3> CustomTimer3Start(string name, object o = null);
        /// <summary>
        /// Logs the actual time a customer timer took (in milliseconds)
        /// </summary>
        /// <remarks>This should be called after CustomTimerStart</remarks>
        /// <param name="info">The object with the timer which was returned from CustomTimerStart</param>
        /// <param name="o">Any object to store additional user-supplied information</param>
        Task CustomTimerEnd(CustomTimerInformation info, object o = null);
        #endregion

        #region Attributes
        /// <summary>
        /// Will log an Attribute call
        /// </summary>
        /// <remarks>
        /// Call this method when first entering an attribute
        /// </remarks>
        /// <param name="attributeName">The name of the attribute</param>
        /// <param name="parameters">The parameters into the attribute</param>
        /// <returns>The object which keeps the api information (to be passed into ApiExit)</returns>
        Task<AttributeInformation> AttributeEntry(string attributeName, Dictionary<string, string> parameters);
        /// <summary>
        /// Logs the actual time the attribute took (in milliseconds), as well as the result
        /// </summary>
        /// <remarks>This should be called after AttributeEntry</remarks>
        /// <param name="info">The object with the api information which was returned from ApiEntry</param>
        Task AttributeExit(AttributeInformation info);
        #endregion

        #region Api
        /// <summary>
        /// Will log all configurable information about an API call
        /// </summary>
        /// <remarks>
        /// Call this method when first entering an API method
        /// The actual information that is collected (or if this even logs out) will be given by your implementation of ILoggerConfiguration/ApiInformation
        /// </remarks>
        /// <returns>The object which keeps the api information (to be passed into ApiExitAsync)</returns>
        Task<ApiInformation> ApiEntry();
        /// <summary>
        /// Logs the actual time API took (in milliseconds), as well as the result
        /// </summary>
        /// <remarks>This should be called after ApiEntry</remarks>
        /// <param name="info">The object with the api information which was returned from ApiEntry</param>
        /// <param name="statusCode">The http status code of the result</param>
        /// <param name="result">The full http result</param>
        Task ApiExit(ApiInformation info, int statusCode, string result);
        /// <summary>
        /// Logs the actual time API took (in milliseconds), as well as the result
        /// </summary>
        /// <remarks>This should be called after ApiEntry</remarks>
        /// <param name="info">The object with the api information which was returned from ApiEntry</param>
        /// <param name="response">The HttpResponse object</param>
        Task ApiExit(ApiInformation info, HttpResponse response);
        #endregion

        #region Database
        /// <summary>
        /// Will log all configurable information about a database call
        /// </summary>
        /// <remarks>
        /// Call this method right before making a call to a database
        /// The actual information that is collected (or if this even logs out) will be given by your implementation of ILoggerConfiguration/DatabaseInformation
        /// </remarks>
        /// <param name="cnnName">The "nice" name for which database/connection you are using (don't include sensitive information)</param>
        /// <param name="command">The command being executed</param>
        /// <param name="args">Any parameters for the command</param>
        /// <returns>The object which keeps the database information (to be passed into DatabaseExit)</returns>
        Task<DatabaseInformation> DatabaseEntry(string cnnName, string command, object args = null);
        /// <summary>
        /// Logs the actual time the database call took (in milliseconds)
        /// </summary>
        /// <remarks>This should be called after DatabaseEntry</remarks>
        /// <param name="info">The object with the database information which was returned from DatabaseEntry</param>
        Task DatabaseExit(DatabaseInformation info);
        #endregion

        #region Web Services
        /// <summary>
        /// Will log all configurable information about a web service call
        /// </summary>
        /// <remarks>
        /// Call this method right before making a web service call
        /// The actual information that is collected (or if this even logs out) will be given by your implementation of ILoggerConfiguration/WebServiceInformation
        /// </remarks>
        /// <param name="headers">The collection of HTTP headers that will have logger prefix added</param>
        /// <param name="route">The unique route you are hitting (without dynamic url replacement)</param>
        /// <param name="url">The endpoint that is being hit</param>
        /// <param name="method">
        /// The HTTP method:
        ///     GET
        ///     POST
        ///     PUT
        ///     DELETE
        ///     OTHER
        /// </param>
        /// <param name="data">Any additional data being sent in the body of the request</param>
        /// <returns>The object which keeps the web service information (to be passed into WebServiceExit)</returns>
        Task<WebServiceInformation> WebServiceEntry(HttpHeaders headers, string route, string url, string method, object data = null);
        /// <summary>
        /// Logs the actual time the web service call took (in milliseconds), and optionally the result
        /// </summary>
        /// <remarks>This should be called after WebServiceEntry</remarks>
        /// <param name="info">The object with the web service information which was returned from WebServiceEntry</param>
        /// <param name="statusCode">The http status code of the result</param>
        /// <param name="result">The full http result</param>
        Task WebServiceExit(WebServiceInformation info, int statusCode, string result);
        /// <summary>
        /// Logs the actual time the web service call took (in milliseconds), and optionally the result
        /// </summary>
        /// <remarks>This should be called after WebServiceEntry</remarks>
        /// <param name="info">The object with the web service information which was returned from WebServiceEntry</param>
        /// <param name="response">The result from the web service call</param>
        Task WebServiceExit(WebServiceInformation info, HttpResponseMessage response);
        #endregion

        #region Middleware
        /// <summary>
        /// Will log information about a middleware component
        /// </summary>
        /// <param name="name">The name of the middleware component</param>
        /// <returns>The object which keeps the middleware information (to be passed into MiddlewareExit)</returns>
        Task<MiddlewareInformation> MiddlewareEntry(string name);
        /// <summary>
        /// Logs the actual time the middleware took (in milliseconds)
        /// </summary>
        /// <param name="info">The object with the middleware information which was returned from MiddlewareEntry</param>
        Task MiddlewareExit(MiddlewareInformation info);
        #endregion

        #region Hidden Exception
        /// <summary>
        /// Will log all hidden exception information
        /// </summary>
        /// <remarks>
        /// Call this method whenever an exception has occurred that will be ignored
        /// The actual information that is collected (or if this even logs out) will be given by your implementation of ILoggerConfiguration/ExceptionInformation
        /// </remarks>
        /// <param name="ex">The exception that is being hidden</param>
        /// <param name="messageOnly">
        /// Optional. Default = true
        /// If true, this will only log the exception text and type
        /// If false, all relevant information will be logged (eg. Stack Trace)
        /// </param>
        Task HiddenException(Exception ex, bool messageOnly);
        /// <summary>
        /// Will log all hidden exception information
        /// </summary>
        /// <remarks>
        /// Call this method whenever an exception has occurred that will be ignored
        /// The actual information that is collected (or if this even logs out) will be given by your implementation of ILoggerConfiguration/ExceptionInformation
        /// </remarks>
        /// <param name="ex">The exception that is being hidden</param>
        /// <param name="title">The type of exception, or a "nice" name you are giving this</param>
        /// <param name="messageOnly">
        /// Optional. Default = true
        /// If true, this will only log the exception text and type
        /// If false, all relevant information will be logged (eg. Stack Trace)
        /// </param>
        Task HiddenException(Exception ex, string title = "", bool messageOnly = true);
        #endregion

        #region Exception
        /// <summary>
        /// Will log all exception information
        /// </summary>
        /// <remarks>
        /// Call this method whenever an exception has occurred that has stopped processing of the request
        /// The actual information that is collected (or if this even logs out) will be given by your implementation of ILoggerConfiguration/ExceptionInformation
        /// </remarks>
        /// <param name="e">The exception that was thrown</param>
        /// <param name="messageOnly">
        /// Optional. Default = true
        /// If true, this will only log the exception text and type
        /// If false, all relevant information will be logged (eg. Stack Trace)
        /// </param>
        /// <returns>An identifier so the user can contact customer service</returns>
        Task<Guid> Exception(Exception e, bool messageOnly);
        /// <summary>
        /// Will log all exception information
        /// </summary>
        /// <remarks>
        /// Call this method whenever an exception has occurred that has stopped processing of the request
        /// The actual information that is collected (or if this even logs out) will be given by your implementation of ILoggerConfiguration/ExceptionInformation
        /// </remarks>
        /// <param name="e">The exception that was thrown</param>
        /// <param name="title">The type of exception, or a "nice" name you are giving this</param>
        /// <param name="messageOnly">
        /// Optional. Default = true
        /// If true, this will only log the exception text and type
        /// If false, all relevant information will be logged (eg. Stack Trace)
        /// </param>
        /// <returns>An identifier so the user can contact customer service</returns>
        Task<Guid> Exception(Exception e, string title = "", bool messageOnly = false);
        #endregion
    }
}
