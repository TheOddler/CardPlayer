using UnityEngine;
using System.Collections;

public class InfoLoader : MonoBehaviour
{
    // Singleton
    private static InfoLoader _instance;
    public static InfoLoader Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null) throw new UnityException("Multiple ImageLoaders in the scene.");
        _instance = this;
    }

    public void LoadInfoFor(CardInfo card)
    {
        StartCoroutine(LoadInfoForByName(card));
    }

    IEnumerator LoadInfoForByName(CardInfo card)
    {
        //string url = "https://api.deckbrew.com/mtg/cards?name=" + WWW.EscapeURL(card.Name);
        string url = "http://api.mtgdb.info/cards/" + WWW.EscapeURL(card.SimpleName);
        WWW www = new WWW(url);


        yield return www;

        if (www.error == null)
        {
            Debug.Log(card.Name + "\n" + www.text);
        }
        else
        {
            Debug.Log("Failed to load info: " + url + "; error: " + www.error + "\n" + www.text);
        }
    }

}
