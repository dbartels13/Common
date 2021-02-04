using Sphyrnidae.Common.Dal;
using Sphyrnidae.Common.EncryptionImplementations;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Logging.Interfaces;

namespace Sphyrnidae.Common.Repos
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