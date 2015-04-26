using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageLoader : MonoBehaviour
{
    const string NAME_TAG = "[NAME]";
    const string SIMPLE_NAME_TAG = "[SNAME]";

    // Singleton
    private static ImageLoader _instance;
    public static ImageLoader Instance
    {
        get { return _instance; }
    }

    //
    // Variables
    // ---
    [SerializeField, TextArea]
    private string _imageUrlBase = "https://deckbox.org/mtg/[SNAME]/tooltip.jpg";

    [SerializeField]
    private Material _cardMaterial;

    private Dictionary<CardInfo, Material> _knownMaterials = new Dictionary<CardInfo,Material>();

    void Awake()
    {
        if (_instance != null) throw new UnityException("Multiple ImageLoaders in the scene.");
        _instance = this;
    }

    public Material GetMaterialFor(CardInfo card)
    {
        if (_knownMaterials.ContainsKey(card))
        {
            return _knownMaterials[card];
        }
        else
        {
            Material mat = new Material(_cardMaterial);
            _knownMaterials[card] = mat;

            StartCoroutine(LoadImageInto(GetTextureUrl(card), mat));

            return mat;
        }
    }

    string GetTextureUrl(CardInfo card)
    {
        return _imageUrlBase
            .Replace(NAME_TAG, WWW.EscapeURL(card.Name))
            .Replace(SIMPLE_NAME_TAG, WWW.EscapeURL(card.SimpleName));
    }

    IEnumerator LoadImageInto(string url, Material mat)
    {
        //url = WWW.EscapeURL(url);

        WWWForm form = new WWWForm();
        form.AddField("Origin", "null");
        WWW www = new WWW(url, form);
        yield return www;

        if (www.error == null)
        {
            var texture = new Texture2D(1, 1/*, TextureFormat.DXT1, false*/);
            www.LoadImageIntoTexture(texture as Texture2D);
            texture.anisoLevel = 4;

            //Debug.Log("Image loaded: " + url);
            mat.mainTexture = texture;
        }
        else
        {
            Debug.Log("Failed to load image: " + url + "; error: " + www.error);
        }
    }

    IEnumerator LoadImageIntoWithBypass(string url, Material mat)
    {
        //url = WWW.EscapeURL(url);

        var www = new WWW(BypassCrosdomain(url));

        Debug.Log("Url: " + www.url);

        yield return www;

        if (www.error == null)
        {
            string withPadding = www.text;
            int imageStart = withPadding.LastIndexOf("<p>") + 3;
            int imageEnd = withPadding.LastIndexOf("</p>");

            Debug.Log("start: " + imageStart + "; end: " + imageEnd);

            byte[] bytes = www.bytes;
            byte[] imageBytes = new byte[imageEnd - imageStart];
            System.Array.Copy(bytes, imageStart, imageBytes, 0, imageEnd - imageStart);

            var texture = new Texture2D(1, 1/*, TextureFormat.DXT1, false*/);
            texture.LoadImage(imageBytes);
            //www.LoadImageIntoTexture(texture as Texture2D);
            texture.anisoLevel = 4;

            //Debug.Log("Image loaded: " + url);
            mat.mainTexture = texture;
        }
        else
        {
            Debug.Log("Failed to load image: " + url + "; error: " + www.error);
        }
    }

    private static string BypassCrosdomain(string url)
    {
        return "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20html%20where%20url%3D'" +
                url + //Double encoding fixes the 400 error you get when having spaces
                "'%0A&format=XML";
    }
}
