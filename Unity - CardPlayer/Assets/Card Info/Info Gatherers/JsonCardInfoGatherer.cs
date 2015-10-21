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
	
	TokenString _tokenString;
	public TokenString TokenString
	{
		get
		{
			if (_tokenString == null) _tokenString = new TokenString(_baseUrl);
			return _tokenString;
		}
	}
	
	[JsonProperty]
	private Dictionary<string, string> _infoPaths; // id - path
	
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
	
	public void GatherInfoFor(CardInfo cardInfo, System.Action<Dictionary<string,string>> onFinished)
	{
		// I assume all id's are known. So only safe one to use is 'name' at the moment. TODO
		string url = TokenString.FillWith(cardInfo);
		CardInfoProvider.Get.StartCoroutine(LoadInfoFrom(url, onFinished));
	}
	
	IEnumerator LoadInfoFrom(string url, System.Action<Dictionary<string,string>> onFinished)
	{
		using (WWW www = new WWW(url))
		{
			yield return www;
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
}
