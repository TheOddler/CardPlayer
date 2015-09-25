﻿using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class CardInfoGatherer
{
    [TextArea(1, 5)]
    [SerializeField]
    private string _baseInfoUrl;

    [TextArea(1, 5)]
    [SerializeField]
    private string _baseImageUrl;

    public IEnumerator LoadInfoFor(CardInfo card, System.Action<bool> success)
    {
        string url = card.FillInfoIn(_baseInfoUrl);

        var headers = new Dictionary<string,string>();
        headers["Access-Control-Allow-Origin"] = "*";
        WWW www = new WWW(url, null, headers);

        yield return www;

        if (www.error == null)
        {
            try
            {
                card.Extra = JToken.Parse(www.text);
                //Debug.Log("Succesfully loaded info for " + card.Name + "\n" + card.Extra.ToString());
                success(true);
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed parsing info as json, with error: " + e.Message
                    + "\nFrom: " + url
                    + "\nGotten text: " + www.text);
                success(false);
            }
        }
        else
        {
            Debug.Log("Failed to load info for: " + card.Name + "; url: " + url + "; error: " + www.error + "\n" + www.text);
            success(false);
        }
    }

    public IEnumerator LoadImageFor(CardInfo card, System.Action<bool> success)
    {
        string url = card.FillInfoIn(_baseImageUrl);

        var headers = new Dictionary<string, string>();
        headers["Access-Control-Allow-Origin"] = "*";
        WWW www = new WWW(url, null, headers);

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
            Debug.Log("Failed to load image for " + card.Name + " from " + url + " with error: " + www.error);
            success(false);
        }
    }
}
