using System.Collections.Generic;

public class TokenString
{
	string _text;
	
	public TokenString(string text)
	{
		_text = text;
	}
	
	public string FillWith(CardInfo info)
	{
		return TokenHelpers.FillAllTokensIn(_text, info);
	}
	
	public IEnumerable<Token> GetAllTokens()
	{
		return TokenHelpers.GetAllTokensFrom(_text);
	}
}
