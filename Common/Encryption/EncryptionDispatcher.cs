using System;
using System.Linq;
using Sphyrnidae.Common.Encryption.Algorithms;
using Sphyrnidae.Common.Encryption.Models;
using Sphyrnidae.Common.Extensions;

namespace Sphyrnidae.Common.Encryption
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
        protected IEncryptionAlgorithms Algorithms { get; }
        public EncryptionDispatcher(IEncryptionAlgorithms algorithms) => Algorithms = algorithms;

        protected EncryptionAlgorithm CurrentAlgorithm
            => Algorithms.Current ?? throw new Exception("You did not register a current encryption algorithm");

        protected EncryptionAlgorithm FindAlgorithm(string str) =>
            Algorithms.All.FirstOrDefault(x =>
                !string.IsNullOrEmpty(x.Id)
                && str.StartsWith(x.Id, StringComparison.CurrentCulture))
            ?? Algorithms.Void
            ?? throw new Exception("Unable to locate matching encryption algorithm");

        protected static string AddIdentifier(EncryptionAlgorithm algorithm, string str) =>
            string.IsNullOrEmpty(algorithm.Id) ? str : $"{algorithm.Id}{str}";

        protected static string RemoveIdentifier(EncryptionAlgorithm algorithm, string str) =>
            string.IsNullOrEmpty(algorithm.Id) ? str : str.Remove(0, algorithm.Id.Length);

        public byte[] Hash(string str, string salt)
        {
            // Do the hash
            var hash = CurrentAlgorithm.Hash(str, salt);

            // Add on the ID
            if (string.IsNullOrEmpty(CurrentAlgorithm.Id))
                return hash;

            var id = CurrentAlgorithm.Id.ToBytes();
            return id.Concat(hash).ToArray();
        }

        public bool HashMatch(string str, string salt, byte[] hash)
        {
            foreach (var algorithm in Algorithms.All)
            {
                // Check to see if the hash starts with the id (is this algorithm)
                var id = algorithm.Id.ToBytes();
                if (id.Length > hash.Length)
                    continue;
                if (!hash.Take(id.Length).SequenceEqual(id))
                    continue;

                // Get the new hash
                var hashed = algorithm.Hash(str, salt);
                var compare = hash.Skip(id.Length);
                return hashed.SequenceEqual(compare);
            }

            // Not found, so using the void algorithm
            return Algorithms.Void.IsPopulated() && Algorithms.Void.Hash(str, salt).SequenceEqual(hash);
        }

        public string Encrypt(string str)
        {
            // Null/Empty strings do not encrypt, so just return string.empty
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            // Do the encryption
            var encrypted = CurrentAlgorithm.Encrypt(str);

            // Add on the ID
            return AddIdentifier(CurrentAlgorithm, encrypted);
        }

        public DecryptionResponse Decrypt(string str)
        {
            // Null/Empty string do not encrypt, so just return it back
            if (string.IsNullOrEmpty(str))
                return new DecryptionResponse { IsCurrent = true, Value = str };

            // Find the algorithm
            var algorithm = FindAlgorithm(str);

            // Remove the ID
            var algorithmStr = RemoveIdentifier(algorithm, str);

            // Do the decryption
            var response = algorithm.Decrypt(algorithmStr);
            response.IsCurrent = response.IsCurrent && algorithm.Id == CurrentAlgorithm.Id;
            return response;
        }
    }
}
