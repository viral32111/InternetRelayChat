using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace viral32111.InternetRelayChat;

/// <summary>
/// Event arguments for <see cref="Client.SecuredEvent">.
/// </summary>
public class SecuredEventArgs : OpenedEventArgs {

	/// <summary>
	/// The TLS protocol version.
	/// </summary>
	public required SslProtocols Protocol { get; init; }

	/// <summary>
	/// The cipher algorithm for encryption.
	/// </summary>
	/// <seealso cref="SecuredEventArgs.CipherStrength" />
	public required CipherAlgorithmType CipherAlgorithm { get; init; }

	/// <summary>
	/// The strength of the cipher algorithm, in bits.
	/// </summary>
	/// <seealso cref="SecuredEventArgs.CipherAlgorithm" />
	public required int CipherStrength { get; init; }

	/// <summary>
	/// The hash algorithm for message authentication.
	/// </summary>
	/// <seealso cref="SecuredEventArgs.HashStrength" />
	public required HashAlgorithmType HashAlgorithm { get; init; }

	/// <summary>
	/// The strength of the hash algorithm, in bits.
	/// </summary>
	/// <seealso cref="SecuredEventArgs.HashAlgorithm" />
	public required int HashStrength { get; init; }

	/// <summary>
	/// The key exchange algorithm.
	/// </summary>
	/// <seealso cref="SecuredEventArgs.KeyExchangeStrength" />
	public required ExchangeAlgorithmType KeyExchangeAlgorithm { get; init; }

	/// <summary>
	/// The strength of the key exchange algorithm, in bits.
	/// </summary>
	/// <seealso cref="SecuredEventArgs.KeyExchangeAlgorithm" />
	public required int KeyExchangeStrength { get; init; }

	/// <summary>
	/// The local X.509 certificate, if supplied.
	/// </summary>
	public required X509Certificate? LocalCertificate { get; init; }

	/// <summary>
	/// The remote X.509 certificate.
	/// </summary>
	public required X509Certificate RemoteCertificate { get; init; }

	/// <summary>
	/// Whether the connection is encrypted.
	/// </summary>
	public required bool IsEncrypted { get; init; }

	/// <summary>
	/// Whether the connection is signed.
	/// </summary>
	public required bool IsSigned { get; init; }

	/// <summary>
	/// Whether the connection is authenticated.
	/// </summary>
	public required bool IsAuthenticated { get; init; }

	/// <summary>
	/// Whether the connection is mutually authenticated.
	/// </summary>
	public required bool IsMutuallyAuthenticated { get; init; }

}
