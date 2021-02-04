using System;
using System.Threading.Tasks;
using Sphyrnidae.Common.RetryMethod.Models;

namespace Sphyrnidae.Common.RetryMethod
{
    internal class RetryRun
    {
        private RetryOptions Options { get; }
        private int NumAttempts { get; set; } = 1;
        private int WaitTime { get; set; }
        private int TotalWaitTime { get; set; }

        internal RetryRun(RetryOptions options) => Options = options ?? new RetryOptions();

        internal async Task<bool> RetryOnException(Exception ex)
        {
            if (Options.MaxFailures > 0 && NumAttempts >= Options.MaxFailures)
                return Done(ex);

            WaitTime = Options.NextDelayMilliseconds(WaitTime, NumAttempts, Options.FirstDelayMilliseconds);

            TotalWaitTime += WaitTime;
            if (Options.MaxWaitTime > 0 && TotalWaitTime >= Options.MaxWaitTime)
                return Done(ex);

            await Task.Delay(WaitTime);

            NumAttempts++;

            return true;
        }

        private bool Done(Exception ex)
        {
            if (Options.RethrowOnFailure)
                throw ex;
            return false;
        }
    }
}