using System;
using System.Collections.Generic;
using System.Linq;
using Sphyrnidae.Common.Cache;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations.Models;
using Sphyrnidae.Common.Environment;

namespace Sphyrnidae.Common.EncryptionImplementations.KeyManager
{
    /// <inheritdoc />
    public class EncryptionKeyManager : IEncryptionKeyManager
    {
        protected ICache Cache { get; }
        protected IEnvironmentSettings Env { get; }

        public EncryptionKeyManager(ICache cache, IEnvironmentSettings env)
        {
            Cache = cache;
            Env = env;
        }

        private List<EncryptionKey> _keys;

        /// <summary>
        /// Keys will be cached for the default time (20 minutes).
        /// They are currently hard-coded
        /// </summary>
        protected List<EncryptionKey> AllKeys => _keys ??= Caching.Get(Cache, "EncryptionKeys", () =>
            new List<EncryptionKey>
                {
                    new EncryptionKey
                    {
                        Id = "",
                        IsCurrent = false,
                        Key = SettingsEnvironmental.Get(Env, "Encryption_Key_Old")
                    },
                    new EncryptionKey
                    {
                        Id = "24a776b0-c967-4780-8010-95d2b90e456c",
                        IsCurrent = true,
                        Key = SettingsEnvironmental.Get(Env, "Encryption_Key")
                    }
                });

        private EncryptionKey _voidKey;
        protected EncryptionKey VoidKey => _voidKey ??= AllKeys.FirstOrDefault(x => string.IsNullOrEmpty(x.Id));

        private EncryptionKey _currentKey;
        public EncryptionKey CurrentKey => _currentKey ??= AllKeys.First(x => x.IsCurrent); // There must always be 1, so let this throw

        public FoundEncryptionKey GetKeyFromString(string encrypted)
        {
            var key = AllKeys.FirstOrDefault(x => !string.IsNullOrEmpty(x.Id) && encrypted.StartsWith(x.Id, StringComparison.CurrentCulture))
                ?? VoidKey
                ?? throw new Exception("Unable to locate matching encryption key");

            return new FoundEncryptionKey(key, encrypted);
        }
    }
}
