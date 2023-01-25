using Xunit;

// https://www.alien.net.au/irc/irc2numerics.html

namespace viral32111.InternetRelayChat.Message.Tests {

	public class UnitTests_Responses {

		[Fact]
		public void Test_Welcome_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( ":tmi.twitch.tv 001 myusername :Welcome, GLHF!" );

			Assert.Equal( ircMessage.Host, "tmi.twitch.tv" );
			Assert.Equal( ircMessage.Command, "001" );
			Assert.Equal( ircMessage.Middle, "myusername" );
			Assert.Equal( ircMessage.Parameters, "Welcome, GHLF!" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_YourHost_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( ":tmi.twitch.tv 002 myusername :Your host is tmi.twitch.tv" );

			Assert.Equal( ircMessage.Host, "tmi.twitch.tv" );
			Assert.Equal( ircMessage.Command, "002" );
			Assert.Equal( ircMessage.Middle, "myusername" );
			Assert.Equal( ircMessage.Parameters, "Your host is tmi.twitch.tv" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_Created_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( ":tmi.twitch.tv 003 myusername :This server is rather new" );

			Assert.Equal( ircMessage.Host, "tmi.twitch.tv" );
			Assert.Equal( ircMessage.Command, "003" );
			Assert.Equal( ircMessage.Middle, "myusername" );
			Assert.Equal( ircMessage.Parameters, "This server is rather new" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_MyInfo_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( ":tmi.twitch.tv 004 myusername :-" );

			Assert.Equal( ircMessage.Host, "tmi.twitch.tv" );
			Assert.Equal( ircMessage.Command, "004" );
			Assert.Equal( ircMessage.Middle, "myusername" );
			Assert.Equal( ircMessage.Parameters, "-" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_MoTD_Start_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( ":tmi.twitch.tv 375 myusername :-" );

			Assert.Equal( ircMessage.Host, "tmi.twitch.tv" );
			Assert.Equal( ircMessage.Command, "375" );
			Assert.Equal( ircMessage.Middle, "myusername" );
			Assert.Equal( ircMessage.Parameters, "-" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_MoTD_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( ":tmi.twitch.tv 372 myusername :You are in a maze of twisty passages." );

			Assert.Equal( ircMessage.Host, "tmi.twitch.tv" );
			Assert.Equal( ircMessage.Command, "372" );
			Assert.Equal( ircMessage.Middle, "myusername" );
			Assert.Equal( ircMessage.Parameters, "You are in a maze of twisty passages." );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_MoTD_End_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( ":tmi.twitch.tv 376 myusername :>" );

			Assert.Equal( ircMessage.Host, "tmi.twitch.tv" );
			Assert.Equal( ircMessage.Command, "376" );
			Assert.Equal( ircMessage.Middle, "myusername" );
			Assert.Equal( ircMessage.Parameters, ">" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_AuthenticationFailed_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( ":tmi.twitch.tv NOTICE * :Login authentication failed" );

			Assert.Equal( ircMessage.Host, "tmi.twitch.tv" );
			Assert.Equal( ircMessage.Command, "NOTICE" );
			Assert.Equal( ircMessage.SubCommand, "*" );
			Assert.Equal( ircMessage.Parameters, "Login authentication failed" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.Middle );
			Assert.Null( ircMessage.SubCommand );
		}

	}

}
