using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

public class JsonInfo:
	ExtraCardInfo
{
	private JToken _info;
	
	public JsonInfo(JToken info)
	{
		_info = info;
	}
	
	public string TranslateToken(string path)
	{
		// First see if the token one of my own tokens, so no jsonpath thing
		try
		{
			var valueArray = _info.SelectTokens(path);
			return valueArray.Last().ToString(); //TODO allow other values than just the last
		}
		catch (System.Exception) { }
		
		return "ERROR";
	}
}
