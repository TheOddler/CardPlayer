using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardDatabase : MonoBehaviour
{
	//
	// Tags
	// ---
	const string NAME_TAG = "[NAME]";
	const string SNAME_TAG = "[SNAME]";
	const string ID_TAG = "[ID]";
	
	//
    // Constants
    // ---
	private static readonly Dictionary<string, string> SpecialMapping = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
	{
		{"œ", "oe"},
		{"Ÿ", "Y"},
		{"À", "A"}, {"Á", "A"}, {"Â", "A"}, {"Ã", "A"}, {"Ä", "A"},
		{"Æ", "AE"},
		{"é", "e"},
		{"/",""},
    };

	//
	// Settings
	// ---
	[SerializeField, TextArea]
	private string _imageUrlBase = "https://deckbox.org/mtg/[SNAME]/tooltip.jpg";
	
	[SerializeField, TextArea]
	private string _infoUrlBase = "http://api.mtgdb.info/cards/[SNAME]";

	[SerializeField]
	private Material _frontMaterial;

	//
	// Data
	// ---
	private static Dictionary<string, CardInfo> _knownInfo = new Dictionary<string, CardInfo>();

	//
	// Singleton
	// ---
	private static CardDatabase _instance;
	public static CardDatabase Instance
	{
		get { return _instance; }
	}

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
	public CardInfo GetByName(string name)
	{
		string nameSimple = SimplifyName(name);

		CardInfo card;
		if (_knownInfo.ContainsKey(nameSimple))
		{
			card = _knownInfo[nameSimple];
		}
		else
		{
			Material mat = new Material(_frontMaterial);
			card = new CardInfo(name, mat);
			_knownInfo[nameSimple] = card;
			
			StartCoroutine(LoadInfoFor(card));
		}

		return card;
	}
	
	//
	// Info
	// ---
	IEnumerator LoadInfoFor(CardInfo card)
	{
		if (ImagesRequireAditionalInfo())
		{
			yield return StartCoroutine(LoadAdditionalInfoFor(card));
		}
		yield return StartCoroutine(LoadImageFor(card));
	}

	//
	// Additional info
	// ---
	IEnumerator LoadAdditionalInfoFor(CardInfo card)
	{
		string url = FillTagsInUrl(_infoUrlBase, card);
		WWW www = new WWW(url);

		yield return www;

		if (www.error == null)
		{
			JSONObject json = new JSONObject(www.text);
			JSONObject info = null;
			// select card
			if (json.IsArray)
			{
				info = json[0];
			}
			else
			{
				info = json;
			}
			
			JSONObject cardNumber = info["id"];
			string id;
			if (cardNumber.IsString) {
				id = cardNumber.str;
			}
			else {
				id = cardNumber.ToString();
			}
			
			card.Id = id;
		}
		else
		{
			Debug.Log("Failed to load info: " + url + "; error: " + www.error + "\n" + www.text);
		}
	}

	//
	// Images
	// ---
	private bool ImagesRequireAditionalInfo()
	{
		return _imageUrlBase.Contains(ID_TAG);
	}
	IEnumerator LoadImageFor(CardInfo card)
	{
		string url = FillTagsInUrl(_imageUrlBase, card);
		
		WWW www = new WWW(url);
		yield return www;

		if (www.error == null)
		{
			var texture = new Texture2D(1, 1); //, TextureFormat.DXT1, false);
			www.LoadImageIntoTexture(texture as Texture2D);
			texture.anisoLevel = 4;
			
			card.Material.mainTexture = texture;
		}
		else
		{
			Debug.Log("Failed to load image for " + card.Name + " from " + url + " with error: " + www.error);
		}
	}

	//
	// Helpers
	// ---
	private static string SimplifyName(string name)
	{
		return SpecialMapping.Aggregate(name, (res, s) => res.Replace(s.Key, s.Value)).ToLowerInvariant();
	}

	static string FillTagsInUrl(string url, CardInfo card)
	{
		return url
			.ReplaceURLEscaped(NAME_TAG, card.Name)
			.ReplaceURLEscaped(SNAME_TAG, SimplifyName(card.Name))
			.ReplaceURLEscaped(ID_TAG, card.Id);
	}
}
