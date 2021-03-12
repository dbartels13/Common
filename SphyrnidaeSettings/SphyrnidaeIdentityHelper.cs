using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.Authentication.Identity;
using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.Cache.Models;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Settings.Repos.Interfaces;
using System.Threading.Tasks;

namespace Sphyrnidae.Settings
{
    public class SphyrnidaeIdentityHelper : IdentityHelper<SphyrnidaeIdentity>
    {
        private ICache Cache { get; }
        private IDefaultUserRepo Repo { get; }
        public SphyrnidaeIdentityHelper(IHttpClientSettings http, IEncryption encryption, ICache cache, IDefaultUserRepo repo)
            : base(http, encryption)
        {
            Cache = cache;
            Cache.Options.Seconds = CacheOptions.Year;
            Repo = repo;
        }

        public override int ExpirationMinutes => 60;

        // Async/Await allow conversion from SphyrnidaeIdentity to BaseIdentity
        public override async Task<BaseIdentity> GetDefaultIdentity()
            => await Caching.GetAsync(Cache, "SphyrnidaeDefaultIdentity", async () => await Repo.GetDefaultUser());
    }
}
