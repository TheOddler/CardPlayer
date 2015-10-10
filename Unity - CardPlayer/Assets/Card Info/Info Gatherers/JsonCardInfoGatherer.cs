using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[JsonObject(MemberSerialization.OptIn)]
public class JsonCardInfoGatherer: CardInfoGatherer
{
	[JsonProperty]
	private string _baseUrl;
	
	[JsonProperty]
	private Dictionary<string, string> _tokens;
	
	public JsonCardInfoGatherer(string baseUrl, Dictionary<string, string> tokens)
	{
		_baseUrl = baseUrl;
		_tokens = tokens;
	}
	
	public ICollection<string> PotentialHits
	{
		get 
		{
			return _tokens.Keys;
		}
	}

	public IEnumerator LoadInfoFor(CardInfo card)
	{
		string url = TokenHelpers.FillInfoIn(_baseUrl, card);
		using (WWW www = new WWW(url))
		{
			yield return www;
			
			if (www.error == null)
			{
				try
				{
					JToken info = JToken.Parse(www.text);
					card.AddExtraInfo(new JsonCardInfo(info, this));
					//Debug.Log("Succesfully loaded info for " + card.Name + "\nAdded info: " + info.ToString());
				}
				catch (System.Exception e)
				{
					Debug.Log("Failed parsing info as json, with error: " + e.Message + "\nFrom: " + url + "\nGotten text: " + www.text);
				}
			}
			else
			{
				Debug.Log("Failed to load info for: " + card.Name + "; url: " + url + "; error: " + www.error + "\n" + www.text);
			}
		}
	}
	
	public string GetTokenValueFor(string id)
	{
		string value;
		return _tokens.TryGetValue(id, out value) ? value : null;
	}
}
