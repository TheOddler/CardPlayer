using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using System.Linq;

public class CardInfo
{
	public const string NAME_TOKEN = "name";
	
	private bool _gatheringScheduled = false;
	List<string> _missingIds = new List<string>(); //contains all missing id's that aren't being gathered already
	
	//
	// Constructors
	// ---
	public CardInfo(string name)
	{
		_info.Add(NAME_TOKEN, new Updateable<string>(name, true));
	}
	
	//
	// Info
	// ---
	private Dictionary<string, Updateable<string>> _info = new Dictionary<string, Updateable<string>>(System.StringComparer.InvariantCultureIgnoreCase);
	
	public Updateable<string> GetById(string id)
	{
		if (!_info.ContainsKey(id))
		{
			_info.Add(id, new Updateable<string>());
			_missingIds.Add(id);
			StartGatheringIfNeeded();
		}
		return _info[id];
	}
	
	public Updateable<string> this[string id]
	{
		get
		{
			return GetById(id);
		}
	}
	
	//
	// Material
	// ---
	private Material _material = null;
	public Material Material
	{
		get
		{
			if (_material == null)
			{
				_material = CardInfoProvider.Get.FrontMaterialCopy();
				// TODO Don't just use the first one...
				CardInfoProvider.Get.ImageGatherersCopy().First().GatherImageFor(this, OnImageGathered);
			}
			return _material;
		}
	}
	
	void OnImageGathered(Texture2D tex)
	{
		Material.mainTexture = tex;
	}
	
	
	//
	// Gathering
	// ---
	void StartGatheringIfNeeded()
	{
		if (!_gatheringScheduled)
		{
			// We need a monobehaviour for coroutines, so we use the CardInfoProvider as a dummy for this
			CardInfoProvider.Get.StartCoroutine(DoGathering());
		}
	}
	IEnumerator DoGathering()
	{
		_gatheringScheduled = true;
		yield return new WaitForEndOfFrame();
		
		// TODO remember which gatherers were already used
		var gatherers = CardInfoProvider.Get.InfoGatherersCopy();
		while (_missingIds.Count > 0)
		{
			//TODO select best fitting one, rather than first one that fits.
			string firstWanted = _missingIds[0];
			var firstGatherer = gatherers.First(g => g.PotentialHits.Contains(firstWanted));
			_missingIds.RemoveAll(firstGatherer.PotentialHits.Contains);
			//TODO I assume all 'potential hits' were actual hits.
			firstGatherer.GatherInfoFor(this, OnInfoGathererFinished);
		}
		
		_gatheringScheduled = false;
	}
	
	void OnInfoGathererFinished(Dictionary<string,string> foundValues)
	{
		if (foundValues == null)
		{
			Debug.Log("Info gathering failed for: " + Name);
		}
		else
		{
			//TODO I assume all 'potential hits' were actual hits.
			foreach(var kvp in foundValues)
			{
				GetById(kvp.Key).UpdateValue(kvp.Value);
			}
		}
	}
	
	
	//
	// Convenience functions
	// ---
	public string Name
	{
		get
		{
			return _info[NAME_TOKEN].Value;
		}
	}
	
	//
	// Debugging
	// ---
	public Dictionary<string, Updateable<string>> DebugInfo { get { return _info; } }
}
