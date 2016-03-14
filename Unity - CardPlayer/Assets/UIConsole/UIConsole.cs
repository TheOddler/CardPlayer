using UnityEngine;
using UnityEngine.UI;

public class UIConsole : MonoBehaviour
{
	[SerializeField]
	private Text _logText;

	void Awake()
	{
		if (_logText.raycastTarget)
		{
			Debug.Log("Console is clickable, click won't go through to other object!");
		}
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
