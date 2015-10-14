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
		_info.Add(NAME_TOKEN, new Updateable<string>(name));
		_material = mat;
	}
	
	//
	// Info
	// ---
	private Dictionary<string, Updateable<string>> _info = new Dictionary<string, Updateable<string>>(System.StringComparer.InvariantCultureIgnoreCase);
	
	private Material _material;
	public Material Material
	{
		get { return _material; }
	}
	
	public IEnumerable<string> MissingInfo
	{
		get { return _info.Where(kvp => kvp.Value == null).Select(kvp => kvp.Key); }
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
	// Methods
	// ---
	public Updateable<string> GetExtraInfoById(string id)
	{
		if (!_info.ContainsKey(id))
		{
			_info.Add(id, new Updateable<string>());
			CardInfoProvider.Get.RequestUpdateFor(this);
		}
		return _info[id];
	}
	
	/*private static readonly Dictionary<string, System.Func<CardInfo, string>> DEFAULT_TOKENS = new Dictionary<string, System.Func<CardInfo, string>>(System.StringComparer.OrdinalIgnoreCase)
	{
		{ "name", 		c => WWW.EscapeURL(c.Name) },
		{ "sname", 		c => WWW.EscapeURL(c.Name.SimplifySpecialCharacter().ToLowerInvariant()) },
		{ "name_ne", 	c => c.Name },
		{ "sname_ne", 	c => c.Name.SimplifySpecialCharacter().ToLowerInvariant() }
	};*/
}
