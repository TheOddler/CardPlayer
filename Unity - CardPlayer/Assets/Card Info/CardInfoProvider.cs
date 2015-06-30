using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;
using System.Collections.Generic;
using System.Linq;

public class CardInfoProvider : MonoBehaviour
{
    //
    // Singleton
    // ---
    private static CardInfoProvider _instance;
    public static CardInfoProvider Get
    {
        get { return _instance; }
    }

    //
    // Settings
    // ---
    [SerializeField]
    private Material _frontMaterial;

    [SerializeField]
    private List<CardInfoGatherer> _gatherers;

    //
    // Data
    // ---
    private Dictionary<string, CardInfo> _knownInfo = new Dictionary<string, CardInfo>();

    //
    // Init
    // ---
    void Awake()
    {
        if (_instance != null) throw new UnityException("Multiple card info providers in the scene.");
        _instance = this;
    }

    //
    // Getters
    // ---
    public CardInfo ByName(string name)
    {
        return GetExistingOrDefault(name) ?? CreateCardInfoFor(name);
    }
    // Checks if there is an existing one, if not, returns null
    private CardInfo GetExistingOrDefault(string name)
    {
        if (_knownInfo.ContainsKey(name))
        {
            return _knownInfo[name];
        }
        return null;
    }
    // Creates card info, also starts routines to get additional info
    private CardInfo CreateCardInfoFor(string name)
    {
        Material mat = new Material(_frontMaterial);
        CardInfo card = new CardInfo(name, mat);
        _knownInfo[name] = card;

        LoadInfoAndImageFor(card);

        return card;
    }

    //
    // Info Gatherers
    // ---
    void LoadInfoAndImageFor(CardInfo card)
    {
        StartCoroutine(TryLoadInfoAndImageFor(card));
    }

    IEnumerator TryLoadInfoAndImageFor(CardInfo card)
    {
        foreach (var gatherer in _gatherers)
        {
            bool infoSuccess = false;
            yield return StartCoroutine(gatherer.LoadInfoFor(card, s => { infoSuccess = s; }));

            if (!infoSuccess) continue;

            bool imageSuccess = false;
            yield return StartCoroutine(gatherer.LoadImageFor(card, s => { imageSuccess = s; }));

            if (/*infoSuccess &&*/ imageSuccess) break;
            Debug.Log("Trying another gatherer for: " + card.Name);
        }
    }
}
