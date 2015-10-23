using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public static class TokenHelpers
{
	public static IEnumerable<Token> GetAllTokensFrom(string text)
	{
		return Token.REGEX.Matches(text).Cast<Match>().Select(m => new Token(m));
	}
	
	public static string FillAllTokensIn(string text, CardInfo info)
	{
		return Token.REGEX.Replace(text, m => TranslateToken(m, info));
	}
	
	private static string TranslateToken(Match match, CardInfo card)
	{
		Token token = new Token(match);
		return token.GetValueFrom(card);
	}
}
