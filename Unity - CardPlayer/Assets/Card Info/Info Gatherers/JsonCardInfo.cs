using System.Linq;
using Newtonsoft.Json.Linq;

public class JsonCardInfo:
	ExtraCardInfo
{
	private JToken _info;
	private JsonCardInfoGatherer _parent;
	
	public JsonCardInfo(JToken info, JsonCardInfoGatherer parent)
	{
		_info = info;
		_parent = parent;
	}
	
	public string GetById(string id)
	{
		return GetByPath(_parent.GetTokenValueFor(id));
	}
	
	public string GetByPath(string path)
	{
		// First see if the token one of my own tokens, so no jsonpath thing
		try
		{
			var valueArray = _info.SelectTokens(path);
			return valueArray.Last().ToString(); //TODO allow other values than just the last
		}
		catch (System.Exception) { }
		
		return "ERROR"; //TODO better solution?
	}
}
