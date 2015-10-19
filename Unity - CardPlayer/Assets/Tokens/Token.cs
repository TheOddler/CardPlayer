
public class Token
{
	private string _id;
	public string ID { get { return _id; } }
	
	public Token(string token)
	{
		_id = token.Trim();
	}
	
	public string GetValueFrom(CardInfo card)
	{
		return card.GetExtraInfoById(_id).Value;
	}
}
