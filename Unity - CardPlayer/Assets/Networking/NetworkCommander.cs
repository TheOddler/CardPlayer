using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkCommander : MonoBehaviour
{
	public static NetworkCommander Get { get; private set; }

	private NetworkClient _client;
	
	void Start()
	{
		if (Get != null) throw new UnityException("Multiple NetworkCommanders in the scene, please make sure there is exactly one.");
		Get = this;
		
		// Server
		NetworkServer.RegisterHandler(NetworkCommandMessage.Type, OnServerMessageRecieved);

		// Client
		_client = NetworkManager.singleton.client;
		_client.RegisterHandler(NetworkCommandMessage.Type, OnClientMessageRecieved);
	}

	void OnServerMessageRecieved(NetworkMessage netMsg)
	{
		var commandMessage = netMsg.ReadMessage<NetworkCommandMessage>();
		Debug.Log("On Server Received Message: " + commandMessage);
		// decide what to do with the message
		// check if it is valid, ...
		// for now just forward to everyone else
		NetworkServer.SendToAll(NetworkCommandMessage.Type, commandMessage);
	}

	void OnClientMessageRecieved(NetworkMessage netMsg)
	{
		var commandMessage = netMsg.ReadMessage<NetworkCommandMessage>();
		Command command = commandMessage.Command;
		Debug.Log("On Client Received Message: " + commandMessage);
		// execute command here
		command.Do();
	}


	public void SendCommand(Command command)
	{
		var message = new NetworkCommandMessage(command);
		Debug.Log("Sending Command: " + command + " as the message: " + message);
		_client.Send(NetworkCommandMessage.Type, message);
	}

	// Undo & Redo
	
}
