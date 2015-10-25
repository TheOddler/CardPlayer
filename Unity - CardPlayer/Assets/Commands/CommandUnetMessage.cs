using UnityEngine;
using UnityEngine.Networking;

public class CommandUnetMessage : MessageBase
{
	public Command Command { get; private set; }
	
	public CommandUnetMessage(Command command)
	{
		Command = command;
	}
}
