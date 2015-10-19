﻿
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class CardImageGatherer
{
	[JsonProperty]
	private string _baseUrl;
	
	public CardImageGatherer(string baseUrl)
	{
		_baseUrl = baseUrl;
	}
	
	public IEnumerator LoadImageFor(CardInfo card/*, System.Action<bool> success*/)
	{
		var tokens = TokenHelpers.GetAllTokensFrom(_baseUrl);
		Stack<Updateable<string>> values = new Stack<Updateable<string>>();
		foreach(var token in tokens)
		{
			values.Push(card.GetExtraInfoById(token.ID));
		}
		while(values.Count > 0)
		{
			var value = values.Peek();
			if (value.Ready) values.Pop();
			else yield return null;
		}
		string url = TokenHelpers.FillAllTokensIn(_baseUrl, card);
		
		using (WWW www = new WWW(url))
		{
			yield return www;
			
			if (www.error == null)
			{
				Texture2D texture = new Texture2D(1, 1); //, TextureFormat.DXT1, false);
				www.LoadImageIntoTexture(texture);
				card.Material.mainTexture = texture;
				//success(true);
			}
			else
			{
				//Debug.Log("Failed to load image for " + card.Name + " from " + url + " with error: " + www.error);
				//success(false);
			}
			
			www.Dispose();
		}
	}
}