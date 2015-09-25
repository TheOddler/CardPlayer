using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

[RequireComponent(typeof(Renderer))]
public class Card : NetworkBehaviour, IDragHandler, IPointerClickHandler
{
	const float TAP_ANGLE = 45f;

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
	// Mechanics
	// ---
	[SyncVar(hook = "SetTapped")]
	public bool _tapped;
	public void SetTapped(bool tapped) // no property so it works with unet.
	{
		_tapped = tapped;
		UpdateRotation();
	}
	[Command]
	void CmdSetTapped(bool tapped)
	{
		SetTapped(tapped);
	}

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

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.clickCount % 2 == 0)
		{
			if (hasAuthority)
			{
				CmdSetTapped(!_tapped);
			}
		}
	}

	//
	// Visuals
	// ---
	private void SetMaterial()
	{
		Assert.IsNotNull(_info);
		GetComponent<Renderer>().material = _info.Material;
	}

	private void UpdateRotation()
	{
		float rotation = 0f;
		if (_tapped)
		{
			rotation += TAP_ANGLE;
		}
		// Other options that change the rotation
		transform.localRotation = Quaternion.Euler(0, rotation, 0);
	}
}
