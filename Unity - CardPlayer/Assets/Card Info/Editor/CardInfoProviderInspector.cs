using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardInfoProvider))]
public class CardInfoProviderInspector : Editor
{
	override public void OnInspectorGUI()
	{
		CardInfoProvider provider = target as CardInfoProvider;
		
		DrawDefaultInspector();
		
		GUILayout.Label("---");
		
		GUILayout.Label("Image Gatherers: ");
		foreach (var gatherer in provider.DebugImageGatherers)
		{
			GUILayout.Label(gatherer.TokenString.ToString());
		}
	}
}
