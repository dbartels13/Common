﻿using System;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.SignalR
{
    /// <summary>
    /// Handles communication with a SignalR Hub
    /// </summary>
    public static class SignalRHub
    {
        #region Send Message
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method) => await signalR.Send(url, method);
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <param name="arg1">The 1st argument (Optional)</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method, object arg1) => await signalR.Send(url, method, arg1);
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <param name="arg1">The 1st argument (Optional)</param>
        /// <param name="arg2">The 2nd argument (Optional)</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method, object arg1, object arg2) => await signalR.Send(url, method, arg1, arg2);
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <param name="arg1">The 1st argument (Optional)</param>
        /// <param name="arg2">The 2nd argument (Optional)</param>
        /// <param name="arg3">The 3rd argument (Optional)</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method, object arg1, object arg2, object arg3) => await signalR.Send(url, method, arg1, arg2, arg3);
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <param name="arg1">The 1st argument (Optional)</param>
        /// <param name="arg2">The 2nd argument (Optional)</param>
        /// <param name="arg3">The 3rd argument (Optional)</param>
        /// <param name="arg4">The 4th argument (Optional)</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method, object arg1, object arg2, object arg3, object arg4) => await signalR.Send(url, method, arg1, arg2, arg3, arg4);
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <param name="arg1">The 1st argument (Optional)</param>
        /// <param name="arg2">The 2nd argument (Optional)</param>
        /// <param name="arg3">The 3rd argument (Optional)</param>
        /// <param name="arg4">The 4th argument (Optional)</param>
        /// <param name="arg5">The 5th argument (Optional)</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5) => await signalR.Send(url, method, arg1, arg2, arg3, arg4, arg5);
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <param name="arg1">The 1st argument (Optional)</param>
        /// <param name="arg2">The 2nd argument (Optional)</param>
        /// <param name="arg3">The 3rd argument (Optional)</param>
        /// <param name="arg4">The 4th argument (Optional)</param>
        /// <param name="arg5">The 5th argument (Optional)</param>
        /// <param name="arg6">The 6th argument (Optional)</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6) => await signalR.Send(url, method, arg1, arg2, arg3, arg4, arg5, arg6);
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <param name="arg1">The 1st argument (Optional)</param>
        /// <param name="arg2">The 2nd argument (Optional)</param>
        /// <param name="arg3">The 3rd argument (Optional)</param>
        /// <param name="arg4">The 4th argument (Optional)</param>
        /// <param name="arg5">The 5th argument (Optional)</param>
        /// <param name="arg6">The 6th argument (Optional)</param>
        /// <param name="arg7">The 7th argument (Optional)</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7) => await signalR.Send(url, method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        /// <summary>
        /// Sends a message to the SignalR Hub
        /// </summary>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="method">The name of the method inside the hub to invoke</param>
        /// <param name="arg1">The 1st argument (Optional)</param>
        /// <param name="arg2">The 2nd argument (Optional)</param>
        /// <param name="arg3">The 3rd argument (Optional)</param>
        /// <param name="arg4">The 4th argument (Optional)</param>
        /// <param name="arg5">The 5th argument (Optional)</param>
        /// <param name="arg6">The 6th argument (Optional)</param>
        /// <param name="arg7">The 7th argument (Optional)</param>
        /// <param name="arg8">The 8th argument (Optional)</param>
        /// <returns>True if the message was sent asynchronously (no guarantee of delivery)</returns>
        public static async Task<bool> Send(ISignalR signalR, string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8) => await signalR.Send(url, method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        #endregion

        #region Receive
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive(ISignalR signalR, string url, string context, Action method) => signalR.Receive(url, context, method);
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <typeparam name="T">The type of 1st parameter</typeparam>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive<T>(ISignalR signalR, string url, string context, Action<T> method) => signalR.Receive(url, context, method);
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <typeparam name="T1">The type of 1st parameter</typeparam>
        /// <typeparam name="T2">The type of 2nd parameter</typeparam>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive<T1, T2>(ISignalR signalR, string url, string context, Action<T1, T2> method) => signalR.Receive(url, context, method);
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <typeparam name="T1">The type of 1st parameter</typeparam>
        /// <typeparam name="T2">The type of 2nd parameter</typeparam>
        /// <typeparam name="T3">The type of 3rd parameter</typeparam>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive<T1, T2, T3>(ISignalR signalR, string url, string context, Action<T1, T2, T3> method) => signalR.Receive(url, context, method);
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <typeparam name="T1">The type of 1st parameter</typeparam>
        /// <typeparam name="T2">The type of 2nd parameter</typeparam>
        /// <typeparam name="T3">The type of 3rd parameter</typeparam>
        /// <typeparam name="T4">The type of 4th parameter</typeparam>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive<T1, T2, T3, T4>(ISignalR signalR, string url, string context, Action<T1, T2, T3, T4> method) => signalR.Receive(url, context, method);
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <typeparam name="T1">The type of 1st parameter</typeparam>
        /// <typeparam name="T2">The type of 2nd parameter</typeparam>
        /// <typeparam name="T3">The type of 3rd parameter</typeparam>
        /// <typeparam name="T4">The type of 4th parameter</typeparam>
        /// <typeparam name="T5">The type of 5th parameter</typeparam>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive<T1, T2, T3, T4, T5>(ISignalR signalR, string url, string context, Action<T1, T2, T3, T4, T5> method) => signalR.Receive(url, context, method);
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <typeparam name="T1">The type of 1st parameter</typeparam>
        /// <typeparam name="T2">The type of 2nd parameter</typeparam>
        /// <typeparam name="T3">The type of 3rd parameter</typeparam>
        /// <typeparam name="T4">The type of 4th parameter</typeparam>
        /// <typeparam name="T5">The type of 5th parameter</typeparam>
        /// <typeparam name="T6">The type of 6th parameter</typeparam>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive<T1, T2, T3, T4, T5, T6>(ISignalR signalR, string url, string context, Action<T1, T2, T3, T4, T5, T6> method) => signalR.Receive(url, context, method);
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <typeparam name="T1">The type of 1st parameter</typeparam>
        /// <typeparam name="T2">The type of 2nd parameter</typeparam>
        /// <typeparam name="T3">The type of 3rd parameter</typeparam>
        /// <typeparam name="T4">The type of 4th parameter</typeparam>
        /// <typeparam name="T5">The type of 5th parameter</typeparam>
        /// <typeparam name="T6">The type of 6th parameter</typeparam>
        /// <typeparam name="T7">The type of 7th parameter</typeparam>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive<T1, T2, T3, T4, T5, T6, T7>(ISignalR signalR, string url, string context, Action<T1, T2, T3, T4, T5, T6, T7> method) => signalR.Receive(url, context, method);
        /// <summary>
        /// Receives a message
        /// </summary>
        /// <remarks>
        /// This should be called in the application start file if you want it to live for the entire scope of the application
        /// </remarks>
        /// <typeparam name="T1">The type of 1st parameter</typeparam>
        /// <typeparam name="T2">The type of 2nd parameter</typeparam>
        /// <typeparam name="T3">The type of 3rd parameter</typeparam>
        /// <typeparam name="T4">The type of 4th parameter</typeparam>
        /// <typeparam name="T5">The type of 5th parameter</typeparam>
        /// <typeparam name="T6">The type of 6th parameter</typeparam>
        /// <typeparam name="T7">The type of 7th parameter</typeparam>
        /// <typeparam name="T8">The type of 8th parameter</typeparam>
        /// <param name="signalR">The instance of the ISignalR interface</param>
        /// <param name="url">The name of the url to the hub</param>
        /// <param name="context">The name of the context that was invoked</param>
        /// <param name="method">A function to execute upon being called</param>
        public static Task Receive<T1, T2, T3, T4, T5, T6, T7, T8>(ISignalR signalR, string url, string context, Action<T1, T2, T3, T4, T5, T6, T7, T8> method) => signalR.Receive(url, context, method);
        #endregion
    }
}
