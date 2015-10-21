using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class CardImageGatherer: WWWGatherer<Texture2D>
{
	public CardImageGatherer(string baseUrl) : base(baseUrl) { }
	
	override protected void HandleFinished(WWW www, System.Action<Texture2D> onFinished)
	{
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
