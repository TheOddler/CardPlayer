using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardInfo
{    
    //
    // Constants
    // ---
    private static readonly Dictionary<string, string> SpecialMapping = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
    {
        {"œ", "oe"},
        {"Ÿ", "Y"},
        {"À", "A"}, {"Á", "A"}, {"Â", "A"}, {"Ã", "A"}, {"Ä", "A"},
        {"Æ", "AE"},
        {"é", "e"},
    };

    //
    // Constructors
    // ---
    private CardInfo(string name, string simpleName)
    {
        _name = name;
        _simpleName = simpleName;
    }

    //
    // Info
    // ---
    private string _name;
    /**
     * The card's anme.
     * It's the string the first use was initialized with
     */
    public string Name
    {
        get { return _name; }
    }

    private string _simpleName;
    /**
     * A simpler version of the name
     * replaces all special letters
     */
    public string SimpleName
    {
        get { return _simpleName; }
    }

    private static string SimplifyName(string name)
    {
        return SpecialMapping.Aggregate(name, (res, s) => res.Replace(s.Key, s.Value)).ToLowerInvariant();
    }

    //
    // Create
    // ---
    public static class Get
    {
        private static Dictionary<string, CardInfo> _knownInfo = new Dictionary<string, CardInfo>();

        public static CardInfo ByName(string name)
        {
            string nameSimple = SimplifyName(name);
            
            CardInfo card;
            if (_knownInfo.ContainsKey(nameSimple))
            {
                card = _knownInfo[nameSimple];
            }
            else
            {
                card = new CardInfo(name, nameSimple);
                _knownInfo[nameSimple] = card;
            }

            return card;
        }
    }
}
