using System;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.RetryMethod.Models
{
    public class RetryOptions
    {
        /// <summary>
        /// Starting/First delay after failure
        /// </summary>
        /// <remarks>Default = 100</remarks>
        public int FirstDelayMilliseconds { get; set; } = 100;

        /// <summary>
        /// How much the next delay will be. Function arguments: 1) Previous Delay (ms) - note this will be 0 on the first failure; 2) Retry count
        /// </summary>
        /// <remarks>
        /// Default = Function (GrowByFirstDelay): Each subsequent delay will grow by the First Delay (eg. 100, 200, 300, 400)
        /// Alternative Functions:
        /// SlowGrowth (each wait time will be longer by the starting amount (eg. 100, 200, 400, 700, 1100, 1600, 2200)
        /// Double (each wait time is doubled)
        /// Exponential (each wait time is the squared value of the previous value)
        /// Custom (write your own)
        /// </remarks>
        public Func<int, int, int, int> NextDelayMilliseconds { get; set; } = GrowByFirstDelay;

        /// <summary>
        /// How many failures before we stop retrying
        /// </summary>
        /// <remarks>Default = 20</remarks>
        public int MaxFailures { get; set; } = 20;

        /// <summary>
        /// If we have been waiting longer than this, we will stop retrying
        /// </summary>
        /// <remarks>Default = 0 (not set)</remarks>
        public int MaxWaitTime { get; set; } = 0;

        /// <summary>
        /// If true, will rethrow the exception from the last attempt.
        /// If false, will return the default value (or false if no return)
        /// </summary>
        /// <remarks>Default = true (rethrow)</remarks>
        public bool RethrowOnFailure { get; set; } = true;

        public static int GrowByFirstDelay(int previousDelay, int count, int firstDelayMilliseconds) => previousDelay + firstDelayMilliseconds;
        public static int SlowGrowth(int previousDelay, int count, int firstDelayMilliseconds) => previousDelay + firstDelayMilliseconds * count;
        public static int Double(int previousDelay, int count, int firstDelayMilliseconds) => count == 1 ? firstDelayMilliseconds : previousDelay * 2;
        public static int Exponential(int previousDelay, int count, int firstDelayMilliseconds) => count == 1 ? firstDelayMilliseconds : previousDelay * previousDelay;
    }
}
