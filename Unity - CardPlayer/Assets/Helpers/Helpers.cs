using System.Collections.Generic;
using System.Text;

public static class Helpers
{
	//
	// Special characters
	// ---
	private static readonly Dictionary<string, string> SPECIAL_MAPPING = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
	{
		{"œ", "oe"},
		{"Ÿ", "Y"},
		{"À", "A"}, {"Á", "A"}, {"Â", "A"}, {"Ã", "A"}, {"Ä", "A"},
		{"Æ", "AE"},
		{"é", "e"},
		{"/",""}
	};

	public static string SimplifySpecialCharacter(this string text)
	{
		var output = new StringBuilder(text);
		foreach (var kvp in SPECIAL_MAPPING)
		{
			output.Replace(kvp.Key, kvp.Value);
		}
		return output.ToString();
	}
}
