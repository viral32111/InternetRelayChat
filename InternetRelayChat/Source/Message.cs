using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace viral32111.InternetRelayChat {

	public class Message {

		// Regular expressions for capturing components of IRC messages
		private static readonly Regex MessageStartPattern = new( @"^(?>@(?'tags'.+?) )?(?>:(?>(?'nick'[\w.]+))?(?>!(?'user'[\w.]+))?(?>@?(?'host'[\w.]+)) )?(?'command'\d{3}|[A-Z]+)(?> \*)?(?> (?'subcommand'\d{3}|[A-Z]+))?" );
		private static readonly Regex MessageEndPattern = new( @"^(?> (?'middle'.*?))?(?> :(?'params'.+))?$", RegexOptions.RightToLeft );

		// Extensions - https://ircv3.net/specs/extensions/message-tags.html
		public readonly Tags Tags;

		// Prefix
		public readonly string? Nick;
		public readonly string? User;
		public readonly string? Host;

		// Command
		public readonly string Command;
		public readonly string? SubCommand; // E.g, 'CAP * ACK' where CAP is command and ACK is sub-command?

		// Data
		public readonly string? Middle; // Sometimes present, sometimes not?
		public readonly string? Parameters;

		// Full message with all components
		public Message(
			string command,
			Tags? tags = null,
			string? nick = null,
			string? user = null,
			string? host = null,
			string? subCommand = null,
			string? middle = null,
			string? parameters = null
		) {
			Tags = tags ?? new();
			Nick = nick;
			User = user;
			Host = host;
			Command = command;
			SubCommand = subCommand;
			Middle = middle;
			Parameters = parameters;
		}

		// Client message to be sent to a server, does not have all components
		/*
		public Message(
			string command,
			string? middle = null,
			string? parameters = null,
			Tags? tags = null
		) {
			Tags = tags ?? new();
			Command = command;
			Middle = middle;
			Parameters = parameters;
		}

		public Message(
			string command
		) {
			Tags = new();
			Command = command;
		}

		public Message(
			string command,
			string middle
		) {
			Tags = new();
			Command = command;
			Middle = middle;
		}
		*/

		// Parses a single IRC message from a string
		public static Message Parse( string messageToParse ) {
			if ( string.IsNullOrWhiteSpace( messageToParse ) ) throw new ArgumentException( "Message cannot be null, empty, or whitespace", nameof( messageToParse ) );

			// Match the message using the regular expressions
			Match messageStartMatch = MessageStartPattern.Match( messageToParse );
			if ( messageStartMatch.Success == false ) throw new ArgumentException( "Message is not a valid IRC message", nameof( messageToParse ) );
			Match messageEndMatch = MessageEndPattern.Match( messageToParse[ messageStartMatch.Length.. ] );
			if ( messageEndMatch.Success == false ) throw new ArgumentException( "Message is not a valid IRC message", nameof( messageToParse ) );

			// Store all the captured values, they may be null
			string? tagsCapture = messageStartMatch.Groups[ "tags" ].Value.NullIfWhiteSpace();
			string? nickCapture = messageStartMatch.Groups[ "nick" ].Value.NullIfWhiteSpace();
			string? userCapture = messageStartMatch.Groups[ "user" ].Value.NullIfWhiteSpace();
			string? hostCapture = messageStartMatch.Groups[ "host" ].Value.NullIfWhiteSpace();
			string? commandCapture = messageStartMatch.Groups[ "command" ].Value.NullIfWhiteSpace();
			string? subCommandCapture = messageStartMatch.Groups[ "subcommand" ].Value.NullIfWhiteSpace();
			string? middleCapture = messageEndMatch.Groups[ "middle" ].Value.NullIfWhiteSpace();
			string? paramsCapture = messageEndMatch.Groups[ "params" ].Value.NullIfWhiteSpace();

			// Create a message object using captured values, the command is required
			return new(
				command: commandCapture ?? throw new Exception( "No command found in IRC message" ),
				tags: tagsCapture != null ? Tags.FromDictionary( Tags.Parse( tagsCapture ) ) : null,
				nick: nickCapture,
				user: userCapture,
				host: hostCapture,
				subCommand: subCommandCapture,
				middle: middleCapture,
				parameters: paramsCapture
			);
		}

		// Parses a single IRC message from a byte array, defaults to UTF-8
		public static Message Parse( byte[] messageBytesToParse, Encoding? desiredEncoding = null ) =>
			Parse( ( desiredEncoding ?? Encoding.UTF8 ).GetString( messageBytesToParse ) );

		// Parses multiple IRC messages in a string
		public static Message[] ParseMany( string messagesToParse, string lineDelimiter = "\r\n" ) =>
			messagesToParse.Split( lineDelimiter )
				.Where( messageToParse => !string.IsNullOrWhiteSpace( messageToParse ) ) // Skip empty messages
				.Select( Parse )
				.ToArray();

		// Parses multiple IRC messages in a byte array, defaults to UTF-8
		public static Message[] ParseMany( byte[] messagesToParse, string lineDelimiter = "\r\n", Encoding? desiredEncoding = null ) =>
			ParseMany( ( desiredEncoding ?? Encoding.UTF8 ).GetString( messagesToParse ), lineDelimiter );

		// Converts the message components into a string
		public override string ToString() => string.Concat(
			Tags.Count > 0 ? string.Concat( Tags.Join(), " " ) : string.Empty,
			Host != null ? string.Concat( ":", ( Nick != null && User != null ? $"{Nick}!{User}@{Host}" : Host ), " " ) : string.Empty,
			Command == "NOTICE" ? string.Concat( Command, " *" ) : Command, // Why are NOTICE commands always followed by an asterisk?
			SubCommand != null ? string.Concat( " *", SubCommand ) : string.Empty,
			Middle != null ? string.Concat( " ", Middle ) : string.Empty,
			Parameters != null ? string.Concat( " :", Parameters ) : string.Empty
		);

		// Converts the message to a byte array, defaults to UTF-8
		public byte[] GetBytes( string lineEnding = "\r\n", Encoding? desiredEncoding = null ) =>
			( desiredEncoding ?? Encoding.UTF8 ).GetBytes( string.Concat( ToString(), lineEnding ) );

	}

}
