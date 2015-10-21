using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using System.Linq;

public class CardInfo
{
	public const string NAME_TOKEN = "name";
	
	private bool _gatheringScheduled = false;
	MultiMap<string, CardInfoGatherer> _idsBeingGathered = new MultiMap<string, CardInfoGatherer>(); //contains all id's that are (potentially) being gathered, and by who.
	
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
			StartGatheringIfNonScheduled();
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
	void StartGatheringIfNonScheduled()
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
		var missingIds = _info.Where(kvp => !kvp.Value.Ready).Select(kvp => kvp.Key);
		var gatherers = CardInfoProvider.Get.InfoGatherersCopy(); //TODO: Remember which I already used, and don't use them again
		string firstMissing;
		while ((firstMissing = missingIds.FirstOrDefault(id => !_idsBeingGathered.ContainsKey(id))) != null)
		// While there is a missing id that isn't already being gathered
		{
			//TODO select best fitting one, rather than first one that fits.
			var firstGatherer = gatherers.First(g => g.PotentialHits.Contains(firstMissing));
			_idsBeingGathered.AddValueToKeys(firstGatherer.PotentialHits, firstGatherer);
			firstGatherer.GatherInfoFor(this, dict => { OnInfoGathererFinished(dict, firstGatherer); });
		}
		
		_gatheringScheduled = false;
	}
	
	void OnInfoGathererFinished(Dictionary<string,string> foundValues, CardInfoGatherer usedGatherer)
	{
		if (foundValues == null)
		{
			Debug.Log("Info gathering failed for: " + Name);
		}
		else
		{
			_idsBeingGathered.RemoveValueFromKeys(usedGatherer.PotentialHits, usedGatherer);
			foreach(var kvp in foundValues)
			{
				var value = GetById(kvp.Key);
				if(value.Ready)
				{
					if (value.Value != kvp.Value)
					{
						// TODO
						// What to do?
						// Update anyway?
						value.UpdateValue(kvp.Value);
					}
					//else everything is fine
				}
				else
				{
					value.UpdateValue(kvp.Value);
				}
			}
		}
		
		StartGatheringIfNonScheduled();
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
