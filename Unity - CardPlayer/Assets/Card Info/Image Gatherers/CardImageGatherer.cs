
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
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
	
	public IEnumerator LoadImageFor(CardInfo card, System.Action<bool> success)
	{
		string url = TokenHelpers.FillAllTokensIn(_baseUrl, card);
		
		using (WWW www = new WWW(url))
		{
			yield return www;
			
			if (www.error == null)
			{
				Texture2D texture = new Texture2D(1, 1); //, TextureFormat.DXT1, false);
				www.LoadImageIntoTexture(texture);
				card.Material.mainTexture = texture;
				success(true);
			}
			else
			{
				//Debug.Log("Failed to load image for " + card.Name + " from " + url + " with error: " + www.error);
				success(false);
			}
			
			www.Dispose();
		}
	}
}
