using System;
using System.Linq;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations.Models;
using Sphyrnidae.Common.Extensions;

namespace Sphyrnidae.Common.EncryptionImplementations
{
    /// <inheritdoc />
    /// <summary>
    /// Encrypts or decrypts a string.
    /// </summary>
    /// <remarks>
    /// Note that this class doesn't actually perform encryption activities.
    /// It instead passes it off to the correct handler to do the encryption/decryption.
    /// </remarks>
    public class EncryptionDispatcher : IEncryption
    {
        protected IEncryptionImplementations Implementations { get; }
        public EncryptionDispatcher(IEncryptionImplementations implementations) => Implementations = implementations;

        protected EncryptionImplementation CurrentImplementation
            => Implementations.Current ?? throw new Exception("You did not register a current encryption implementation");

        protected EncryptionImplementation FindImplementation(string str) =>
            Implementations.All.FirstOrDefault(x =>
                !string.IsNullOrEmpty(x.Id)
                && str.StartsWith(x.Id, StringComparison.CurrentCulture))
            ?? Implementations.Void
            ?? throw new Exception("Unable to locate matching encryption implementation");

        protected static string AddIdentifier(EncryptionImplementation implementation, string str) =>
            string.IsNullOrEmpty(implementation.Id) ? str : $"{implementation.Id}{str}";

        protected static string RemoveIdentifier(EncryptionImplementation implementation, string str) =>
            string.IsNullOrEmpty(implementation.Id) ? str : str.Remove(0, implementation.Id.Length);

        public byte[] Hash(string str, string salt)
        {
            // Do the hash
            var hash = CurrentImplementation.Hash(str, salt);

            // Add on the ID
            if (string.IsNullOrEmpty(CurrentImplementation.Id))
                return hash;

            var id = CurrentImplementation.Id.ToBytes();
            return id.Concat(hash).ToArray();
        }

        public bool HashMatch(string str, string salt, byte[] hash)
        {
            foreach (var implementation in Implementations.All)
            {
                // Check to see if the hash starts with the id (is this implementation)
                var id = implementation.Id.ToBytes();
                if (id.Length > hash.Length)
                    continue;
                if (!hash.Take(id.Length).SequenceEqual(id))
                    continue;

                // Get the new hash
                var hashed = implementation.Hash(str, salt);
                var compare = hash.Skip(id.Length);
                return hashed.SequenceEqual(compare);
            }

            // Not found, so using the void implementation
            return Implementations.Void.IsPopulated() && Implementations.Void.Hash(str, salt).SequenceEqual(hash);
        }

        public string Encrypt(string str)
        {
            // Null/Empty strings do not encrypt, so just return string.empty
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            // Do the encryption
            var encrypted = CurrentImplementation.Encrypt(str);

            // Add on the ID
            return AddIdentifier(CurrentImplementation, encrypted);
        }

        public DecryptionResponse Decrypt(string str)
        {
            // Null/Empty string do not encrypt, so just return it back
            if (string.IsNullOrEmpty(str))
                return new DecryptionResponse { IsCurrent = true, Value = str };

            // Find the implementation
            var implementation = FindImplementation(str);

            // Remove the ID
            var implementationStr = RemoveIdentifier(implementation, str);

            // Do the decryption
            var response = implementation.Decrypt(implementationStr);
            response.IsCurrent = response.IsCurrent && implementation.Id == CurrentImplementation.Id;
            return response;
        }
    }
}
