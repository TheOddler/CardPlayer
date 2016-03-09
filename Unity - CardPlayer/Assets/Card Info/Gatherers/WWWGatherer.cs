using System;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public abstract class WWWGatherer<T>
{
	TokenString _tokenUrl;
	[JsonProperty]
	public string Url
	{
		get { return _tokenUrl.Text; }
		private set { _tokenUrl = new TokenString(value); }
	}
	
	public WWWGatherer(string baseUrl)
	{
		_tokenUrl = new TokenString(baseUrl);
	}
	
	public void GatherFor(CardInfo cardInfo, System.Action<T> onFinished)
	{
		IEnumerable<Token> tokens = _tokenUrl.GetAllTokens();
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
				GatherFor(cardInfo, onFinished);
			};
			waitFor.UpdatedTo += onUpdated;
		}
		else 
		{
			// All values are ready, start the downloading of texture.
			string url = _tokenUrl.FillWith(cardInfo);
			CardInfoProvider.Get.StartCoroutine(DownloadFrom(url, onFinished));
		}
	}
	
	IEnumerator DownloadFrom(string url, System.Action<T> onFinished)
	{
		using (WWW www = new WWW(url))
		{
			yield return www;
			HandleFinished(www, onFinished);
		}
	}
	
	abstract protected void HandleFinished(WWW www, System.Action<T> onFinished);

	public override string ToString()
	{
		return base.ToString() + "(" + _tokenUrl + ")";
	}
}
