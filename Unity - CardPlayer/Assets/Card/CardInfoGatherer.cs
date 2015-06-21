using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class CardInfoGatherer : MonoBehaviour
{
    //
    // Singleton
    // ---
    private static CardInfoGatherer _instance;
    public static CardInfoGatherer Get
    {
        get { return _instance; }
    }

    //
    // Settings
    // ---
    [TextArea(1, 1)]
    [SerializeField]
    private string _baseInfoUrl;

    [TextArea(1, 1)]
    [SerializeField]
    private string _baseImageUrl;

    [SerializeField]
    private Material _frontMaterial;

    //
    // Data
    // ---
    private Dictionary<string, CardInfo> _knownInfo = new Dictionary<string, CardInfo>();

    //
    // Init
    // ---
    void Awake()
    {
        if (_instance != null) throw new UnityException("Multiple ImageLoaders in the scene.");
        _instance = this;
    }

    //
    // Getters
    // ---
    public CardInfo ByName(string name)
    {
        return GetExistingOrDefault(name) ?? CreateCardInfoFor(name);
    }
    // Checks if there is an existing one, if not, returns null
    private CardInfo GetExistingOrDefault(string name)
    {
        if (_knownInfo.ContainsKey(name))
        {
            return _knownInfo[name];
        }
        return null;
    }
    // Creates card info, also starts routines to get additional info
    private CardInfo CreateCardInfoFor(string name)
    {
        Material mat = new Material(_frontMaterial);
        CardInfo card = new CardInfo(name, mat);
        _knownInfo[name] = card;

        StartCoroutine(LoadInfoAndImageFor(card));

        return card;
    }

    //
    // Info Gatherers
    // ---
    IEnumerator LoadInfoAndImageFor(CardInfo card)
    {
        yield return StartCoroutine(LoadInfoFor(card));
        yield return StartCoroutine(LoadImageFor(card));
    }
    IEnumerator LoadInfoFor(CardInfo card)
    {
        string url = card.FillInfoIn(_baseInfoUrl);

        WWW www = new WWW(url);

        yield return www;

        if (www.error == null)
        {
            card.Extra = JObject.Parse(www.text);
            //Debug.Log("Succesfully loaded info for " + card.Name + "\n" + card.Extra.ToString());
        }
        else
        {
            Debug.Log("Failed to load info: " + url + "; error: " + www.error + "\n" + www.text);
        }
    }
    IEnumerator LoadImageFor(CardInfo card)
    {
        string url = card.FillInfoIn(_baseImageUrl);

        WWW www = new WWW(url);
        yield return www;

        if (www.error == null)
        {
            Texture2D texture = new Texture2D(1, 1); //, TextureFormat.DXT1, false);
            www.LoadImageIntoTexture(texture);
            card.Material.mainTexture = texture;
        }
        else
        {
            Debug.Log("Failed to load image for " + card.Name + " from " + url + " with error: " + www.error);
        }
    }
}
