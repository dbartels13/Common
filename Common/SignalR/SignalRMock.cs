using System;
using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.SignalR
{
    /// <inheritdoc />
    public class SignalRMock : ISignalR
    {
        public Task<bool> Send(string url, string method) => Task.FromResult(true);
        public Task<bool> Send(string url, string method, object arg1) => Task.FromResult(true);
        public Task<bool> Send(string url, string method, object arg1, object arg2) => Task.FromResult(true);
        public Task<bool> Send(string url, string method, object arg1, object arg2, object arg3) => Task.FromResult(true);
        public Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4) => Task.FromResult(true);
        public Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5) => Task.FromResult(true);
        public Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6) => Task.FromResult(true);
        public Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7) => Task.FromResult(true);
        public Task<bool> Send(string url, string method, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8) => Task.FromResult(true);

        public Task Receive(string url, string context, Action method) => Task.CompletedTask;
        public Task Receive<T>(string url, string context, Action<T> method) => Task.CompletedTask;
        public Task Receive<T1, T2>(string url, string context, Action<T1, T2> method) => Task.CompletedTask;
        public Task Receive<T1, T2, T3>(string url, string context, Action<T1, T2, T3> method) => Task.CompletedTask;
        public Task Receive<T1, T2, T3, T4>(string url, string context, Action<T1, T2, T3, T4> method) => Task.CompletedTask;
        public Task Receive<T1, T2, T3, T4, T5>(string url, string context, Action<T1, T2, T3, T4, T5> method) => Task.CompletedTask;
        public Task Receive<T1, T2, T3, T4, T5, T6>(string url, string context, Action<T1, T2, T3, T4, T5, T6> method) => Task.CompletedTask;
        public Task Receive<T1, T2, T3, T4, T5, T6, T7>(string url, string context, Action<T1, T2, T3, T4, T5, T6, T7> method) => Task.CompletedTask;
        public Task Receive<T1, T2, T3, T4, T5, T6, T7, T8>(string url, string context, Action<T1, T2, T3, T4, T5, T6, T7, T8> method) => Task.CompletedTask;
    }
}
