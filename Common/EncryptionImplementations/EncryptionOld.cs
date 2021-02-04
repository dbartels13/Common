using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations.Models;
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo

namespace Sphyrnidae.Common.EncryptionImplementations
{
    /// <inheritdoc />
    /// <summary>
    /// Encrypts or decrypts a string.
    /// Note that this uses an encryption key stored in the environment
    /// </summary>
    public class EncryptionOld : EncryptionImplementation
    {
        protected IEncryptionKeyManager KeyManager { get; }
        public EncryptionOld(IEncryptionKeyManager keyManager)
        {
            KeyManager = keyManager;
        }

        #region Settings
        // Preconfigured Encryption Parameters
        protected const int BlockBitSize = 128;
        protected const int KeyBitSize = 256;
        protected const int HashBytes = 128;

        // Preconfigured Password Key Derivation Parameters
        protected const int SaltBitSize = 64;
        protected const int Iterations = 10;
        protected const int MinPasswordLength = 12;

        public override string Id => "";
        #endregion

        #region Hash (1-way encryption)
        public override byte[] Hash(string str, string salt)
        {
            var strBytes = Encoding.UTF8.GetBytes(str);
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            //var algorithm = new Rfc2898DeriveBytes(str, saltBytes) { IterationCount = Iterations };
            //return algorithm.GetBytes(HashBytes);
            return Pbkdf2Sha256GetBytes(strBytes, saltBytes, Iterations, HashBytes);
        }

        protected static byte[] Pbkdf2Sha256GetBytes(byte[] str, byte[] salt, int iterations, int outputBytes)
        {
            using var hmac = new HMACSHA256(str);
            var hashLength = hmac.HashSize / 8;
            if ((hmac.HashSize & 7) != 0)
                hashLength++;

            var keyLength = outputBytes / hashLength;
            if (outputBytes > (0xFFFFFFFFL * hashLength) || outputBytes < 0)
                throw new ArgumentOutOfRangeException(nameof(outputBytes));

            if (outputBytes % hashLength != 0)
                keyLength++;

            var extendedKey = new byte[salt.Length + 4];
            Buffer.BlockCopy(salt, 0, extendedKey, 0, salt.Length);
            using var ms = new MemoryStream();
            for (var i = 0; i < keyLength; i++)
            {
                extendedKey[salt.Length] = (byte)(((i + 1) >> 24) & 0xFF);
                extendedKey[salt.Length + 1] = (byte)(((i + 1) >> 16) & 0xFF);
                extendedKey[salt.Length + 2] = (byte)(((i + 1) >> 8) & 0xFF);
                extendedKey[salt.Length + 3] = (byte)(((i + 1)) & 0xFF);
                var u = hmac.ComputeHash(extendedKey);
                Array.Clear(extendedKey, salt.Length, 4);
                var f = u;
                for (var j = 1; j < iterations; j++)
                {
                    u = hmac.ComputeHash(u);
                    for (var k = 0; k < f.Length; k++)
                    {
                        f[k] ^= u[k];
                    }
                }

                ms.Write(f, 0, f.Length);
                Array.Clear(u, 0, u.Length);
                Array.Clear(f, 0, f.Length);
            }

            var dk = new byte[outputBytes];
            ms.Position = 0;
            ms.Read(dk, 0, outputBytes);
            ms.Position = 0;
            for (long i = 0; i < ms.Length; i++)
            {
                ms.WriteByte(0);
            }

            Array.Clear(extendedKey, 0, extendedKey.Length);
            return dk;
        }
        #endregion

        #region Encrypt
        /// <summary>
        /// Simple Encryption (AES) then Authentication (HMAC) of a UTF8 message
        /// using Keys derived from a Password (PBKDF2).
        /// </summary>
        /// <param name="str">The string to encrypt.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <exception cref="ArgumentException">password</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// Adds additional non secret payload for key generation parameters.
        /// </remarks>
        public override string Encrypt(string str)
        {
            // Parameter checking
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("Nothing to encrypt!", nameof(str));

            // Get the encryption password
            var key = KeyManager.CurrentKey;
            var password = key.Key;

            // Do the encryption
            var plainText = Encoding.UTF8.GetBytes(str);
            var cipherText = Encrypt(plainText, null, password);
            var encrypted = Convert.ToBase64String(cipherText);
            return key.Id + encrypted;
        }

