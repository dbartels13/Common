using Sphyrnidae.Common.Dal;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Logging.Interfaces;
using Sphyrnidae.Settings.Repos.Interfaces;
using System.Threading.Tasks;

namespace Sphyrnidae.Settings.Repos
{
    public class DefaultUserRepo : SqlServerRepo, IDefaultUserRepo
    {
        protected override string CnnName => "Default User";

        private static string _cnnStr;
        public override string CnnStr => _cnnStr ??= SettingsEnvironmental.Get(Env, "Cnn:Authentication").Decrypt(Encrypt).Value;

        protected IEnvironmentSettings Env { get; }
        protected IEncryption Encrypt { get; }
        public DefaultUserRepo(ILogger logger, IEnvironmentSettings env, IEncryption encrypt) : base(logger)
        {
            Env = env;
            Encrypt = encrypt;
        }

        public async Task<SphyrnidaeIdentity> GetDefaultUser()
        {
            return await GetSPAsync<SphyrnidaeIdentity>("Users_GetDefault", null);
        }
    }
}
