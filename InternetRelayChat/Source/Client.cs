// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
// https://medium.com/@a.lyskawa/the-hitchhiker-guide-to-asynchronous-events-in-c-e9840109fb53, https://stackoverflow.com/a/724375

using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace viral32111.InternetRelayChat;

/// <summary>
/// Bare-bones IRC client.
/// </summary>
/// <seealso href="https://www.rfc-editor.org/rfc/rfc1459" />
public class Client {

	// Internal state
	protected readonly TcpClient tcpClient;
	protected NetworkStream? networkStream = null;
	protected SslStream? secureStream = null;
	protected CancellationTokenSource cancellationTokenSource = new();
	protected Task? receiveInBackgroundTask = null;
	protected readonly ConcurrentQueue<Message> messageQueue = new();
	protected readonly SemaphoreSlim messageAvailable = new(0);

	/// <summary>
	/// Event that fires after the client connection to the IRC server begins.
	/// </summary>
	/// <seealso cref="OpenedEventArgs" />
	public event OpenedEventHandler? OpenedEvent;
	public delegate void OpenedEventHandler(object sender, OpenedEventArgs e);

	/// <summary>
	/// Event that fires after the connection to the IRC server finishes.
	/// </summary>
	/// <seealso cref="ClosedEventArgs" />
	public event ClosedEventHandler? ClosedEvent;
	public delegate void ClosedEventHandler(object sender, ClosedEventArgs e);

	/// <summary>
	/// Event that fires after the client negotiates a secure connection with the IRC server.
	/// </summary>
	/// <seealso cref="SecuredEventArgs" />
	public event SecuredEventHandler? SecuredEvent;
	public delegate void SecuredEventHandler(object sender, SecuredEventArgs e);

	/// <summary>
	/// Event that fires after the client receives a message from the IRC server.
	/// </summary>
	/// <seealso cref="MessagedEventArgs" />
	public event MessagedEventHandler? MessagedEvent;
	public delegate void MessagedEventHandler(object sender, MessagedEventArgs e);

	// Logging
	private readonly ILogger logger;
	public Client(AddressFamily addressFamily = AddressFamily.InterNetwork, ILogger? logger = null)
		=> (tcpClient, this.logger) = (new(addressFamily), logger ?? new LoggerFactory().CreateLogger<Client>());

	/// <summary>
	/// Checks if the client is connected to an IRC server.
	/// </summary>
	/// <returns>Whether or not the client is connected to an IRC server.</returns>
	public bool IsConnected() => tcpClient.Connected;

	/// <summary>
	/// Checks if the connection to the IRC server is secured using TLS.
	/// </summary>
	/// <returns>Whether or not the connection to the IRC server is secured using TLS.</returns>
	public bool IsSecure() => secureStream != null && secureStream.IsEncrypted && secureStream.IsAuthenticated && secureStream.IsMutuallyAuthenticated;

