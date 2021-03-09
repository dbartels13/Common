using Sphyrnidae.Common.Dal;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Settings.Repos
{
    /// <inheritdoc />
    /// <summary>
    /// Base class for all repositories - gets connection string from the Environmental Setting "Cnn" (Encrypted)
    /// </summary>
    public abstract class SphyrnidaeRepo : SqlServerRepo
    {
        protected IEnvironmentSettings Env { get; }
        protected IEncryption Encrypt { get; }
        protected SphyrnidaeRepo(ILogger logger, IEnvironmentSettings env, IEncryption encrypt) : base(logger)
        {
            Env = env;
            Encrypt = encrypt;
        }

        private static string _cnnStr;
        public override string CnnStr => _cnnStr ??= SettingsEnvironmental.Get(Env, "Cnn:Main").Decrypt(Encrypt).Value;
    }
}