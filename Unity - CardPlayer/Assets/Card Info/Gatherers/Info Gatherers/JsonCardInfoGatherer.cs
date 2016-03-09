using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

[JsonObject(MemberSerialization.OptIn)]
public class JsonCardInfoGatherer: CardInfoGatherer
{
	private Dictionary<string, TokenString> _infoPaths; // id - path
	[JsonProperty]
	private Dictionary<string, string> InfoPaths //also for serialization
	{
		get { return _infoPaths.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Text); }
		set { _infoPaths = value.ToDictionary(kvp => kvp.Key, kvp => new TokenString(kvp.Value)); }
	}


	public JsonCardInfoGatherer(string baseUrl, Dictionary<string, string> infoPaths) : base(baseUrl)
	{
		InfoPaths = infoPaths;
	}
	
	override public ICollection<string> PotentialHits
	{
		get 
		{
			return _infoPaths.Keys;
		}
	}

	override protected IEnumerable<Token> GetRequiredTokens()
	{
		return base.GetRequiredTokens().Concat(_infoPaths.SelectMany(kvp => kvp.Value.GetAllTokens())).Distinct();
	}

	override protected void HandleFinished(WWW www, CardInfo cardInfo, System.Action<Dictionary<string,string>> onFinished)
	{
		if (www.error == null)
		{
			try
			{
				JToken jtoken = JToken.Parse(www.text);
				// Find all id-value pairs in the jtoken based on the _infoPaths.
				var dict = new Dictionary<string,string>(_infoPaths.Count);
				foreach(var infoPath in _infoPaths)
				{
					// infoPath.Key = id; infoPath.Value = path
					string filledPath = infoPath.Value.GetFilledWith(cardInfo);
					var foundTokens = jtoken.SelectTokens(filledPath, true);
					if (foundTokens.Any())
					{
						string selected = foundTokens.Last().ToString();
						dict[infoPath.Key] = selected;
					}
				}
				//Debug.Log("Succesfully loaded info from " + www.url + "\nText: " + www.text);
				onFinished(dict);
			}
			catch (System.Exception e)
			{
				Debug.Log("Failed parsing info as json, with error: " + e.Message + "\nFrom: " + www.url + "\nGotten text: " + www.text);
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
