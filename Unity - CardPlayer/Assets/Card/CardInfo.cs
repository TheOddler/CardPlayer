using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

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

    public JObject Extra { get; set; }

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
        return TOKEN_REGEX.Replace(text, match => GetTokenValue(match.Groups[1].Value));
    }

    private string GetTokenValue(string token)
    {
        // First see if the token one of my own tokens, so no jsonpath thing
        if (TOKENS.ContainsKey(token))
        {
            return TOKENS[token](this);
        }
        else // If not, treat is as a jsonpath and try to find the info
        {
            try
            {
                var valueArray = Extra.SelectTokens(token);
                //Debug.Log("Getting token (" + token + ") out of " + Extra);
                //Debug.Log("Gotten value array is: " + valueArray);
                return valueArray.First().ToString();
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed getting token: " + e.Message);
                return "ERROR";
            }
        }
    }
}
