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
	private Queue<CardInfo> _gatheringQueue = new Queue<CardInfo>();
	
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
	
	void Start()
	{
		StartCoroutine(GatheringLoop());
	}
	
	//
	// Getters
	// ---
	public CardInfo ByName(string name)
	{
		if (!_knownInfo.ContainsKey(name))
		{
			Material mat = new Material(_frontMaterial); // Copy of the default front material, the actual image will be loaded into this
			CardInfo card = new CardInfo(name, mat);
			_knownInfo[name] = card; // Remember this for later
		}
		
		return _knownInfo[name];
	}
	
	//
	// Gathering
	// ---
	public void RequestUpdateFor(CardInfo card)
	{
		if (!_gatheringQueue.Contains(card))
		{
			_gatheringQueue.Enqueue(card);
		}
	}
	
	IEnumerator GatheringLoop()
	{
		yield return new WaitForEndOfFrame();
		while(true)
		{
			if(_gatheringQueue.Count <= 0)
			{
				yield return null;
			}
			else
			{
				var card = _gatheringQueue.Dequeue();
				
				// Gather all possible info. TODO: only gather what is really needed
				foreach (var gatherer in _infoGatherers)
				{
					yield return StartCoroutine(gatherer.LoadInfoFor(card));
				}
			}
		}
	}
	
	
	
	void OnGUI() 
	{
		GUI.Label(new Rect(10,10,2000,30), "Data: " + Application.persistentDataPath);
	}
}
