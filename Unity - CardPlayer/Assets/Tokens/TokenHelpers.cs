using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public static class TokenHelpers
{
	private static readonly Regex TOKEN_REGEX = new Regex(@"{(.+)}");
	
	public static IEnumerable<Token> GetAllTokensFrom(string text)
	{
		return TOKEN_REGEX.Matches(text).Cast<Match>().Select(m => new Token(m.Value));
	}
	
	public static string FillAllTokensIn(string text, CardInfo info)
	{
		return TOKEN_REGEX.Replace(text, match => TranslateToken(match.Groups[1].Value, info));
	}
	
	public static string TranslateToken(string token, CardInfo info)
	{
		return info.GetExtraInfoById(token).Value;
	}
}
