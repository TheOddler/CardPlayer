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

        var www = new WWW(url);
        yield return www;

        if (www.error == null)
        {
            var texture = new Texture2D(1, 1/*, TextureFormat.DXT1, false*/);
            www.LoadImageIntoTexture(texture as Texture2D);
            texture.anisoLevel = 4;

            Debug.Log("Image loaded: " + url);
            mat.mainTexture = texture;
        }
        else
        {
            Debug.Log("Failed to load image: " + url + "; error: " + www.error);
        }
    }
}
