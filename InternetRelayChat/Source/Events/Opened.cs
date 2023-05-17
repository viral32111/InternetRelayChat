using System;

namespace viral32111.InternetRelayChat;

/// <summary>
/// Event arguments for <see cref="Client.OpenedEvent">.
/// </summary>
public class OpenedEventArgs : EventArgs {

	/// <summary>
	/// The host name (or IP address) of the IRC server.
	/// </summary>
	public required string RemoteName { get; init; }

	/// <summary>
	/// The IP address of the IRC server.
	/// </summary>
	public required string RemoteAddress { get; init; }

	/// <summary>
	/// The TCP port number of the IRC server.
	/// </summary>
	public required int RemotePort { get; init; }

}
