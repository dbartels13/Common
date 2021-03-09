using System.Threading.Tasks;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Logging.Information;
using Sphyrnidae.Common.Logging.Loggers.Models;

namespace Sphyrnidae.Common.Logging.Loggers
{
    /// <summary>
    /// Base class to identify a class as a Logger
    /// </summary>
    public abstract class BaseLogger
    {
        /// <summary>
        /// Name of the logger
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// If 'identity' custom items for the logging type should be included
        /// </summary>
        public abstract bool IncludeIdentity { get; }
        /// <summary>
        /// If 'static' custom items for the logging type should be included
        /// </summary>
        public abstract bool IncludeStatic { get; }
        /// <summary>
        /// If 'high' custom items for the logging type should be included
        /// </summary>
        public abstract bool IncludeHigh { get; }
        /// <summary>
        /// If 'medium' custom items for the logging type should be included
        /// </summary>
        public abstract bool IncludeMed { get; }
        /// <summary>
        /// If 'low' custom items for the logging type should be included
        /// </summary>
        public abstract bool IncludeLow { get; }

        /// <summary>
        /// Called by the logger, this is when a record needs to be logged
        /// </summary>
        /// <remarks>Do not override this one - you should instead customize in DoInsert</remarks>
        /// <param name="info">The log information class</param>
        /// <param name="maxLength">When to truncate items</param>
        public virtual async Task Insert(BaseLogInformation info, int maxLength)
            => await DoInsert(ToInsertModel(info, maxLength), info, maxLength);
        /// <summary>
        /// Actual implementation of logging the record
        /// </summary>
        /// <param name="model">The model containing everything to be logged</param>
        /// <param name="info">The log information class</param>
        /// <param name="maxLength">When to truncate items</param>
        protected abstract Task DoInsert(LogInsert model, BaseLogInformation info, int maxLength);

        /// <summary>
        /// Called by the logger, this is when a logged record needs to be updated
        /// </summary>
        /// <remarks>Do not override this one - you should instead customize in DoInsert</remarks>
        /// <param name="info">The log information class</param>
        /// <param name="maxLength">When to truncate items</param>
        public virtual async Task Update(TimerBaseInformation info, int maxLength)
            => await DoUpdate(ToUpdateModel(info, maxLength), info, maxLength);
        /// <summary>
        /// Actual implementation of updating a logged record
        /// </summary>
        /// <param name="model">The model containing everything to be logged</param>
        /// <param name="info">The log information class</param>
        /// <param name="maxLength">When to truncate items</param>
        protected abstract Task DoUpdate(LogUpdate model, TimerBaseInformation info, int maxLength);

        protected virtual LogInsert ToInsertModel(BaseLogInformation info, int maxLength)
        {
            var model = new LogInsert
            {
                Type = info.Type,
                Identifier = info.Identifier,
                Timestamp = info.Timestamp,
                Severity = info.Severity,
                Order = info.Order,
                RequestId = info.RequestId,
                Session = info.Session,
                UserId = info.UserId ?? 0,
                Message = info.Message.ShortenWithEllipses(maxLength),
                Category = info.Category.ShortenWithEllipses(maxLength),
                Machine = info.Machine,
                Application = info.Application
            };

            if (IncludeIdentity)
                foreach (var (key, value) in info.IdentityProperties)
                    model.Other.Add(key, value.ShortenWithEllipses(maxLength));

            if (IncludeStatic)
                foreach (var (key, value) in info.StaticProperties)
                    model.Other.Add(key, value.ShortenWithEllipses(maxLength));

            if (IncludeHigh)
                foreach (var (key, value) in info.HighProperties)
                    model.Other.Add(key, value.ShortenWithEllipses(maxLength));

            if (IncludeMed)
                foreach (var (key, value) in info.MedProperties)
                    model.Other.Add(key, value.ShortenWithEllipses(maxLength));

            // ReSharper disable once InvertIf
            if (IncludeLow)
                foreach (var (key, value) in info.LowProperties)
                    model.Other.Add(key, value.ShortenWithEllipses(maxLength));

            return model;
        }

        protected virtual LogUpdate ToUpdateModel(TimerBaseInformation info, int maxLength)
        {
            var model = new LogUpdate
            {
                Identifier = info.Identifier,
                Elapsed = info.GetElapsed()
            };

            if (IncludeHigh)
                foreach (var (key, value) in info.HighProperties)
                    model.Other.Add(key, value.ShortenWithEllipses(maxLength));

            if (IncludeMed)
                foreach (var (key, value) in info.MedProperties)
                    model.Other.Add(key, value.ShortenWithEllipses(maxLength));

            // ReSharper disable once InvertIf
            if (IncludeLow)
                foreach (var (key, value) in info.LowProperties)
                    model.Other.Add(key, value.ShortenWithEllipses(maxLength));

            return model;
        }
    }
}
