using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardInfo
{
	public const string NAME_TOKEN = "name";
	
	//
	// Constructors
	// ---
	public CardInfo(string name, Material mat)
	{
		_info.Add(NAME_TOKEN, new Updateable<string>(name, true));
		_material = mat;
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
			CardInfoProvider.Get.RequestUpdateFor(this);
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
	
	public IEnumerable<string> MissingInfo
	{
		get { return _info.Where(kvp => kvp.Value == null).Select(kvp => kvp.Key); }
	}
	
	//
	// Material
	// ---
	private Material _material;
	public Material Material
	{
		get { return _material; }
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
