using UnityEngine;
using System.Collections;

public class Seat : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	void Awake()
	{
		// Activated when selected
		gameObject.SetActive(false);
	}
}
