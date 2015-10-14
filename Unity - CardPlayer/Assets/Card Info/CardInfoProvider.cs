using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CardInfoProvider : MonoBehaviour
{
	//
	// Singleton
	// ---
	private static CardInfoProvider _instance;
	public static CardInfoProvider Get
	{
		get { return _instance; }
	}

	//
	// Settings
	// ---
	[SerializeField]
	private Material _frontMaterial;
	
	[SerializeField]
	private TextAsset _infoGatherersFile;
	
	[SerializeField]
	private TextAsset _imageGatherersFile;

	//
	// Data
	// ---
	private List<CardInfoGatherer> _infoGatherers = new List<CardInfoGatherer>();
	private List<CardImageGatherer> _imageGatherers = new List<CardImageGatherer>();
	private Dictionary<string, CardInfo> _knownInfo = new Dictionary<string, CardInfo>();

	//
	// Init
	// ---
	void Awake()
	{
		if (_instance != null) throw new UnityException("Multiple card info providers in the scene.");
		_instance = this;
		
		var autoTypeNameHandling = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
		_infoGatherers = JsonConvert.DeserializeObject<List<CardInfoGatherer>>(_infoGatherersFile.text, autoTypeNameHandling);
		_imageGatherers = JsonConvert.DeserializeObject<List<CardImageGatherer>>(_imageGatherersFile.text, autoTypeNameHandling);
	}

	//
	// Getters
	// ---
	public CardInfo ByName(string name)
	{
		// If we already have info for this card return that
		if (_knownInfo.ContainsKey(name))
		{
			return _knownInfo[name];
		}
		// Otherwise create default info and gather its extra info and image
		else
		{
			Material mat = new Material(_frontMaterial); // Copy of the default front material, the actual image will be loaded into this
			CardInfo card = new CardInfo(name, mat);
			_knownInfo[name] = card; // Remember this for later
	
			LoadInfoAndImageFor(card);
	
			return card;
		}
	}

	//
	// Info Gatherers
	// ---
	void LoadInfoAndImageFor(CardInfo card)
	{
		StartCoroutine(TryLoadInfoAndImageFor(card));
	}
	
	IEnumerator TryLoadInfoAndImageFor(CardInfo card)
	{
		// First let all gatherers gather info
		foreach (var gatherer in _infoGatherers)
		{
			yield return StartCoroutine(gatherer.LoadInfoFor(card));
		}
		
		// Now try each image gatherer one by one to get an image
		foreach (var gatherer in _imageGatherers)
		{
			bool success = false;
			yield return StartCoroutine(gatherer.LoadImageFor(card, s => { success = s; }));
			if (success) break;
			//Debug.Log("Trying another image gatherer for: " + card.Name);
		}
	}
	
	
	
	void OnGUI() 
	{
		GUI.Label(new Rect(10,10,2000,30), "Data: " + Application.persistentDataPath);
	}
}
