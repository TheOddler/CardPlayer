using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Linq;

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
	private List<CardInfoGatherer> _gatherers;

	//
	// Data
	// ---
	private Dictionary<string, CardInfo> _knownInfo = new Dictionary<string, CardInfo>();

	//
	// Init
	// ---
	void Awake()
	{
		if (_instance != null) throw new UnityException("Multiple card info providers in the scene.");
		_instance = this;
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
		foreach (var gatherer in _gatherers)
		{
			bool infoSuccess = false;
			yield return StartCoroutine(gatherer.LoadInfoFor(card, s => { infoSuccess = s; }));

			if (!infoSuccess) continue;

			bool imageSuccess = false;
			yield return StartCoroutine(gatherer.LoadImageFor(card, s => { imageSuccess = s; }));

			if (/*infoSuccess &&*/ imageSuccess) break;
			Debug.Log("Trying another gatherer for: " + card.Name);
		}
	}
	
	
	
	void OnGUI() 
	{
		GUI.Label(new Rect(10,10,2000,30), "Data: " + Application.persistentDataPath);
	}
}
