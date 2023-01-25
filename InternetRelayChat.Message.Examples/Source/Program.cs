using System;
using System.Collections.Generic;

namespace viral32111.InternetRelayChat.Examples {

	public class Program {

		public static void Main( string[] arguments ) {
			Message ircMessage = Message.Parse( "@badge-info=;badges=moderator/1;color=;display-name=foo;emote-sets=0,300374282;mod=1;subscriber=0;user-type=mod :tmi.twitch.tv USERSTATE #bar" );
			Console.WriteLine( "Tags:" );
			foreach ( KeyValuePair<string, string?> tag in ircMessage.Tags ) Console.WriteLine( "  '{0}': '{1}'", tag.Key, tag.Value );
			Console.WriteLine( "Nick: '{0}'", ircMessage.Nick );
			Console.WriteLine( "User: '{0}'", ircMessage.User );
			Console.WriteLine( "Host: '{0}'", ircMessage.Host );
			Console.WriteLine( "Command: '{0}'", ircMessage.Command );
			Console.WriteLine( "Sub-Command: '{0}'", ircMessage.SubCommand );
			Console.WriteLine( "Middle: '{0}'", ircMessage.Middle );
			Console.WriteLine( "Parameters: '{0}'", ircMessage.Parameters );
		}

	}

}
