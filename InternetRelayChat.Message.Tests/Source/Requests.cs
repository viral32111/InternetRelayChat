using Xunit;

namespace viral32111.InternetRelayChat.Message.Tests {

	public class UnitTests_Requests {

		[Fact]
		public void Test_Capabilities_Request() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands" );

			Assert.Equal( ircMessage.Command, "CAP" );
			Assert.Equal( ircMessage.SubCommand, "REQ" );
			Assert.Equal( ircMessage.Parameters, "twitch.tv/membership twitch.tv/tags twitch.tv/commands" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.Host );
			Assert.Null( ircMessage.Middle );
		}

		[Fact]
		public void Test_AuthenticationPassword_Request() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( "PASS oauth:yfvzjqb705z12hrhy1zkwa9xt7v662" );

			Assert.Equal( ircMessage.Command, "PASS" );
			Assert.Equal( ircMessage.Middle, "oauth:yfvzjqb705z12hrhy1zkwa9xt7v662" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.Host );
			Assert.Null( ircMessage.Parameters );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_AuthenticationNickname_Request() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse( "NICK myusername" );

			Assert.Equal( ircMessage.Command, "NICK" );
			Assert.Equal( ircMessage.Middle, "myusername" );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.Host );
			Assert.Null( ircMessage.Parameters );
			Assert.Null( ircMessage.SubCommand );
		}

	}

}
