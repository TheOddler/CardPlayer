using System.Collections.Generic;
using Newtonsoft.Json;

public class TokenString
{
	private string _text;
	public string Text { get { return _text; } }
	
	public TokenString(string text)
	{
		_text = text;
	}
	
	public string GetFilledWith(CardInfo info)
	{
		return TokenHelpers.FillAllTokensIn(_text, info);
	}
	
	public IEnumerable<Token> GetAllTokens()
	{
		return TokenHelpers.GetAllTokensFrom(_text);
	}
	
	public override string ToString()
	{
		return "Token: " + _text;
	}
}
