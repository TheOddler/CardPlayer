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
				GUILayout.Label(pair.Key + ":\t" + pair.Value.Value);
			}
		}
		
		if (GUI.changed)
		{
			target.name = "Card - " + card.Name;
		}
	}
}
