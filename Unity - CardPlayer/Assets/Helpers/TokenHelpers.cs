using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class TokenHelpers
{
	private static readonly Regex TOKEN_REGEX = new Regex(@"{(.+)}");
	
	public static string FillInfoIn(string text, CardInfo info)
	{
		return TOKEN_REGEX.Replace(text, match => 
				info.GetExtraInfoById(match.Groups[1].Value)
			);
	}
}