        /// <summary>
        /// Simple Encryption (AES) then Authentication (HMAC) for a UTF8 Message.
        /// </summary>
        /// <param name="str">The secret message.</param>
        /// <param name="cryptKey">The crypt key.</param>
        /// <param name="authKey">The auth key.</param>
        /// <param name="nonSecretPayload">(Optional) Non-Secret Payload.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <exception cref="ArgumentException">Secret Message Required!;secretMessage</exception>
        /// <remarks>
        /// Adds overhead of (Optional-Payload + BlockSize(16) + Message-Padded-To-BlockSize +  HMac-Tag(32)) * 1.33 Base64
        /// </remarks>
        public static string Encrypt(string str, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
        {
            // Parameter checking
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("Nothing to encrypt!", nameof(str));

            // Do the encryption
            var plainText = Encoding.UTF8.GetBytes(str);
            var cipherText = Encrypt(plainText, cryptKey, authKey, nonSecretPayload);
            return Convert.ToBase64String(cipherText);
        }

        protected static byte[] Encrypt(byte[] secretMessage, byte[] nonSecretPayload, string password)
        {
            // Parameter checking
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                throw new ArgumentException($"Must have a password of at least {MinPasswordLength} characters!", nameof(password));

            if (secretMessage == null || secretMessage.Length == 0)
                throw new ArgumentException("Secret Message Required!", nameof(secretMessage));

            // Check for additional payload
            nonSecretPayload ??= new byte[] { };
            var payload = new byte[SaltBitSize / 8 * 2 + nonSecretPayload.Length];

            Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
            var payloadIndex = nonSecretPayload.Length;

            // Use Random Salt to prevent pre-generated weak password attacks.
            byte[] cryptKey;
            byte[] authKey;
            using (var generator = new Rfc2898DeriveBytes(password, SaltBitSize / 8, Iterations))
            {
                var salt = generator.Salt;

                // Generate Keys
                cryptKey = generator.GetBytes(KeyBitSize / 8);

                // Create Non Secret Payload
                Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
                payloadIndex += salt.Length;
            }

            // Deriving separate key, might be less efficient than using HKDF, 
            // but now compatible with RNEncryptor which had a very similar wireformat and requires less code than HKDF.
            using (var generator = new Rfc2898DeriveBytes(password, SaltBitSize / 8, Iterations))
            {
                var salt = generator.Salt;

                // Generate Keys
                authKey = generator.GetBytes(KeyBitSize / 8);

                // Create Rest of Non Secret Payload
                Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
            }

            return Encrypt(secretMessage, cryptKey, authKey, payload);
        }

        protected static byte[] Encrypt(byte[] secretMessage, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
        {
            // Parameter checking
            if (cryptKey == null || cryptKey.Length != KeyBitSize / 8)
                throw new ArgumentException($"Key needs to be {KeyBitSize} bit!", nameof(cryptKey));

            if (authKey == null || authKey.Length != KeyBitSize / 8)
                throw new ArgumentException($"Key needs to be {KeyBitSize} bit!", nameof(authKey));

            if (secretMessage == null || secretMessage.Length < 1)
                throw new ArgumentException("Secret Message Required!", nameof(secretMessage));

            // non-secret payload optional
            nonSecretPayload ??= new byte[] { };

            byte[] cipherText;
            byte[] iv;

            using (var aes = new AesManaged
            {
                KeySize = KeyBitSize,
                BlockSize = BlockBitSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            })
            {
                // Use random IV
                aes.GenerateIV();
                iv = aes.IV;

                using var encrypt = aes.CreateEncryptor(cryptKey, iv);
                using var cipherStream = new MemoryStream();
                using (var cryptoStream = new CryptoStream(cipherStream, encrypt, CryptoStreamMode.Write))
                {
                    using var binaryWriter = new BinaryWriter(cryptoStream);
                    //Encrypt Data
                    binaryWriter.Write(secretMessage);
                }

                cipherText = cipherStream.ToArray();
            }

            // Assemble encrypted message and add authentication
            using var hmac = new HMACSHA256(authKey);
            using var encryptedStream = new MemoryStream();
            using (var binaryWriter = new BinaryWriter(encryptedStream))
            {
                // Prepend non-secret payload if any
                binaryWriter.Write(nonSecretPayload);

                // Prepend IV
                binaryWriter.Write(iv);

                // Write Ciphertext
                binaryWriter.Write(cipherText);
                binaryWriter.Flush();

                // Authenticate all data
                var tag = hmac.ComputeHash(encryptedStream.ToArray());

                // Postpend tag
                binaryWriter.Write(tag);
            }
            return encryptedStream.ToArray();
        }
        #endregion

        #region Decrypt

        /// <summary>
        /// Simple Authentication (HMAC) and then Descryption (AES) of a UTF8 Message
        /// using keys derived from a password (PBKDF2). 
        /// </summary>
        /// <param name="str">The encrypted message.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// </remarks>
        public override DecryptionResponse Decrypt(string str) => Decrypt(str, 0);

        /// <summary>
        /// Simple Authentication (HMAC) and then Descryption (AES) of a UTF8 Message
        /// using keys derived from a password (PBKDF2). 
        /// </summary>
        /// <param name="str">The encrypted message.</param>
        /// <param name="nonSecretPayloadLength">Length of the non secret payload.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// </remarks>
        public virtual DecryptionResponse Decrypt(string str, int nonSecretPayloadLength)
        {
            // Parameter checking
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException("Nothing to decrypt!", nameof(str));

            // Get the encryption password
            var key = KeyManager.GetKeyFromString(str);
            var password = key.Key;

            // Do the decryption
            var cipherText = Convert.FromBase64String(key.Encrypted);
            var plainText = Decrypt(cipherText, password, nonSecretPayloadLength);
            return new DecryptionResponse
            {
                IsCurrent = key.IsCurrent,
                Value = plainText == null ? null : Encoding.UTF8.GetString(plainText)
            };
        }

        /// <summary>
        /// Simple Authentication (HMAC) then Decryption (AES) for a secrets UTF8 Message.
        /// </summary>
        /// <param name="str">The encrypted message.</param>
        /// <param name="cryptKey">The crypt key.</param>
        /// <param name="authKey">The auth key.</param>
        /// <param name="nonSecretPayloadLength">Length of the non secret payload.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
        public static string Decrypt(string str, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
        {
            // Parameter checking
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException("Nothing to decrypt!", nameof(str));

            // Do the decryption
            var cipherText = Convert.FromBase64String(str);
            var plainText = Decrypt(cipherText, cryptKey, authKey, nonSecretPayloadLength);
            return plainText == null ? null : Encoding.UTF8.GetString(plainText);
        }

        protected static byte[] Decrypt(byte[] encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            // Parameter checking
            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                throw new ArgumentException($"Must have a password of at least {MinPasswordLength} characters!", nameof(password));

            if (encryptedMessage == null || encryptedMessage.Length == 0)
                throw new ArgumentException("Encrypted Message Required!", nameof(encryptedMessage));

            var cryptSalt = new byte[SaltBitSize / 8];
            var authSalt = new byte[SaltBitSize / 8];

            // Grab Salt from Non-Secret Payload
            Array.Copy(encryptedMessage, nonSecretPayloadLength, cryptSalt, 0, cryptSalt.Length);
            Array.Copy(encryptedMessage, nonSecretPayloadLength + cryptSalt.Length, authSalt, 0, authSalt.Length);

            byte[] cryptKey;
            byte[] authKey;

            // Generate crypt key
            using (var generator = new Rfc2898DeriveBytes(password, cryptSalt, Iterations))
            {
                cryptKey = generator.GetBytes(KeyBitSize / 8);
            }

            // Generate auth key
            using (var generator = new Rfc2898DeriveBytes(password, authSalt, Iterations))
            {
                authKey = generator.GetBytes(KeyBitSize / 8);
            }

            return Decrypt(encryptedMessage, cryptKey, authKey, cryptSalt.Length + authSalt.Length + nonSecretPayloadLength);
        }

        public static byte[] Decrypt(byte[] encryptedMessage, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
        {
            // Parameter checking
            if (cryptKey == null || cryptKey.Length != KeyBitSize / 8)
                throw new ArgumentException($"CryptKey needs to be {KeyBitSize} bit!", nameof(cryptKey));

            if (authKey == null || authKey.Length != KeyBitSize / 8)
                throw new ArgumentException($"AuthKey needs to be {KeyBitSize} bit!", nameof(authKey));

            if (encryptedMessage == null || encryptedMessage.Length == 0)
                throw new ArgumentException("Encrypted Message Required!", nameof(encryptedMessage));

            using var hmac = new HMACSHA256(authKey);
            // Calculate Tags
            var sentTag = new byte[hmac.HashSize / 8];
            var calcTag = hmac.ComputeHash(encryptedMessage, 0, encryptedMessage.Length - sentTag.Length);
            const int ivLength = BlockBitSize / 8;

            // If message length is to small just return null
            if (encryptedMessage.Length < sentTag.Length + nonSecretPayloadLength + ivLength)
                return null;

            // Grab Sent Tag
            Array.Copy(encryptedMessage, encryptedMessage.Length - sentTag.Length, sentTag, 0, sentTag.Length);

            // Compare Tag with constant time comparison
            var compare = 0;
            for (var i = 0; i < sentTag.Length; i++)
                compare |= sentTag[i] ^ calcTag[i];

            // If message doesn't authenticate return null
            if (compare != 0)
                return null;

            using var aes = new AesManaged
            {
                KeySize = KeyBitSize,
                BlockSize = BlockBitSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            // Grab IV from message
            var iv = new byte[ivLength];
            Array.Copy(encryptedMessage, nonSecretPayloadLength, iv, 0, iv.Length);

            using var decrypt = aes.CreateDecryptor(cryptKey, iv);
            using var plainTextStream = new MemoryStream();
            using (var decryptStream = new CryptoStream(plainTextStream, decrypt, CryptoStreamMode.Write))
            {
                using var binaryWriter = new BinaryWriter(decryptStream);
                // Decrypt Cipher Text from Message
                binaryWriter.Write(
                    encryptedMessage,
                    nonSecretPayloadLength + iv.Length,
                    encryptedMessage.Length - nonSecretPayloadLength - iv.Length - sentTag.Length
                );
            }

            // Return Plain Text
            return plainTextStream.ToArray();
        }
        #endregion
    }
}
