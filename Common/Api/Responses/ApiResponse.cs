﻿using System;
using System.Collections.Generic;
using System.Net;
using Sphyrnidae.Common.Extensions;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Api.Responses
{
    /// <summary>
    /// Helper methods for creating a response to an Api call
    /// </summary>
    public static class ApiResponse
    {
        #region Success
        /// <summary>
        /// HTTP 2xx response
        /// </summary>
        /// <param name="code">Default OK(200) : You can optionally specify your own status code</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Success(HttpStatusCode code = HttpStatusCode.OK)
            => new ApiResponseStandard { Code = (int)code };

        /// <summary>
        /// HTTP 2xx response
        /// </summary>
        /// <param name="response">The response object to include in the body</param>
        /// <param name="code">Default OK (200): You can optionally specify your own status code</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Success(object response, HttpStatusCode code = HttpStatusCode.OK)
            => new ApiResponseStandard { Code = (int)code, Body = response };
        #endregion

        #region Generic Error Response
        /// <summary>
        /// Any HTTP response code with an error
        /// </summary>
        /// <param name="code">The HTTP Status Code</param>
        /// <param name="message">The response object to include in the body</param>
        /// <param name="error">The error that occurred</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Error(HttpStatusCode code, string message, object error = null)
        {
            var response = Success(message, code);
            response.Error = error;
            return response;
        }
        #endregion

        #region Login Responses
        /// <summary>
        /// HTTP 401 response for Login failed
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard LoginFailed()
            => Error(HttpStatusCode.Unauthorized, "Invalid credentials. Please try again.");

        /// <summary>
        /// HTTP 405 response for Login method not allowed
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard LoginMethodInvalid()
            => Error(HttpStatusCode.MethodNotAllowed, "The requested login is not activated for the user");

        /// <summary>
        /// HTTP 429 response for Login invalid due to account locked
        /// </summary>
        /// <param name="minutes">How long is the account locked for</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard TooManyFailedLogins(int minutes)
            => Error(
                HttpStatusCode.TooManyRequests,
                $"Your account has been locked because there were too many successive failed login attempts. Please try again in {minutes} minutes.");

        /// <summary>
        /// HTTP 423 response for Login invalid due to account locked
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard AccountLocked()
            => Error(
                HttpStatusCode.Locked,
                "Your account has been locked because there were too many successive failed login attempts.");
        #endregion

        #region HTTP 500 (Server Error)
        /// <summary>
        /// HTTP 500 response
        /// </summary>
        /// <param name="ex"> The exception that was thrown and will be formatted into the response</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard InternalServerError(Exception ex) => InternalServerError(ex.GetFullMessage());

        /// <summary>
        /// HTTP 500 response
        /// </summary>
        /// <param name="message"> Optional: The message to include in the response</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard InternalServerError(string message = null)
            => Error(
                HttpStatusCode.InternalServerError,
                message ?? "We're sorry, but something went wrong with your request.");

        /// <summary>
        /// HTTP 500 response
        /// </summary>
        /// <param name="guid"> A guid which references the real error which will be formatted into the response</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard InternalServerError(Guid guid)
            => Error(
                HttpStatusCode.InternalServerError,
                $"We're sorry, but something went wrong with your request. The details of this issue have been captured - please reference issue #{guid} if you need to contact the help desk.");
        #endregion

        #region HTTP 401 (Not Authenticated)
        /// <summary>
        /// HTTP 401 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard NotAuthenticated()
            => Error(HttpStatusCode.Unauthorized, "Your session has expired or is invalid. Please re-authenticate.");
        #endregion

        #region HTTP 403 (Forbidden, Not Authorized)
        /// <summary>
        /// HTTP 403 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard NotAuthorized()
            => Error(HttpStatusCode.Forbidden, "You do not have permission to perform this operation.");
        #endregion

        #region HTTP 400 (Bad Request)
        /// <summary>
        /// HTTP 400 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard BadRequest()
            => Error(HttpStatusCode.BadRequest, "The parameters to the request were invalid");

        /// <summary>
        /// HTTP 400 response
        /// </summary>
        /// <param name="parameter"> The parameter that is required</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Required(string parameter)
            => Error(HttpStatusCode.BadRequest, $"{parameter} is required");

        /// <summary>
        /// HTTP 400 response
        /// </summary>
        /// <param name="parameter"> The parameter that is required to not be default</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard NotDefault(string parameter)
            => Error(HttpStatusCode.BadRequest, $"{parameter} must not have the default value");

        /// <summary>
        /// HTTP 400 response
        /// </summary>
        /// <param name="parameter"> The parameter that is invalid</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Invalid(string parameter)
            => Error(HttpStatusCode.BadRequest, $"{parameter} must have a valid value");

        /// <summary>
        /// HTTP 400 response
        /// </summary>
        /// <param name="errors"> The service which will translate the code into the proper error messages</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard ModelStateErrors(List<string> errors)
        {
            var message = string.Join("\r\n", errors);
            return Error(HttpStatusCode.BadRequest, message);
        }
        #endregion

        #region HTTP 404 (Not Found)
        /// <summary>
        /// HTTP 404 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard NotFound()
            => Error(HttpStatusCode.NotFound, "The requested resource could not be found");

        /// <summary>
        /// HTTP 404 response
        /// </summary>
        /// <param name="type"> The type of resource being requested</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard NotFound(string type)
            => Error(HttpStatusCode.NotFound, $"Could not locate {type}");

        /// <summary>
        /// HTTP 404 response
        /// </summary>
        /// <param name="type"> The type of resource being requested</param>
        /// <param name="id"> The ID of that resource</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard NotFound(string type, string id)
            => Error(HttpStatusCode.NotFound, $"Could not locate {type} with ID {id}");
        #endregion

        #region HTTP 409 (Duplicate/Conflict)
        /// <summary>
        /// HTTP 409 response(Eg.Post/Create when item already exists)
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Duplicate()
            => Error(HttpStatusCode.Conflict, "Could not create resource because it already exists");

        /// <summary>
        /// HTTP 409 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Conflict()
            => Error(
                HttpStatusCode.Conflict,
                "Could not create resource because it would conflict with other resources");

        /// <summary>
        /// HTTP 409 response
        /// </summary>
        /// <param name="resource"> The type of thing in conflict</param>
        /// <param name="reason"> What the conflict is</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Conflict(string resource, string reason)
            => Error(
                HttpStatusCode.Conflict,
                $"Could not create {resource} because it would conflict with others",
                reason);

        /// <summary>
        /// HTTP 409 response
        /// </summary>
        /// <param name="id"> The ID of the actual latest object</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard NotLatest(string id)
            => Error(
                HttpStatusCode.Conflict,
                "Unable to update resource because you are not using the latest. The ID of the latest is identified in the Error property",
                id);
        #endregion

        #region HTTP 410 (Gone)
        /// <summary>
        /// HTTP 410 response(Gone)
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Gone()
            => Error(HttpStatusCode.Gone, "The requested resource could not be located");

        /// <summary>
        /// HTTP 410 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard AlreadyDeleted()
            => Error(HttpStatusCode.Gone, "Could not delete item because it does not exist");

        /// <summary>
        /// HTTP 410 response
        /// </summary>
        /// <param name="name"> The name of the item that has expired</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Expired(string name)
            => Error(HttpStatusCode.Gone, $"{name} has expired");
        #endregion

        #region HTTP 415 (Unsupported Media)
        /// <summary>
        /// HTTP 415 response(eg.Uploaded file invalid)
        /// </summary>
        /// <param name="mediaType"> The type of media that is invalid</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard InvalidMedia(string mediaType)
            => Error(HttpStatusCode.UnsupportedMediaType, $"{mediaType} is not supported");
        #endregion

        #region HTTP 422 (Validation Failed)
        /// <summary>
        /// HTTP 422 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard ValidationFailed()
            => Error(HttpStatusCode.UnprocessableEntity, "The requested resource failed validation");

        /// <summary>
        /// HTTP 422 response
        /// </summary>
        /// <param name="fields"> Collection of fields that failed validation</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard ValidationFailed(List<string> fields)
            => Error(
                HttpStatusCode.UnprocessableEntity,
                "One or more resources failed validation. The Error property contains the fields with validation errors",
                fields);
        #endregion

        #region HTTP 423 (Locked)
        /// <summary>
        /// HTTP 423 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Locked()
            => Error(HttpStatusCode.Locked, "The requested resource is locked");

        /// <summary>
        /// HTTP 423 response
        /// </summary>
        /// <param name="name"> Name of the object that is closed</param>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard Closed(string name)
            => Error(HttpStatusCode.Locked, $"{name} is closed");
        #endregion

        #region HTTP 497 (HTTP instead of HTTPS)
        /// <summary>
        /// HTTP 497 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard HttpsRequired()
            => Error((HttpStatusCode)497, "HTTPS is required");
        #endregion

        #region HTTP 503 (Service unavailable)
        /// <summary>
        /// HTTP 503 response
        /// </summary>
        /// <returns>The ApiResponseStandard object</returns>
        public static ApiResponseStandard EmailFailure()
            => Error(HttpStatusCode.ServiceUnavailable, "Email failed to send");
        #endregion
    }
}
