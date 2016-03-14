using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameManager : NetworkManager
{
	private List<Player> _players = new List<Player>();
	public List<Player> Players { get { return _players; } }

	[SerializeField]
	private List<Seat> _seats;
	public List<Seat> Seats { get { return _seats; } }


}
