using System.Collections.Generic;

public interface CardInfoGatherer
{
	ICollection<string> PotentialHits { get; }
	
	void GatherInfoFor(CardInfo cardInfo, System.Action<Dictionary<string,string>> onFinished);
}
