using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;

public class Tester : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		JToken jtoken = JToken.Parse(
			@"{""cards"":[{""name"":""Krark-Clan Engineers"",""manaCost"":""{3}{R}"",""cmc"":4,""colors"":[""Red""],""type"":""Creature — Goblin Artificer"",""types"":[""Creature""],""subtypes"":[""Goblin"",""Artificer""],""rarity"":""Uncommon"",""set"":""5DN"",""text"":""{R}, Sacrifice two artifacts: Destroy target artifact."",""flavor"":""\""Well, I jammed the whatsit into the whackdoodle, but I think I broke the thingamajigger.\"""",""artist"":""Pete Venters"",""number"":""70"",""power"":""2"",""toughness"":""2"",""layout"":""normal"",""multiverseid"":50201,""imageUrl"":""http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid=50201&type=card"",""foreignNames"":[{""name"":""喀勒克族机械工"",""language"":""Chinese Simplified"",""multiverseid"":81620},{""name"":""Ingénieurs du clan Krark"",""language"":""French"",""multiverseid"":80795},{""name"":""Ingenieure des Krark-Clans"",""language"":""German"",""multiverseid"":80960},{""name"":""Ingegneri di Krark-Clan"",""language"":""Italian"",""multiverseid"":81290},{""name"":""クラーク族の技師"",""language"":""Japanese"",""multiverseid"":80630},{""name"":""Engenheiros do Clã-de-Krark"",""language"":""Portuguese (Brazil)"",""multiverseid"":81455},{""name"":""Ingenieros del clan Krark"",""language"":""Spanish"",""multiverseid"":81125}],""printings"":[""5DN""],""originalText"":""{R}, Sacrifice two artifacts: Destroy target artifact."",""originalType"":""Creature — Goblin Artificer"",""legalities"":[{""format"":""Commander"",""legality"":""Legal""},{""format"":""Freeform"",""legality"":""Legal""},{""format"":""Legacy"",""legality"":""Legal""},{""format"":""Mirrodin Block"",""legality"":""Legal""},{""format"":""Modern"",""legality"":""Legal""},{""format"":""Prismatic"",""legality"":""Legal""},{""format"":""Singleton 100"",""legality"":""Legal""},{""format"":""Tribal Wars Legacy"",""legality"":""Legal""},{""format"":""Vintage"",""legality"":""Legal""}],""id"":""a4d05fd27ec5d7df470e91218f1ca885eda4f0c6""}]}"
		);

		var foundTokens = jtoken.SelectTokens(@"$..cards[?(@.name=""Krark - Clan Engineers"")].imageUrl", true);
		if (foundTokens.Any())
		{
			string selected = foundTokens.Last().ToString();
			Debug.Log("Found: " + selected);
		}
		else
		{
			Debug.Log("Found nothing!");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
