using UnityEngine;

public class Token
{
	private string _id;
	public string ID { get { return _id; } }
	
	private string _settings;
	
	public Token(string token)
	{
		if (token.Contains(":"))
		{
			int separator = token.IndexOf(":");
			_settings = token.Substring(0, separator).Trim();
			_id = token.Substring(separator+1).Trim();
		}
		else
		{
			_id = token.Trim();
		}
	}
	
	public string GetValueFrom(CardInfo cardInfo)
	{
		string value = cardInfo.GetById(_id).Value;
		if (_settings != null && _settings.Contains("e"))
		{
			value = WWW.EscapeURL(value);
		}
		
		return value;
	}
}
