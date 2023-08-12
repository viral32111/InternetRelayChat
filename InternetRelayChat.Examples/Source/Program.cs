using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace viral32111.InternetRelayChat.Examples;

public class Program {

	private static readonly Random random = new();

	private static readonly ILogger logger = LoggerFactory.Create( builder => {
		builder.ClearProviders();

		builder.AddConsoleFormatter<CustomConsoleFormatter, SimpleConsoleFormatterOptions>( options => {
			options.UseUtcTimestamp = false;
			options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff zzz] ";

			options.ColorBehavior = LoggerColorBehavior.Enabled;

			// Not implemented
			options.SingleLine = true;
			options.IncludeScopes = true;
		} );

		builder.AddConsole( options => options.FormatterName = "Custom" );

		#if DEBUG
			builder.SetMinimumLevel( LogLevel.Trace );
		#else
			builder.SetMinimumLevel( LogLevel.Information );
		#endif
	} ).CreateLogger( "viral32111.InternetRelayChat" );

	//private static readonly string ircServerName = "irc.ipv4.libera.chat"; // https://libera.chat/guides/connect
	private static readonly string ircServerName = "irc.chat.twitch.tv"; // https://dev.twitch.tv/docs/irc/#connecting-to-the-twitch-irc-server
	private static readonly string ircClientUser = "ExampleUser25";
	private static readonly string ircClientPassword = "P4ssw0rd!";

	public static async Task Main() {
		logger.LogInformation( "Hello, World!" );

		CancellationTokenSource cancellationTokenSource = new();
		Client ircClient = new( logger );
	
		ircClient.OpenedEvent += ( object sender, OpenedEventArgs e ) => {
			logger.LogInformation( "Connection to {0}:{1} ({2}) opened", e.RemoteAddress, e.RemotePort, e.RemoteName );
		};

		ircClient.ClosedEvent += ( object sender, ClosedEventArgs e ) => {
			logger.LogInformation( "Connection with {0}:{1} closed", e.RemoteAddress, e.RemotePort );
		};

		ircClient.SecuredEvent += ( object sender, SecuredEventArgs e ) => {
			logger.LogInformation( "Connection with {0}:{1} ({2}) secured with {3} & {4} (Encrypted: {5}, Signed: {6}, Authenticated: {7})", e.RemoteAddress, e.RemotePort, e.RemoteName, e.Protocol, e.CipherAlgorithm, e.IsEncrypted, e.IsSigned, e.IsAuthenticated );
		};

		logger.LogInformation( "Opening connection..." );
		await ircClient.OpenAsync( ircServerName, beSecure: false, cancellationToken: cancellationTokenSource.Token );
		logger.LogInformation( "Connection opened." );

		logger.LogInformation( "Starting receive in background task..." );
		Task receiveTask = ReceiveInBackground( ircClient, cancellationTokenSource.Token );
		logger.LogInformation( "Started receiving in background task..." );

		/*logger.LogInformation( "Pinging..." );
		await ircClient.SendAsync( new( command: "PING" ) );
		logger.LogInformation( "Pinged." );
		*/


		await ircClient.SendAsync( new( command: Command.Password, middle: ircClientPassword ) );
		await ircClient.SendAsync( new( command: Command.Nick, middle: ircClientUser ) );

		/*
		logger.LogInformation( "Closing connection..." );
		await ircClient.CloseAsync( CloseReason.ByClient, cancellationTokenSource.Token );
		logger.LogInformation( "Connection closed." );
		*/

		try {
			logger.LogInformation( "Waiting for receive task to complete..." );
			await receiveTask;
			logger.LogInformation( "Receive task completed." );
		} catch ( IOException exception ) {
			logger.LogWarning( "I/O exception while waiting for receive task: '{0}'!", exception.Message );
		}

	}

	private static async Task ReceiveInBackground( Client ircClient, CancellationToken cancellationToken ) {
		while ( ircClient.IsConnected() && !cancellationToken.IsCancellationRequested ) {
			Message[] messages = await ircClient.ReceiveAsync( cancellationToken: cancellationToken );
			foreach ( Message message in messages ) {
				logger.LogInformation( "Received message: '{0}'", message.ToString() );

				/*if ( message.Parameters?.Contains( "No Ident response" ) == true ) {
					logger.LogInformation( "Registering..." );
	
					await ircClient.SendAsync( new( command: Command.User, middle: $"{ GenerateRandomString( 12 ) } { ircServerName } { ircServerName }", parameters: ircClientUser ) );
					await ircClient.SendAsync( new( command: Command.Nick, middle: ircClientUser ) );

					logger.LogInformation( "Registered." );
				}*/
			}
		}
	}

	private static string GenerateRandomString( int length ) =>
		new( Enumerable.Repeat( "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length ).Select( str => str[ random.Next( str.Length ) ] ).ToArray() );

}

public sealed class CustomConsoleFormatter : ConsoleFormatter, IDisposable {

	private readonly IDisposable? optionsReloadToken;
	private SimpleConsoleFormatterOptions formatterOptions;

	public CustomConsoleFormatter( IOptionsMonitor<SimpleConsoleFormatterOptions> options ) : base( "Custom" ) =>
		( optionsReloadToken, formatterOptions ) = ( options.OnChange( ReloadLoggerOptions ), options.CurrentValue );

	private void ReloadLoggerOptions( SimpleConsoleFormatterOptions options ) => formatterOptions = options;

	public void Dispose() => optionsReloadToken?.Dispose();

	public override void Write<TState>( in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter ) {
		string? message = logEntry.Formatter?.Invoke( logEntry.State, logEntry.Exception );
		if ( message == null ) return;

		WriteColor( textWriter, logEntry.LogLevel );
		WriteTimestamp( textWriter );
		WriteLogLevel( textWriter, logEntry.LogLevel );

		if ( logEntry.Exception != null ) {
			textWriter.Write( message );
			textWriter.Write( ": " );
			textWriter.WriteLine( logEntry.Exception.Message );
		} else {
			textWriter.WriteLine( message );
		}

		if ( formatterOptions.ColorBehavior != LoggerColorBehavior.Disabled ) textWriter.Write( "\x1B[0m" );
	}

	private void WriteColor( TextWriter textWriter, LogLevel logLevel ) {
		if ( formatterOptions.ColorBehavior == LoggerColorBehavior.Disabled ) return;

		textWriter.Write( logLevel switch {
			LogLevel.Trace => "\x1B[36;3m", // Italic & Cyan
			LogLevel.Debug => "\x1B[36;22m", // Cyan
			LogLevel.Information => "\x1B[37;22m", // White
			LogLevel.Warning => "\x1B[33;22m", // Yellow
			LogLevel.Error => "\x1B[31;22m", // Red
			LogLevel.Critical => "\x1B[31;1m", // Bold & Red
			_ => throw new ArgumentException( $"Unrecognised log level '{ logLevel }'" )
		} );
	}

	private void WriteLogLevel( TextWriter textWriter, LogLevel logLevel ) {
		string logLevelString = logLevel switch {
			LogLevel.Trace => "TRACE",
			LogLevel.Debug => "DEBUG",
			LogLevel.Information => "INFO",
			LogLevel.Warning => "WARN",
			LogLevel.Error => "ERROR",
			LogLevel.Critical => "CRITICAL",
			_ => throw new ArgumentException( $"Unrecognised log level '{ logLevel }'" )
		};

		textWriter.Write( $"[{ logLevelString }] " );
	}

	private void WriteTimestamp( TextWriter textWriter ) {
		if ( formatterOptions.TimestampFormat == null ) return;

		DateTimeOffset currentDateTime = formatterOptions.UseUtcTimestamp == true ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
		textWriter.Write( currentDateTime.ToString( formatterOptions.TimestampFormat ) );
	}

}
