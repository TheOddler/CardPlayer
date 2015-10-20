using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

[JsonObject(MemberSerialization.OptIn)]
public class JsonCardInfoGatherer: CardInfoGatherer
{
	[JsonProperty]
	private string _baseUrl;
	
	[JsonProperty]
	private Dictionary<string, string> _infoPaths;
	
	public JsonCardInfoGatherer(string baseUrl, Dictionary<string, string> infoPaths)
	{
		_baseUrl = baseUrl;
		_infoPaths = infoPaths;
	}
	
	public ICollection<string> PotentialHits
	{
		get 
		{
			return _infoPaths.Keys;
		}
	}

	public IEnumerator LoadInfoFor(CardInfo card)
	{
		string url = TokenHelpers.FillAllTokensIn(_baseUrl, card);
		using (WWW www = new WWW(url))
		{
			yield return www;
			
			if (www.error == null)
			{
				try
				{
					JToken info = JToken.Parse(www.text);
					AddInfoTo(card, info);
					//Debug.Log("Succesfully loaded info for " + card.Name + "\nAdded info: " + info.ToString());
				}
				catch (System.Exception /*e*/)
				{
					//Debug.Log("Failed parsing info as json, with error: " + e.Message + "\nFrom: " + url + "\nGotten text: " + www.text);
				}
			}
			else
			{
				//Debug.Log("Failed to load info for: " + card.Name + "; url: " + url + "; error: " + www.error + "\n" + www.text);
			}
		}
	}
	
	void AddInfoTo(CardInfo cardInfo, JToken info)
	{
		foreach(var infoPath in _infoPaths)
		{
			// get value from json
			string newValue = info.SelectTokens(infoPath.Value).Last().ToString();
			// set value in the card
			cardInfo[infoPath.Key].UpdateValue(newValue);
		}
	}
}
