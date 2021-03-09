# Encryption {#EncryptionMd}

## Overview {#EncryptionOverviewMd}
Encryption turns a string with valuable information (eg. passwords, credit card numbers, personal information, etc) into an encrypted value.
This encrypted value will be meaningless to the hacker unless they have a key to decrypt it, and the correct algorithm to do so.
The [IEncryption](@ref Sphyrnidae.Common.Encryption.IEncryption) interface defines the methods needed for encryption:
1. Hash(): Given a string and some hash (string), the implementation will perform 1-way encryption (this CAN NOT be decrypted)
2. HashMatch(): Allows you to re-hash a string and compare it to a stored hash value (eg. password validation)
3. Encrypt(): Encrypts a string (2-way)
4. Decrypt(): Decrypts a string

The implementation [EncryptionDispatcher](@ref Sphyrnidae.Common.Encryption.EncryptionDispatcher) makes everything configurable:
1. [Key lookup](@ref EncryptionKeysMd): Interface driven capability to retrieve the encryption key
2. [Encryption Algorithms](@ref EncryptionAlgorithmsMd): Allows you to have multiple algorithms that do all the work

Interface: [IEncryption](@ref Sphyrnidae.Common.Encryption.IEncryption)

Mock: [EncryptionNone](@ref Sphyrnidae.Common.Encryption.EncryptionNone)

Implementations:
1. [EncryptionDispatcher](@ref Sphyrnidae.Common.Encryption.EncryptionDispatcher): Preferred / Default
2. [EncryptionStrong](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionStrong): A stronger encryption algorithm, but takes longer to execute
3. [EncryptionWeak](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionWeak): A weaker encryption algorithm, but is fast to execute
4. [EncryptionNormal](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionNormal): Somewhere inbetween the weak and strong algorithms
5. [EncryptionOld](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionOld): My first attempt at encryption. Works fine

Wrapper: [EncryptionExtensions](@ref Sphyrnidae.Common.Encryption.EncryptionExtensions) (string extension methods)

## Dispatcher {#EncryptionDispatcherMd}
The preferred implementation of [IEncryption](@ref Sphyrnidae.Common.Encryption.IEncryption) is the [EncryptionDispatcher](@ref Sphyrnidae.Common.Encryption.EncryptionDispatcher).
How this works is that any encryption attempt will use the default key, which will also be linked to a unique identifier.
It will also use the default algorithm, which also has a unique identifier.
These identifiers will be a part of the final encryption string so that the decryption attempt knows which key and algorithm to use.

This implementation utilizes 2 additional interfaces for it's functionality:
1. [IEncryptionKeyManager](@ref Sphyrnidae.Common.Encryption.KeyManager.IEncryptionKeyManager): See [Key lookup](@ref EncryptionKeysMd)
2. [IEncryptionAlgorithms](@ref Sphyrnidae.Common.Encryption.Algorithms.IEncryptionAlgorithms): See [Encryption Algorithms](@ref EncryptionAlgorithmsMd)

## Key Lookup {#EncryptionKeysMd}
An encryption key is a secret key (usually a string, Base64 string, or byte[]) that you must possess to encrypt or decrypt.
Without this key, an encrypted value will be impossible to decrypt.
Because of the importance of this key, it must be kept in a secure location such as:
1. Key Vault Store: Cloud-based service which stores all of your keys securely behind your application firewall. See <a href="https://azure.microsoft.com/en-us/services/key-vault/" target="blank">Azure</a> or <a href="https://aws.amazon.com/kms/" target="blank">AWS</a>
2. Environmental Variable: As long as your application is behind a firewall, a hacker should not be able to get into your application and steal this. If they are able to, you might have larger/other things to worry about.
3. Source code: In older applications, this was stored directly in web.config. This is generally considered a bad practice, but if you can trust your source code repository, you may still wish to hard-code in these keys directly into your application files.
4. Others?

Which one you choose is completely up to you and your security needs.
The end result is that you should have at least 1 key (possibly more), and have an invalidation scheme for these keys.

[EncryptionDispatcher](@ref Sphyrnidae.Common.Encryption.EncryptionDispatcher) will perform lookup of encryption keys via the interface [IEncryptionKeyManager](@ref Sphyrnidae.Common.Encryption.KeyManager.IEncryptionKeyManager).

## Encryption Algorithms {#EncryptionAlgorithmsMd}
As with encryption keys, you may wish to have a dynamic set of encryption algorithms in case one of them gets compromised.
These algorithms will ultimately inherit from [IEncryption](@ref Sphyrnidae.Common.Encryption.IEncryption),
so you could directly register these as an [IEncryption](@ref Sphyrnidae.Common.Encryption.IEncryption) implementation.
However, it would be better to keep multiple algorithms on-hand and swich as necessary.
This will also allow you to decrypt older strings that may have been encrypted using a previous method.

