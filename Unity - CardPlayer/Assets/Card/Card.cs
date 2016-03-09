using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

[RequireComponent(typeof(Renderer))]
public class Card : NetworkBehaviour, IDragHandler
{
	//
	// Info
	// ---
	[SyncVar]
	[SerializeField]
	private string _name;
	public string Name
	{
		get { return _name; }
		set { _name = value; }
	}

	private CardInfo _info;

	//
	// Initialization
	// ---
	void Start()
	{
		_info = CardInfoProvider.Get.ByName(_name);
		SetMaterial();
	}

	//
	// Events
	// ---
	public void OnDrag(PointerEventData eventData)
	{
		//Debug.Log("Hey");
	}

	//
	// Visuals
	// ---
	private void SetMaterial()
	{
		Assert.IsNotNull(_info);
		GetComponent<Renderer>().material = _info.Material;
	}
	
	//
	// Debugging
	// ---
	public CardInfo DebugInfo { get { return _info; } }
}
