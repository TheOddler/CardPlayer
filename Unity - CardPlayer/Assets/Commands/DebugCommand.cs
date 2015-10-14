using UnityEngine;

public class DebugCommand : Command
{
	public void Do()
	{
		Debug.Log("<< Debug >>");
	}
}