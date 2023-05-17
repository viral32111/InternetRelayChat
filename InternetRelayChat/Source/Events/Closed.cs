using System;

namespace viral32111.InternetRelayChat;

/// <summary>
/// Event arguments for <see cref="Client.ClosedEvent">.
/// </summary>
public class ClosedEventArgs : EventArgs {

	/// <summary>
	/// The IP address of the IRC server.
	/// </summary>
	public required string RemoteAddress { get; init; }

	/// <summary>
	/// The TCP port number of the IRC server.
	/// </summary>
	public required int RemotePort { get; init; }

	/// <summary>
	/// The reason why the connection closed.
	/// </summary>
	/// <seealso cref="CloseReason" />
	public required CloseReason CloseReason { get; init; }

}

/// <summary>
/// The reasons why a connection could close.
/// </summary>
public enum CloseReason {

	/// <summary>
	/// The reason for the connection closing is unknown.
	/// </summary>
	Unknown = 0,

	/// <summary>
	/// The connection was closed by the client.
	/// </summary>
	ByClient = 1,

	/// <summary>
	/// The connection was closed by the IRC server.
	/// </summary>
	ByServer = 2,

	/// <summary>
	/// The connection was closed due to an error.
	/// </summary>
	ByError = 3

}
