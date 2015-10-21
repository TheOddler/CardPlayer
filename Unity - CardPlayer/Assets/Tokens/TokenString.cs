using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.Fields)]
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
	
	public override string ToString()
	{
		return "Token: " + _text;
	}
}
