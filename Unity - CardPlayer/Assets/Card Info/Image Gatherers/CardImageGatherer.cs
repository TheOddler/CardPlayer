
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class CardImageGatherer
{
	[JsonProperty]
	private string _baseUrl;
	
	TokenString _tokenString;
	
	public CardImageGatherer(string baseUrl)
	{
		_baseUrl = baseUrl;
		_tokenString = new TokenString(baseUrl);
	}
	
	public void GatherImageFor(CardInfo cardInfo, System.Action<Texture2D> onFinished)
	{
		IEnumerable<Token> tokens = _tokenString.GetAllTokens();
		// Get values. Also use 'to list' to make sure all are asked for now.
		List<Updateable<string>> values = tokens.Select(t => cardInfo[t.ID]).ToList();
		// TODO
		// Wait for values if needed
		// By subscribing to the non-ready update events
		// Create url
		// Start download coroutine
	}
	
	IEnumerator DownloadFrom(string url, System.Action<Texture2D> onFinished)
	{
		using (WWW www = new WWW(url))
		{
			yield return www;
			
			if (www.error == null)
			{
				onFinished(www.texture);
			}
			else
			{
				//Debug.Log("Failed to load image for " + card.Name + " from " + url + " with error: " + www.error);
				onFinished(null);
			}
		}
	}
}