	/// <summary>
	/// Connects to an IRC server.
	/// </summary>
	/// <param name="remoteHost">The host name or IP address of the IRC server.</param>
	/// <param name="remotePort">The TCP port number of the IRC server.</param>
	/// <param name="beSecure">Whether or not to secure the connection using TLS.</param>
	/// <param name="cancellationToken">The token for cancelling asynchronous operations.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">
	/// Thrown when <paramref name="localCertificates"/> is specified but <paramref name="beSecure"/> is not true.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the client is already connected to an IRC server.
	/// </exception>
	/// <exception cref="AuthenticationException">
	/// Thrown when the client fails to securely authenticate the IRC server.
	/// </exception>
	public async Task OpenAsync(string remoteHost, int remotePort = Constants.InsecurePort, bool? beSecure = null, X509CertificateCollection? localCertificates = null, CancellationToken? cancellationToken = null) {
		bool shouldSecure = beSecure == true || remotePort == Constants.SecurePort;

		if (localCertificates != null && !shouldSecure) throw new ArgumentException("Local certificates require secure connections", nameof(localCertificates));
		if (localCertificates != null && localCertificates.Count == 0) throw new ArgumentException("Local certificates collection is empty", nameof(localCertificates));

		if (IsConnected()) throw new InvalidOperationException("Client already connected");

		logger.LogDebug($"Opening TCP connection to '{remoteHost}' on port {remotePort}...");
		await tcpClient.ConnectAsync(remoteHost, remotePort, cancellationToken ?? cancellationTokenSource.Token);
		networkStream = tcpClient.GetStream();

		// Should never be null
		IPEndPoint remoteEndPoint = ( IPEndPoint? ) tcpClient.Client.RemoteEndPoint ?? throw new InvalidOperationException("Remote end-point is null");
		string ipAddress = remoteEndPoint.Address.ToString();
		int portNumber = remoteEndPoint.Port;
		logger.LogDebug($"Remote address is {ipAddress}:{portNumber}.");

		if (shouldSecure) {
			if (secureStream != null) throw new InvalidOperationException("Secure network stream already exists");

			logger.LogDebug("Opening secure network stream...");
			secureStream = new(networkStream, false);

			logger.LogDebug($"Authenticating as client to server '{secureStream.TargetHostName}'...");
			await secureStream.AuthenticateAsClientAsync(new SslClientAuthenticationOptions() {
				TargetHost = remoteHost,
				EnabledSslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12,
				EncryptionPolicy = EncryptionPolicy.RequireEncryption,
				ClientCertificates = localCertificates,
				CertificateRevocationCheckMode = X509RevocationMode.Online,
				RemoteCertificateValidationCallback = (_, certificate, chain, policyErrors) => {
					if (certificate == null || chain == null) return false;
					if (policyErrors != SslPolicyErrors.None) return false;
					return true;
				}
			}, cancellationToken ?? cancellationTokenSource.Token);

			// Should never be null to the validation callback
			X509Certificate remoteCertificate = secureStream.RemoteCertificate ?? throw new InvalidCastException("No remote certificate");
			logger.LogDebug($"Remote certificate is for '{remoteCertificate.Subject}'.");

			logger.LogTrace("Invoking secured event...");
			SecuredEvent?.Invoke(this, new() {
				RemoteName = secureStream.TargetHostName,
				RemoteAddress = ipAddress,
				RemotePort = portNumber,

				Protocol = secureStream.SslProtocol,

				CipherAlgorithm = secureStream.CipherAlgorithm,
				CipherStrength = secureStream.CipherStrength,

				HashAlgorithm = secureStream.HashAlgorithm,
				HashStrength = secureStream.HashStrength,

				KeyExchangeAlgorithm = secureStream.KeyExchangeAlgorithm,
				KeyExchangeStrength = secureStream.KeyExchangeStrength,

				LocalCertificate = secureStream.LocalCertificate,
				RemoteCertificate = remoteCertificate,

				IsEncrypted = secureStream.IsEncrypted,
				IsSigned = secureStream.IsSigned,
				IsAuthenticated = secureStream.IsAuthenticated,
				IsMutuallyAuthenticated = secureStream.IsMutuallyAuthenticated
			});
		}

		logger.LogDebug("Creating receive messages in background task...");
		receiveInBackgroundTask = ReceiveInBackgroundAsync(cancellationToken ?? cancellationTokenSource.Token);

		logger.LogTrace("Invoking opened (ready) event...");
		OpenedEvent?.Invoke(this, new() {
			RemoteName = remoteHost,
			RemoteAddress = ipAddress,
			RemotePort = portNumber
		});
	}

	/// <summary>
	/// Disconnects from the IRC server.
	/// </summary>
	/// <param name="cancellationToken">The token for cancelling asynchronous operations.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the client is not connected to an IRC server.
	/// </exception>
	public async Task CloseAsync(CloseReason closeReason = CloseReason.ByClient, CancellationToken? cancellationToken = null) {
		if (!IsConnected()) throw new InvalidOperationException("Client not connected");

		// Should never be null
		IPEndPoint remoteEndPoint = ( IPEndPoint? ) tcpClient.Client.RemoteEndPoint ?? throw new InvalidOperationException("Remote end-point is null");

		await SendAsync(new(Command.Quit), cancellationToken ?? cancellationTokenSource.Token);

		if (secureStream != null) {
			logger.LogDebug($"Closing secure network stream with '{secureStream.TargetHostName}'...");
			secureStream.Close();
			secureStream = null;
		}

		logger.LogDebug("Propagating cancellation signal...");
		cancellationTokenSource.Cancel();

		logger.LogDebug("Closing TCP connection...");
		tcpClient.Close();

		//await WaitAsync();

		logger.LogTrace("Invoking closed event...");
		ClosedEvent?.Invoke(this, new() {
			RemoteAddress = remoteEndPoint.Address.ToString(),
			RemotePort = remoteEndPoint.Port,

			CloseReason = closeReason
		});
	}

	/// <summary>
	/// Sends a message to the IRC server.
	/// </summary>
	/// <param name="message">The message to send.</param>
	/// <param name="cancellationToken">The token for cancelling asynchronous operations.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the client is not connected to an IRC server.
	/// </exception>
	public async Task SendAsync(Message message, CancellationToken? cancellationToken = null) {
		if (!IsConnected()) throw new InvalidOperationException("Client not connected");

		// Console.WriteLine($"Sending message '{message}'...");
		logger.LogDebug($"Sending message '{message}'...");

		byte[] messageBytes = message.GetBytes();

		if (secureStream != null) {
			await secureStream.WriteAsync(messageBytes, cancellationToken ?? cancellationTokenSource.Token);
			await secureStream.FlushAsync(cancellationToken ?? cancellationTokenSource.Token);
		} else if (networkStream != null) {
			await networkStream.WriteAsync(messageBytes, cancellationToken ?? cancellationTokenSource.Token);
			await networkStream.FlushAsync(cancellationToken ?? cancellationTokenSource.Token);
		} else throw new InvalidOperationException("No network stream");
	}

