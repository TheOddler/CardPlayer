using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.Fields)]
public class DebugMessageCommand : Command
{
	private string _message;
	
	public DebugMessageCommand(string message)
	{
		_message = message;
	}
	
	public void Do()
	{
		Debug.Log("<< Debug: " + _message + " >>");
	}

	public override string ToString()
	{
		return "[DebugMessageCommand: " + _message + "]";
	}
}