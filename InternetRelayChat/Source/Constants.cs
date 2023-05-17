namespace viral32111.InternetRelayChat;

/// <summary>
/// Constants used by the package.
/// </summary>
public static class Constants {

	/// <summary>
	/// The default port number for insecure IRC connections.
	/// </summary>
	/// <seealso href="https://www.rfc-editor.org/rfc/rfc7194" />
	/// <seealso href="https://dev.twitch.tv/docs/irc/#connecting-to-the-twitch-irc-server" />
	public const int InsecurePort = 6667;

	/// <summary>
	/// The default port number for secure IRC connections.
	/// </summary>
	/// <seealso href="https://www.rfc-editor.org/rfc/rfc7194" />
	/// <seealso href="https://dev.twitch.tv/docs/irc/#connecting-to-the-twitch-irc-server" />
	public const int SecurePort = 6697;

	/// <summary>
	/// The default buffer size for receiving TCP packets.
	/// </summary>
	public const int ReceiveBufferSize = 4096;

}
