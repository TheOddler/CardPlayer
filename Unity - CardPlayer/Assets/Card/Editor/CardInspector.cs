using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Card))]
public class CardInspector : Editor
{
	override public void OnInspectorGUI()
	{
		Card card = target as Card;
		
		DrawDefaultInspector();
		
		GUILayout.Label("---");
		
		CardInfo cardInfo = card.DebugInfo;
		if (cardInfo != null && cardInfo.DebugInfo != null)
		{
			foreach (var pair in cardInfo.DebugInfo)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(pair.Key + ":");
				GUILayout.FlexibleSpace();
				GUILayout.TextField(pair.Value.Value);
				GUILayout.EndHorizontal();
			}
			
			GUILayout.Label("Busy Info Gatherers: ");
			foreach (var gatherer in cardInfo.DebugBusyGatherers)
			{
				GUILayout.Label("Gatherer: " + gatherer.TokenString.ToString());
			}
		}
		
		if (GUI.changed)
		{
			target.name = "Card - " + card.Name;
		}
	}
}
