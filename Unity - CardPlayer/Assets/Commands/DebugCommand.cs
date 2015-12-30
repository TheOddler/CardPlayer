using UnityEngine;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.Fields)]
public class DebugCommand : Command
{
	public void Do()
	{
		Debug.Log("<< Debug >>");
	}

	public override string ToString()
	{
		return "[DebugCommand]";
	}
}