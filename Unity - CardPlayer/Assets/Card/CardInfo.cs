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

    public JSONObject Extra { get; set; }

    private string _simpleName;
    public string SimpleName
    {
        get
        {
            if (_simpleName == null)
            {
                _simpleName = _name.SimplifySpecialCharacter();
            }
            return _simpleName;
        }
    }

    //
    // Interpreter
    // ---
    static readonly Regex DB_TOKEN_REGEX = new Regex(@"{{(.+)}}");
    static readonly Regex TOKEN_REGEX = new Regex(@"{(.+)}");
    static readonly Dictionary<string, System.Func<CardInfo, string>> TOKENS = new Dictionary<string, System.Func<CardInfo, string>>(System.StringComparer.OrdinalIgnoreCase)
    {
        {"name", c => c.Name},
        {"sname", c => c.SimpleName}
    };

    public string FillInfoIn(string text, bool escapeForUrl = false)
    {
        string firstPass = DB_TOKEN_REGEX.Replace(text, match => GetFromDatabase(match.Groups[1].Value, escapeForUrl));
        return TOKEN_REGEX.Replace(firstPass, match => GetTokenValue(match.Groups[1].Value, escapeForUrl));
    }

    private string GetTokenValue(string token, bool escapeForUrl)
    {
        string value = TOKENS[token](this);
        return escapeForUrl ? WWW.EscapeURL(value) : value;
    }

    private string GetFromDatabase(string token, bool escapeForUrl = false)
    {
        return ""; //TODO
    }
}
