using UnityEngine;
//using IEnumerator = System.Collections.IEnumerator;
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
	public List<CardImageGatherer> DebugImageGatherers { get { return _imageGatherers; } }
	
	private Dictionary<string, CardInfo> _knownInfo = new Dictionary<string, CardInfo>();
	
	//
	// Init
	// ---
	void Awake()
	{
		if (_instance != null) throw new UnityException("Multiple card info providers in the scene.");
		_instance = this;
		
		var autoTypeNameHandling = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.Auto
		};
		_infoGatherers = JsonConvert.DeserializeObject<List<CardInfoGatherer>>(_infoGatherersFile.text, autoTypeNameHandling);
		_imageGatherers = JsonConvert.DeserializeObject<List<CardImageGatherer>>(_imageGatherersFile.text, autoTypeNameHandling);
	}
	
	//
	// Getters
	// ---
	public CardInfo ByName(string name)
	{
		if (!_knownInfo.ContainsKey(name))
		{
			_knownInfo[name] = new CardInfo(name);
		}
		
		return _knownInfo[name];
	}
	
	public Material FrontMaterialCopy()
	{
		return new Material(_frontMaterial);
	}
	
	public List<CardInfoGatherer> InfoGatherersCopy()
	{
		return new List<CardInfoGatherer>(_infoGatherers);
	}
	public List<CardImageGatherer> ImageGatherersCopy()
	{
		return new List<CardImageGatherer>(_imageGatherers);
	}
	
	
	
	void OnGUI() 
	{
		GUI.Label(new Rect(10,10,2000,30), "Data: " + Application.persistentDataPath);
	}
}
