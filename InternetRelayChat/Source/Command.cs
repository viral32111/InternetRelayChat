// https://www.alien.net.au/irc/irc2numerics.html

namespace viral32111.InternetRelayChat;

public static class Command {

	/// <summary>
	/// The command to close the connection to the server.
	/// </summary>
	/// <seealso href="https://www.rfc-editor.org/rfc/rfc1459#section-4.1.6" />
	public const string Quit = "QUIT";

	/// <summary>
	/// The command to set password during registration.
	/// </summary>
	/// <seealso href="https://www.rfc-editor.org/rfc/rfc1459#section-4.1.1" />
	public const string Password = "PASS";

	/// <summary>
	/// The command to set or change nickname.
	/// </summary>
	/// <seealso href="https://www.rfc-editor.org/rfc/rfc1459#section-4.1.2" />
	public const string Nick = "NICK";

	/// <summary>
	/// The command to set user names during registration.
	/// </summary>
	/// <seealso href="https://www.rfc-editor.org/rfc/rfc1459#section-4.1.3" />
	public const string User = "USER";

}
