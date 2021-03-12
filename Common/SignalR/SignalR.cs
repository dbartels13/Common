using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Sphyrnidae.Common.Extensions;

namespace Sphyrnidae.Common.SignalR
{
    /// <inheritdoc />
    public class SignalR : ISignalR
    {
        #region Logging Attempt
        // Avoiding circular dependency
        private static Action<string> _strLogger;
        private static Action<Exception> _exLogger;

        protected void LogStr(string message)
        {
            if (_strLogger.IsDefault())
                return;
            _strLogger(message);
        }
        protected void LogEx(Exception ex)
        {
            if (_exLogger.IsDefault())
                return;
            _exLogger(ex);
        }

        public static void SetLogger(Action<string> strLogger, Action<Exception> exLogger)
        {
            _strLogger = strLogger;
            _exLogger = exLogger;
        }
        #endregion

        #region Send
        public async Task<bool> Send(string url, string method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method);
            return success;
        }

        public async Task<bool> Send(string url, string method, object arg1)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method, arg1);
            return success;
        }

        public async Task<bool> Send(string url, string method, object arg1, object arg2)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method, arg1, arg2);
            return success;
        }

        public async Task<bool> Send(string url, string method, object arg1, object arg2, object arg3)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method, arg1, arg2, arg3);
            return success;
        }

        public async Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method, arg1, arg2, arg3, arg4);
            return success;
        }

        public async Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method, arg1, arg2, arg3, arg4, arg5);
            return success;
        }

        public async Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method, arg1, arg2, arg3, arg4, arg5, arg6);
            return success;
        }

        public async Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return success;
        }

        public async Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return false;
            var success = await Start(cnn);
            if (success)
                await cnn.InvokeAsync(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return success;
        }
        #endregion

        #region Receive
        public Task Receive(string url, string context, Action method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }

        public Task Receive<T>(string url, string context, Action<T> method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }

        public Task Receive<T1, T2>(string url, string context, Action<T1, T2> method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }

        public Task Receive<T1, T2, T3>(string url, string context, Action<T1, T2, T3> method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }

        public Task Receive<T1, T2, T3, T4>(string url, string context, Action<T1, T2, T3, T4> method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }

        public Task Receive<T1, T2, T3, T4, T5>(string url, string context, Action<T1, T2, T3, T4, T5> method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }

        public Task Receive<T1, T2, T3, T4, T5, T6>(string url, string context, Action<T1, T2, T3, T4, T5, T6> method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }

        public Task Receive<T1, T2, T3, T4, T5, T6, T7>(string url, string context, Action<T1, T2, T3, T4, T5, T6, T7> method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }

        public Task Receive<T1, T2, T3, T4, T5, T6, T7, T8>(string url, string context, Action<T1, T2, T3, T4, T5, T6, T7, T8> method)
        {
            var cnn = GetConnection(url);
            if (cnn.IsDefault())
                return Task.CompletedTask;
            cnn.On(context, method);
            return Start(cnn);
        }
        #endregion

        #region Helpers
        private HubConnection GetConnection(string url)
        {
            // ReSharper disable once InvertIf
            if (url.IsDefault())
            {
                LogStr("SignalR does not have url configured.");
                return null;
            }

            return new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect(new[]
                {
                    TimeSpan.Zero,
                    TimeSpan.Zero,
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(4),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(6),
                    TimeSpan.FromSeconds(7),
                    TimeSpan.FromSeconds(8),
                    TimeSpan.FromSeconds(9),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(11),
                    TimeSpan.FromSeconds(12),
                    TimeSpan.FromSeconds(13),
                    TimeSpan.FromSeconds(14),
                    TimeSpan.FromSeconds(15),
                    TimeSpan.FromSeconds(16),
                    TimeSpan.FromSeconds(17),
                    TimeSpan.FromSeconds(18),
                    TimeSpan.FromSeconds(19)
                })
                .Build();
        }

        private async Task<bool> Start(HubConnection cnn)
        {
            if (cnn.IsDefault())
                return false;

            cnn.Closed += ex =>
            {
                LogStr("SignalR has stopped working.");
                LogEx(ex);
                return Task.CompletedTask;
            };
            for (var i = 0; i <= 20; i++)
            {
                try
                {
                    await cnn.StartAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    if (i == 20)
                    {
                        LogStr("SignalR was unable to connect.");
                        LogEx(ex);
                        return true;
                    }

                    await Task.Delay(1000 * i);
                }
            }

            return false;
        }
        #endregion
    }
}