The [EncryptionDispatcher](@ref Sphyrnidae.Common.Encryption.EncryptionDispatcher) does this utilizing the [IEncryptionAlgorithms](@ref Sphyrnidae.Common.Encryption.Algorithms.IEncryptionAlgorithms) interface.
You essentially specify all the possible [algorithms](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionAlgorithm),
which one is current, and you're off and running.

The default implementation of the [IEncryptionAlgorithms](@ref Sphyrnidae.Common.Encryption.Algorithms.IEncryptionAlgorithms) interface is [SphyrnidaeEncryptionAlgorithms](@ref Sphyrnidae.Common.Encryption.Algorithms.SphyrnidaeEncryptionAlgorithms).
This registers the following algorithms:
1. [EncryptionWeak](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionWeak): Default algorithm
2. [EncryptionNormal](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionNormal)
3. [EncryptionStrong](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionStrong)

If no key is found, the 'void' algorithm is [EncryptionOld](@ref Sphyrnidae.Common.Encryption.Algorithms.EncryptionOld).
You should likely implement your own set of algorithms.

## Where Used {#EncryptionWhereUsedMd}
1. [AuthenticationMiddleware](@ref Sphyrnidae.Common.Api.Middleware.AuthenticationMiddleware): Parsing the JWT
2. [JwtMiddleware](@ref Sphyrnidae.Common.Api.Middleware.JwtMiddleware): Encrypting the JWT to send out in the HTTP response
3. [IdentityHelper](@ref Sphyrnidae.Common.Authentication.IdentityHelper): Working with the JWT
4. [IdentityWrapper](@ref Sphyrnidae.Common.Authentication.IdentityWrapper): Getting or setting the Identity object
5. [DotNetEmail](@ref Sphyrnidae.Common.EmailUtilities.DotNetEmail): Decrypting the [Password](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IDotNetEmailSettings.Password)
6. [EncryptionExtensions](@ref Sphyrnidae.Common.Encryption.EncryptionExtensions): Extension method you should use when doing any encryption activities
7. [LogRepo](@ref Sphyrnidae.Common.Repos.LogRepo): Decrypting the connection string for the logging repository
8. [WebServiceBase](@ref Sphyrnidae.Common.WebServices.WebServiceBase): The JWT will be included in all web service requests

## Examples {#EncryptionExampleMd}
<pre>
	//
	// Registering your own algorithm(s)
	//

	// Create the algorithm (Note this is not using the key mangaer)
    public class MyEncryptionAlgorithm : EncryptionAlgorithm
    {
        private IEncryptionKeyManager KeyManager { get; set; }
        public MyEncryptionAlgorithm(IEncryptionKeyManager keyManager) => KeyManager = keyManager;

        public override string Id => "my unique algorithm id";
        public override DecryptionResponse Decrypt(string str) => new DecryptionResponse { IsCurrent = true, Value = "decrypted value" };
        public override string Encrypt(string str) => "Encrypted Value";
        public override byte[] Hash(string str, string salt) => new byte[0];
    }

    // Create registration class
    public class MyEncryptionAlgorithms : SphyrnidaeEncryptionAlgorithms
    {
        public MyEncryptionAlgorithms(IEncryptionKeyManager manager) : base(manager) { }

        public override List<EncryptionAlgorithm> All => new List<EncryptionAlgorithm> { Current };
        public override EncryptionAlgorithm Current => new MyEncryptionAlgorithm(Manager);
        public override EncryptionAlgorithm Void => Current;
    }

    // Update service registration (startup.cs)
    services.Transient<IEncryptionAlgorithms, MyEncryptionAlgorithms>();


    //
    // Custom Key Manager
    //

    // Create the key manager
    public class MyKeyManager : IEncryptionKeyManager
    {
        // Will want to look these up from a key vault
        public EncryptionKey CurrentKey => new EncryptionKey { Id = "my key identifier", Key = "my encryption key", IsCurrent = true };
        public FoundEncryptionKey GetKeyFromString(string encrypted)
        {
            if (!encrypted.StartsWith(CurrentKey.Id)) // May look for other keys in the key vault that match
                throw new Exception("Unable to locate matching encryption key");
            return new FoundEncryptionKey(CurrentKey, encrypted);
        }
    }

    // Update service registration (startup.cs)
    services.Transient<IEncryptionKeyManager, MyKeyManager>();


    //
    // Performing Encryption
    //
    IEncryption encryption; // Should be injected
    var myVal = "string to encrypt";
    var encrypted = myVal.Encrypt(encryption);
    var decrypted = encrypted.Decrypt(encryption);
    if (string.IsNullOrWhiteSpace(decrypted.Value))
        throw new Exception("Unable to decrypt");
    if (!decrypted.IsCurrent)
        var updatedEncryption = decrypted.Value.Encrypt(encryption); // Store this back into your repository with latest method
    return decrypted.Value;
</pre>