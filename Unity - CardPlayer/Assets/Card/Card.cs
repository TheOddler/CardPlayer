using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class Card : MonoBehaviour
{

    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    private CardInfo _info;

    void Start()
    {
	    _info = CardDatabase.Instance.GetByName(_name);
	    GetComponent<Renderer>().material = _info.Material;
    }
}
