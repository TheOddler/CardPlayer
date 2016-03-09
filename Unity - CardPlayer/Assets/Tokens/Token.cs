using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class Token
{
	const string PATTERN = @"\{(?:\s*(\w+)\s*)+\}";
	public static readonly Regex REGEX = new Regex(PATTERN);
	
	private string _id;
	public string ID { get { return _id; } }
	
	private List<string> _settings = new List<string>();
	
	public Token(Match regexMatch)
	{
		// Group 0 is everything, group 1 is everything between the {}, the captures are the individual words
		List<Capture> captures = regexMatch.Groups[1].Captures.Cast<Capture>().ToList();
		// all but the last word are settings
		int settingsCount = captures.Count - 1;
		for(int i = 0; i < settingsCount; ++i)
		{
			_settings.Add(captures[i].Value.ToLowerInvariant());
		}
		// the id is the last word
		_id = captures[captures.Count - 1].Value;
	}
	
	public string GetValueFrom(CardInfo cardInfo)
	{
		string value = cardInfo.GetById(_id).Value;
		foreach(var setting in _settings)
		{
			switch (setting)
			{
				case "urlescaped":
				case "ue":
					value = WWW.EscapeURL(value);
					break;
				case "quoteescaped":
				case "qe":
					value = value.Replace(@"""", @"\""").Replace("'", @"\'");
				break;
				case "lower":
				case "l":
					value = value.ToLowerInvariant();
					break;
				case "upper":
				case "u":
					value = value.ToUpperInvariant();
					break;
				default:
					Debug.Log("Unknown setting: " + setting);
					break;
			}
		}
		
		return value;
	}
}
