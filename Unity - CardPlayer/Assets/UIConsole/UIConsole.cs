using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class UIConsole : MonoBehaviour
{
	private Text _logText;

	void Awake()
	{
		_logText = GetComponent<Text>();
	}

	void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}
	void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type)
	{
		_logText.text += "\n" + logString;
	}
}
