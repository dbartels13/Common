using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Models;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.EmailUtilities
{
    /// <inheritdoc />
    public class EmailMock : IEmail
    {
        public Task<bool> SendAsync(EmailType type, IEnumerable<string> to,
            IEnumerable<string> cc, string subject, string content)
            => Task.FromResult(true);
    }
}