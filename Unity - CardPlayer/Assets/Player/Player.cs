using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Player : NetworkBehaviour
{
	[SerializeField]
	private Card _cardPrefab;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		if (!isLocalPlayer) return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				CmdSpawnCard("Ponder", hit.point);
			}
		}
	}

	[Command]
	void CmdSpawnCard(string name, Vector3 position)
	{
		Card card = Instantiate(_cardPrefab);
		card.transform.position = position;
		card.Name = name;
		NetworkServer.SpawnWithClientAuthority(card.gameObject, gameObject);
	}
}
