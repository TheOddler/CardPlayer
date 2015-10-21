using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

[JsonObject(MemberSerialization.OptIn)]
public class JsonCardInfoGatherer: CardInfoGatherer
{
	[JsonProperty]
	private Dictionary<string, string> _infoPaths; // id - path
	
	public JsonCardInfoGatherer(string baseUrl, Dictionary<string, string> infoPaths) : base(baseUrl)
	{
		_infoPaths = infoPaths;
	}
	
	override public ICollection<string> PotentialHits
	{
		get 
		{
			return _infoPaths.Keys;
		}
	}
	
	override protected void HandleFinished(WWW www, System.Action<Dictionary<string,string>> onFinished)
	{
		if (www.error == null)
		{
			try
			{
				JToken jtoken = JToken.Parse(www.text);
				// Find all id-value pairs in the jtoken based on the _infoPaths.
				var dict = new Dictionary<string,string>(_infoPaths.Count);
				foreach(var infoPath in _infoPaths)
				{ // infoPath.Key = id; infoPath.Value = path
					// TODO: Allow some selects to fail
					dict[infoPath.Key] = jtoken.SelectTokens(infoPath.Value, true).Last().ToString();
				}
				//Debug.Log("Succesfully loaded info for " + card.Name + "\nAdded info: " + info.ToString());
				onFinished(dict);
			}
			catch (System.Exception /*e*/)
			{
				//Debug.Log("Failed parsing info as json, with error: " + e.Message + "\nFrom: " + url + "\nGotten text: " + www.text);
				onFinished(null);
			}
		}
		else
		{
			//Debug.Log("Failed to load info for: " + card.Name + "; url: " + url + "; error: " + www.error + "\n" + www.text);
			onFinished(null);
		}
	}
}
