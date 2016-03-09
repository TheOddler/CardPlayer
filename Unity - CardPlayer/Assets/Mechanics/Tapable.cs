using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System.Collections;

public class Tapable : NetworkBehaviour, IPointerClickHandler
{
	const float TAP_ANGLE = 45f;

	[SyncVar(hook = "SetTapped")]
	public bool _tapped;
	void SetTapped(bool tapped)
	{
		_tapped = tapped;
		UpdateRotation();
	}
	[Command]
	void CmdSetTapped(bool tapped)
	{
		SetTapped(tapped);
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

	private void UpdateRotation()
	{
		float rotation = _tapped ? TAP_ANGLE : 0;
		transform.localRotation = Quaternion.Euler(0, rotation, 0);
	}
}
