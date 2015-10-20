
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
	
	public IEnumerator LoadImageFor(CardInfo cardInfo/*, System.Action<bool> success*/)
	{
		var tokens = TokenHelpers.GetAllTokensFrom(_baseUrl);
		Stack<Updateable<string>> values = new Stack<Updateable<string>>();
		foreach(var token in tokens)
		{
			values.Push(cardInfo[token.ID]);
		}
		// Spin wait TODO make better
		while(values.Count > 0)
		{
			var value = values.Peek();
			if (value.Ready) values.Pop();
			else yield return null;
		}
		string url = TokenHelpers.FillAllTokensIn(_baseUrl, cardInfo);
		//Debug.Log("Image url: " + url);
		
		using (WWW www = new WWW(url))
		{
			yield return www;
			
			if (www.error == null)
			{
				Texture2D texture = new Texture2D(1, 1); //, TextureFormat.DXT1, false);
				www.LoadImageIntoTexture(texture);
				cardInfo.Material.mainTexture = texture;
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
