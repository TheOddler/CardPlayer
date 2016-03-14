﻿using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
	[SerializeField]
	private Card _cardPrefab;
	
	[SyncVar]
	private Seat _seat;
	public Seat Seat { get { return _seat; } }
	
	public void InitializeAsPlayer(Seat seat)
	{
		
	}
	
	void Update()
	{
		if (!isLocalPlayer) return;

		/*if (Input.GetKeyDown(KeyCode.Space))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				CmdSpawnCard("Ponder", hit.point);
			}
		}*/
	}

	/*[Command]
	void CmdSpawnCard(string name, Vector3 position)
	{
		Card card = Instantiate(_cardPrefab);
		card.transform.position = position;
		card.Name = name;
		NetworkServer.SpawnWithClientAuthority(card.gameObject, gameObject);
	}*/
		
	void OnGUI()
	{
		//Temp fix for crash when building WebGL with codestripping enabled.
		//Should be fixed in Unity 5.3.
	}
}
