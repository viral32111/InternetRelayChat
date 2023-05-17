using System;
using System.Threading;
using System.Threading.Tasks;

namespace viral32111.InternetRelayChat.Examples;

public class Program {

	private static readonly string ircServerName = ""; // irc.chat.twitch.tv;
	private static readonly string ircClientUser = "";
	private static readonly string ircClientPassword = "";

	public static async Task Main( string[] arguments ) {
		
		CancellationTokenSource cancellationTokenSource = new();
		Client ircClient = new();
	
		ircClient.OpenedEvent += ( object sender, OpenedEventArgs e ) => {
			Console.WriteLine( "Connection to {0}:{1} ({2}) opened", e.RemoteAddress, e.RemotePort, e.RemoteName );
		};

		ircClient.ClosedEvent += ( object sender, ClosedEventArgs e ) => {
			Console.WriteLine( "Connection with {0}:{1} closed", e.RemoteAddress, e.RemotePort );
		};

		ircClient.SecuredEvent += ( object sender, SecuredEventArgs e ) => {
			Console.WriteLine( "Connection with {0}:{1} ({2}) secured with {3} & {4} (Encrypted: {5}, Signed: {6}, Authenticated: {7})", e.RemoteAddress, e.RemotePort, e.RemoteName, e.Protocol, e.CipherAlgorithm, e.IsEncrypted, e.IsSigned, e.IsAuthenticated );
		};

		Console.WriteLine( "Opening connection..." );
		await ircClient.OpenAsync( ircServerName, cancellationToken: cancellationTokenSource.Token );
		Console.WriteLine( "Connection opened." );

		Console.WriteLine( "Starting receive in background task..." );
		Task receiveTask = ReceiveInBackground( ircClient, cancellationTokenSource.Token );
		Console.WriteLine( "Started receiving in background task..." );

		Console.WriteLine( "Pinging..." );
		await ircClient.SendAsync( new( command: "PING" ) );
		Console.WriteLine( "Pinged." );

		/*
		Console.WriteLine( "Closing connection..." );
		await ircClient.CloseAsync( cancellationTokenSource.Token );
		Console.WriteLine( "Connection closed." );
		*/

		Console.WriteLine( "Waiting for receive task to complete..." );
		await receiveTask;
		Console.WriteLine( "Receive task completed." );

	}

	private static async Task ReceiveInBackground( Client ircClient, CancellationToken cancellationToken ) {
		while ( ircClient.IsConnected() && !cancellationToken.IsCancellationRequested ) {
			Message[] messages = await ircClient.ReceiveAsync( cancellationToken: cancellationToken );
			foreach ( Message message in messages ) {
				Console.WriteLine( "Received message: '{0}'", message.ToString() );

				if ( message.Parameters?.Contains( "Could not resolve your hostname" ) == true ) {
					Console.WriteLine( "Registering..." );
					await ircClient.SendAsync( new( command: Command.Password, middle: ircClientPassword ) );
					//await ircClient.SendAsync( new( command: Command.User, middle: $"{ircClientUser} {ircClientUser} {ircClientUser}", parameters: ircClientUser ) );
					await ircClient.SendAsync( new( command: Command.Nick, middle: ircClientUser ) );
					Console.WriteLine( "Registered." );
				}
			}
		}
	}

}
