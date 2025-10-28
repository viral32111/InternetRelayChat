using Xunit;

// https://www.alien.net.au/irc/irc2numerics.html

namespace viral32111.InternetRelayChat.Tests.Messages {

	public class Test_Message_Responses {

		[Fact]
		public void Test_Welcome_Response() {
			string message = ":tmi.twitch.tv 001 myusername :Welcome, GLHF!";

			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse(message);

			Assert.Equal("tmi.twitch.tv", ircMessage.Host);
			Assert.Equal("001", ircMessage.Command);
			Assert.Equal("myusername", ircMessage.Middle);
			Assert.Equal("Welcome, GLHF!", ircMessage.Parameters);

			Assert.Empty(ircMessage.Tags);
			Assert.Null(ircMessage.Nick);
			Assert.Null(ircMessage.User);
			Assert.Null(ircMessage.SubCommand);

			Assert.Equal(message, ircMessage.ToString());
		}

		[Fact]
		public void Test_YourHost_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse(":tmi.twitch.tv 002 myusername :Your host is tmi.twitch.tv");

			Assert.Equal("tmi.twitch.tv", ircMessage.Host);
			Assert.Equal("002", ircMessage.Command);
			Assert.Equal("myusername", ircMessage.Middle);
			Assert.Equal("Your host is tmi.twitch.tv", ircMessage.Parameters);

			Assert.Empty(ircMessage.Tags);
			Assert.Null(ircMessage.Nick);
			Assert.Null(ircMessage.User);
			Assert.Null(ircMessage.SubCommand);
		}

		[Fact]
		public void Test_Created_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse(":tmi.twitch.tv 003 myusername :This server is rather new");

			Assert.Equal("tmi.twitch.tv", ircMessage.Host);
			Assert.Equal("003", ircMessage.Command);
			Assert.Equal("myusername", ircMessage.Middle);
			Assert.Equal("This server is rather new", ircMessage.Parameters);

			Assert.Empty(ircMessage.Tags);
			Assert.Null(ircMessage.Nick);
			Assert.Null(ircMessage.User);
			Assert.Null(ircMessage.SubCommand);
		}

		[Fact]
		public void Test_MyInfo_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse(":tmi.twitch.tv 004 myusername :-");

			Assert.Equal("tmi.twitch.tv", ircMessage.Host);
			Assert.Equal("004", ircMessage.Command);
			Assert.Equal("myusername", ircMessage.Middle);
			Assert.Equal("-", ircMessage.Parameters);

			Assert.Empty(ircMessage.Tags);
			Assert.Null(ircMessage.Nick);
			Assert.Null(ircMessage.User);
			Assert.Null(ircMessage.SubCommand);
		}

		[Fact]
		public void Test_MoTD_Start_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse(":tmi.twitch.tv 375 myusername :-");

			Assert.Equal("tmi.twitch.tv", ircMessage.Host);
			Assert.Equal("375", ircMessage.Command);
			Assert.Equal("myusername", ircMessage.Middle);
			Assert.Equal("-", ircMessage.Parameters);

			Assert.Empty(ircMessage.Tags);
			Assert.Null(ircMessage.Nick);
			Assert.Null(ircMessage.User);
			Assert.Null(ircMessage.SubCommand);
		}

		[Fact]
		public void Test_MoTD_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse(":tmi.twitch.tv 372 myusername :You are in a maze of twisty passages.");

			Assert.Equal("tmi.twitch.tv", ircMessage.Host);
			Assert.Equal("372", ircMessage.Command);
			Assert.Equal("myusername", ircMessage.Middle);
			Assert.Equal("You are in a maze of twisty passages.", ircMessage.Parameters);

			Assert.Empty(ircMessage.Tags);
			Assert.Null(ircMessage.Nick);
			Assert.Null(ircMessage.User);
			Assert.Null(ircMessage.SubCommand);
		}

		[Fact]
		public void Test_MoTD_End_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse(":tmi.twitch.tv 376 myusername :>");

			Assert.Equal("tmi.twitch.tv", ircMessage.Host);
			Assert.Equal("376", ircMessage.Command);
			Assert.Equal("myusername", ircMessage.Middle);
			Assert.Equal(">", ircMessage.Parameters);

			Assert.Empty(ircMessage.Tags);
			Assert.Null(ircMessage.Nick);
			Assert.Null(ircMessage.User);
			Assert.Null(ircMessage.SubCommand);
		}

		[Fact]
		public void Test_AuthenticationFailed_Response() {
			InternetRelayChat.Message ircMessage = InternetRelayChat.Message.Parse(":tmi.twitch.tv NOTICE * :Login authentication failed");

			Assert.Equal("tmi.twitch.tv", ircMessage.Host);
			Assert.Equal("NOTICE", ircMessage.Command);
			Assert.Equal("Login authentication failed", ircMessage.Parameters);

			Assert.Empty(ircMessage.Tags);
			Assert.Null(ircMessage.Nick);
			Assert.Null(ircMessage.User);
			Assert.Null(ircMessage.Middle);
			Assert.Null(ircMessage.SubCommand);
		}

	}

}
