using System;

namespace viral32111.InternetRelayChat;

/// <summary>
/// Event arguments for <see cref="Client.MessagedEvent">.
/// </summary>
public class MessagedEventArgs : EventArgs {

	/// <summary>
	/// The message from the IRC server.
	/// </summary>
	public required Message Message { get; init; }

}
