using System;
using System.Collections.Generic;
using System.Linq;

namespace viral32111.InternetRelayChat {

	public class Tags : Dictionary<string, string?> {

		// Parses from a string of extension tags
		public static Dictionary<string, string?> Parse(string tagsToParse, string tagDelimiter = ";", string keyValueDelimiter = "=") =>
			tagsToParse.Split(tagDelimiter)
				.Where(tagToParse => !string.IsNullOrWhiteSpace(tagToParse)) // Skip empty tags
				.Select(tagToParse => tagToParse.Split(keyValueDelimiter, 2))
				.ToDictionary(
					splitTag => splitTag.ElementAtOrDefault(0).NullIfWhiteSpace() ?? throw new Exception("Tag name cannot be null, empty, or whitespace"),
					splitTag => splitTag.ElementAtOrDefault(1).NullIfWhiteSpace() // Tag value can be null, empty, or whitespace
				);

		public static Tags FromDictionary(Dictionary<string, string?> dictionary) =>
			dictionary.Aggregate(new Tags(), (tags, keyValuePair) => {
				tags.Add(keyValuePair.Key, keyValuePair.Value);
				return tags;
			});

		// Converts to a string of extension tags
		public string? Join(string tagSeperator = ";", string keyValueSeperator = "=") =>
			Count == 0 ? null : string.Join(
				tagSeperator,
				this.Select(tag => string.Concat(tag.Key, keyValueSeperator, tag.Value))
			);

	}

}
