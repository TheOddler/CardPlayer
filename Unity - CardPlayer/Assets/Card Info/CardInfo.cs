using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardInfo
{
	//
	// Constructors
	// ---
	public CardInfo(string name, Material mat)
	{
		_name = name;
		_material = mat;
	}

	//
	// Info
	// ---
	private string _name;
	public string Name
	{
		get { return _name; }
	}

	private Material _material;
	public Material Material
	{
		get { return _material; }
	}
	
	//
	// Extra Info
	// ---
	private List<ExtraCardInfo> _extras = new List<ExtraCardInfo>();
	public void AddExtraInfo(ExtraCardInfo extra)
	{
		_extras.Add(extra);
	}
	
	private static readonly Dictionary<string, System.Func<CardInfo, string>> DEFAULT_TOKENS = new Dictionary<string, System.Func<CardInfo, string>>(System.StringComparer.OrdinalIgnoreCase)
	{
		{ "name", 		c => WWW.EscapeURL(c.Name) },
		{ "sname", 		c => WWW.EscapeURL(c.Name.SimplifySpecialCharacter().ToLowerInvariant()) },
		{ "name_ne", 	c => c.Name },
		{ "sname_ne", 	c => c.Name.SimplifySpecialCharacter().ToLowerInvariant() }
	};
	public string GetExtraInfoById(string id)
	{
		if (DEFAULT_TOKENS.ContainsKey(id)) {
			return DEFAULT_TOKENS[id](this);
		}
		else 
		{
			foreach(var extra in _extras)
			{
				var info = extra.GetById(id);
				if (info != null) return info;
			}
		}
		return null;
	}
}
