using UnityEngine;
using System.Collections;

public static class Helpers
{	
	public static string ReplaceURLEscaped(this string url, string tag, string value)
	{
		return url.Replace(tag, WWW.EscapeURL(value));
	}
}
