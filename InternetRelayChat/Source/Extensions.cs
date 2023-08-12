namespace viral32111.InternetRelayChat;

public static class Extensions {

	// Returns null if the string is empty or whitespace
	public static string? NullIfWhiteSpace( this string? str ) => string.IsNullOrWhiteSpace( str ) ? null : str;

}
