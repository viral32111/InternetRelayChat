using Xunit;

namespace viral32111.InternetRelayChat.Tests.Messages {

	public class Test_Message_Requests {

		[Fact]
		public void Test_Capabilities_Request() {
			Message ircMessage = Message.Parse( "CAP REQ :twitch.tv/membership twitch.tv/tags twitch.tv/commands" );

			Assert.Equal( "CAP", ircMessage.Command );
			Assert.Equal( "REQ", ircMessage.SubCommand );
			Assert.Equal( "twitch.tv/membership twitch.tv/tags twitch.tv/commands", ircMessage.Parameters );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.Host );
			Assert.Null( ircMessage.Middle );
		}

		[Fact]
		public void Test_AuthenticationPassword_Request() {
			Message ircMessage = Message.Parse( "PASS oauth:yfvzjqb705z12hrhy1zkwa9xt7v662" );

			Assert.Equal( "PASS", ircMessage.Command );
			Assert.Equal( "oauth:yfvzjqb705z12hrhy1zkwa9xt7v662", ircMessage.Middle );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.Host );
			Assert.Null( ircMessage.Parameters );
			Assert.Null( ircMessage.SubCommand );
		}

		[Fact]
		public void Test_AuthenticationNickname_Request() {
			Message ircMessage = Message.Parse( "NICK myusername" );

			Assert.Equal( "NICK", ircMessage.Command );
			Assert.Equal( "myusername", ircMessage.Middle );

			Assert.Empty( ircMessage.Tags );
			Assert.Null( ircMessage.Nick );
			Assert.Null( ircMessage.User );
			Assert.Null( ircMessage.Host );
			Assert.Null( ircMessage.Parameters );
			Assert.Null( ircMessage.SubCommand );
		}

	}

}