	/// <summary>
	/// Sends a message to the IRC server & waits for a response.
	/// </summary>
	/// <param name="message">The message to send.</param>
	/// <param name="cancellationToken">The token for cancelling asynchronous operations.</param>
	/// <returns>The response messages from the IRC server.</returns>
	public async Task<Message[]> SendWaitResponseAsync(Message message, CancellationToken? cancellationToken = null, TimeSpan? timeout = null) {
		await SendAsync(message, cancellationToken ?? cancellationTokenSource.Token);

		// Wait for the next message(s) from the background receiver
		using var _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken ?? cancellationTokenSource.Token);
		_cancellationTokenSource.CancelAfter(timeout ?? TimeSpan.FromSeconds(10));

		try {
			// Wait for at least one message
			await messageAvailable.WaitAsync(_cancellationTokenSource.Token);

			// Collect all available messages (there might be multiple)
			var messages = new System.Collections.Generic.List<Message>();
			while (messageQueue.TryDequeue(out var msg)) messages.Add(msg);
			return messages.ToArray();
		} catch (OperationCanceledException) {
			throw new TimeoutException($"Timed out while waiting for response to '{message.Command}'");
		}
	}

	/// <summary>
	/// Receives messages from the IRC server.
	/// </summary>
	/// <param name="bufferSize">The size of the buffer to receive packets into.</param>
	/// <param name="cancellationToken">The token for cancelling asynchronous operations.</param>
	/// <returns>An array of IRC messages.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when the client is not connected to an IRC server.
	/// </exception>
	public async Task<Message[]> ReceiveAsync(int bufferSize = Constants.ReceiveBufferSize, Encoding? encoding = null, CancellationToken? cancellationToken = null) {
		if (!IsConnected()) throw new InvalidOperationException("Client not connected");

		int receivedByteCount;
		byte[] receivedBytes = new byte[bufferSize];

		if (secureStream != null) {
			receivedByteCount = await secureStream.ReadAsync(receivedBytes, cancellationToken ?? cancellationTokenSource.Token);
		} else if (networkStream != null) {
			receivedByteCount = await networkStream.ReadAsync(receivedBytes, cancellationToken ?? cancellationTokenSource.Token);
		} else throw new InvalidOperationException("No network stream");

		if (receivedByteCount == 0) {
			logger.LogDebug("No more bytes to receive! Closing connection...");
			await CloseAsync(CloseReason.ByServer, cancellationToken ?? cancellationTokenSource.Token);
		}

		string receivedString = (encoding ?? Encoding.UTF8).GetString(receivedBytes, 0, receivedByteCount);
		logger.LogDebug($"Received {receivedByteCount}/{bufferSize} bytes: '{receivedString.TrimEnd('\r', '\n')}'.");

		return Message.ParseMany(receivedString);
	}

	/// <summary>
	/// Waits for the receive in background task to complete.
	/// Silently fails if the task does not exist (i.e., client not connected).
	/// </summary>
	public async Task WaitAsync() {
		if (receiveInBackgroundTask == null) return;

		try {
			logger.LogDebug("Waiting for receive in background task to complete...");
			await receiveInBackgroundTask;
			logger.LogDebug("Receive in background task completed.");
		} catch (IOException exception) {
			logger.LogWarning($"I/O exception while waiting for receive in background task! ({exception.Message})");
		}
	}

	// Receives messages from the IRC server in the background.
	private async Task ReceiveInBackgroundAsync(CancellationToken cancellationToken) {
		if (!IsConnected()) throw new InvalidOperationException("Client not connected");

		logger.LogDebug("Started receiving messages...");
		while (IsConnected() && !cancellationToken.IsCancellationRequested) {
			Message[] messages = await ReceiveAsync(cancellationToken: cancellationToken);
			logger.LogDebug($"Received {messages.Length} message(s).");

			if (messages.Length == 0) break;

			// For SendWaitResponseAsync()
			foreach (Message message in messages) messageQueue.Enqueue(message);
			if (messages.Length > 0) messageAvailable.Release();

			logger.LogTrace("Invoking messaged event(s)...");
			foreach (Message message in messages) {
				MessagedEvent?.Invoke(this, new() {
					Message = message
				});
			}
		}

		logger.LogDebug("Finished receiving messages.");
	}

}
