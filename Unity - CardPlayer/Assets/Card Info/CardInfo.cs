using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

	public ExtraCardInfo Extra { get; set; }

	//
	// Other info
	// ---
	public string SimpleName
	{
		get
		{
			return _name.SimplifySpecialCharacter().ToLowerInvariant();
		}
	}

	//
	// Interpreter
	// ---
	static readonly Regex TOKEN_REGEX = new Regex(@"{(.+)}");
	static readonly Dictionary<string, System.Func<CardInfo, string>> TOKENS = new Dictionary<string, System.Func<CardInfo, string>>(System.StringComparer.OrdinalIgnoreCase)
	{
		{"name", c => WWW.EscapeURL(c.Name)},
		{"sname", c => WWW.EscapeURL(c.SimpleName)},
		{"name_ne", c => c.Name},
		{"sname_ne", c => c.SimpleName}
	};

	public string FillInfoIn(string text)
	{
		return TOKEN_REGEX.Replace(text, match => TranslateToken(match.Groups[1].Value));
	}

	private string TranslateToken(string path)
	{
		// First see if the token one of my own tokens, so no jsonpath thing
		if (TOKENS.ContainsKey(path))
		{
			return TOKENS[path](this);
		}
		// If not, check the extra info
		else if (Extra != null)
		{
			return Extra.TranslateToken(path);
		}
		else
		{
			Debug.Log("Failed translating token. It's not a default token, and extra info is null.");
		}

		return "ERROR";
	}
}
