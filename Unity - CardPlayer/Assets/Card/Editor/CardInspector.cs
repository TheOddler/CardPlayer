using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Card))]
public class CardInspector : Editor
{
    override public void OnInspectorGUI()
    {
        Card card = target as Card;

        DrawDefaultInspector();
        
        if (GUI.changed)
        {
            target.name = "Card - " + card.Name;
        }
    }
}
