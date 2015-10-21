using System.Collections.Generic;

public abstract class CardInfoGatherer: WWWGatherer<Dictionary<string,string>>
{
	public CardInfoGatherer(string baseUrl) : base(baseUrl) { }
	
	abstract public ICollection<string> PotentialHits { get; }
}
