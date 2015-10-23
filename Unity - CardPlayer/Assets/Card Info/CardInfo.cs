using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using System.Linq;
using UnityEngine.Assertions;

public class CardInfo
{
	public const string NAME_TOKEN = "name";
	
	private bool _gatheringScheduled = false;
	List<CardInfoGatherer> _unusedGatherers;
	HashSet<CardInfoGatherer> _busyGatherers = new HashSet<CardInfoGatherer>();
	
	//
	// Constructors
	// ---
	public CardInfo(string name)
	{
		_info.Add(NAME_TOKEN, new Updateable<string>(name, true));
		
		_unusedGatherers = CardInfoProvider.Get.InfoGatherersCopy();
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
				CardInfoProvider.Get.ImageGatherersCopy().First().GatherFor(this, OnImageGathered);
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
		//Debug.Log("Scheduled gathering for: " + Name);
		_gatheringScheduled = true;
		yield return new WaitForEndOfFrame();
		
		var missingIds = _info.Where(kvp => !kvp.Value.Ready).Select(kvp => kvp.Key);
		//Debug.Log("Mising ids for " + Name + " are: " + string.Join(", ", missingIds.ToArray()));
		CardInfoGatherer gatherer;
		while ( (gatherer = FindBestUnusedGathererFor(missingIds)) != null )
		// While there is a missing id that isn't already being gathered
		{
			// Capture gatherer in the loop, so it can be used in the lamba (1)
			var captured = gatherer;
			_unusedGatherers.Remove(captured);
			_busyGatherers.Add(captured);
			captured.GatherFor(this, dict => { OnInfoGathererFinished(dict, captured); }); //(1)
			//Debug.Log("Sending out a gatherer for " + Name + ": " + gatherer);
		}
		
		_gatheringScheduled = false;
		//Debug.Log("Finished sending out gatherers for: " + Name);
	}
	CardInfoGatherer FindBestUnusedGathererFor(IEnumerable<string> missingIds)
	{
		if (_unusedGatherers.Count <= 0)
		{
			return null;
		}
		
		// The one to gather is the first that isn't already being gathered
		string toGather = missingIds.FirstOrDefault(id => !_busyGatherers.Any(g => g.PotentialHits.Contains(id)));
		if (toGather != null)
		{
			// Find a gatherer to use, currently simply the first one fr this id
			var gatherer = _unusedGatherers.FirstOrDefault(g => g.PotentialHits.Contains(toGather));
			// Since I check for an id that isn't being gathered, the chosen gatherer can't be busy already
			Assert.IsFalse(_busyGatherers.Contains(gatherer));
			
			return gatherer;
		}
		else 
		{
			return null;
		}
		// TODO: Rewrite so the gatherer with most potential hits is used first
	}
	
	void OnInfoGathererFinished(Dictionary<string,string> foundValues, CardInfoGatherer usedGatherer)
	{
		if (foundValues == null)
		{
			Debug.Log("Info gathering failed for " + Name + " by: " + usedGatherer);
		}
		else
		{
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
		
		_busyGatherers.Remove(usedGatherer); 
		
		// check if any more gathering is needed.
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
	public HashSet<CardInfoGatherer> DebugBusyGatherers { get { return _busyGatherers; } }
}
