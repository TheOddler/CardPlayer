using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public static class TokenHelpers
{
	private static readonly Regex TOKEN_REGEX = new Regex(@"{(.+)}");
	
	public static IEnumerable<Token> GetAllTokensFrom(string text)
	{
		return TOKEN_REGEX.Matches(text).Cast<Match>().Select(m => new Token(m.Groups[1].Value));
	}
	
	public static string FillAllTokensIn(string text, CardInfo info)
	{
		return TOKEN_REGEX.Replace(text, m => TranslateToken(m.Groups[1].Value, info));
	}
	
	private static string TranslateToken(string value, CardInfo card)
	{
		Token token = new Token(value);
		return token.GetValueFrom(card);
	}
}
