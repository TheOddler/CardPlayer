using System;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class CardImageGatherer
{
	[JsonProperty]
	string _baseUrl;
	
	TokenString _tokenString;
	public TokenString TokenString
	{
		get
		{
			if (_tokenString == null) _tokenString = new TokenString(_baseUrl);
			return _tokenString;
		}
	}
	
	public CardImageGatherer(string baseUrl)
	{
		_baseUrl = baseUrl;
	}
	
	public void GatherImageFor(CardInfo cardInfo, System.Action<Texture2D> onFinished)
	{
		IEnumerable<Token> tokens = TokenString.GetAllTokens();
		// Get values. Also use 'to list' to make sure all are asked for now.
		List<Updateable<string>> values = tokens.Select(t => cardInfo[t.ID]).ToList();
		// See if there are still values I need to wait for
		var waitFor = values.FirstOrDefault(t => !t.Ready);
		if (waitFor != null)
		{
			// When the value I was waiting for is updated, call this method again.
			// Make sure to remove the onUpdated method again when UpdatedTo is called
			Action<string> onUpdated = null;
			onUpdated = (v) => 
			{
				waitFor.UpdatedTo -= onUpdated;
				GatherImageFor(cardInfo, onFinished);
			};
			waitFor.UpdatedTo += onUpdated;
		}
		else 
		{
			// All values are ready, start the downloading of texture.
			string url = TokenString.FillWith(cardInfo);
			CardInfoProvider.Get.StartCoroutine(DownloadFrom(url, onFinished));
		}
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
